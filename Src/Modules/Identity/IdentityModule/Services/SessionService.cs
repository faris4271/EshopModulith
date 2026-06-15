using IdentityModule.Data;
using IdentityModule.Domain;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Services;
using Shared.Contract.Context;
using UAParser;

namespace IdentityModule.Services
{
    public class SessionService : ISessionService
    {
        private readonly IdentityDbContext _db;
        private readonly ICurrentUser _currentUser;

        private readonly ILogger<SessionService> _logger;
        private readonly Parser _uaParser;

        public SessionService(IdentityDbContext db,
            ICurrentUser currentUser, ILogger<SessionService> logger, Parser uaParser)
        {
            _db = db;
            _currentUser = currentUser;
            _logger = logger;
            _uaParser = uaParser;
        }

        public Task CleanupExpiredSessionsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UserSessionDto> CreateSessionAsync(
     string userId,
     string refreshTokenHash,
     string ipAddress,
     string userAgent,
     DateTime expiresAt,
     CancellationToken cancellationToken = default)
        {


            var clientInfo = _uaParser.Parse(userAgent);

            var session = UserSession.Create(
                userId: userId,
                refreshTokenHash: refreshTokenHash,
                ipAddress: ipAddress,
                userAgent: userAgent,
                expiresAt: expiresAt,
                deviceType: DeviceTypeClassifier.Classify(clientInfo.Device.Family),
                browser: clientInfo.UA.Family,
                browserVersion: clientInfo.UA.Major,
                operatingSystem: clientInfo.OS.Family,
                osVersion: clientInfo.OS.Major);

            _db.UserSessions.Add(session);
            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created session {SessionId} for user {UserId}", session.Id, userId);

            return MapToDto(session, isCurrentSession: true);
        }

        public async Task<UserSessionDto?> GetSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            var session = await _db.UserSessions
                .AsNoTracking()
                  .Include(s => s.User)
                      .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

            return session is null ? null : MapToDto(session, isCurrentSession: false);
        }

        public async Task<Guid?> GetSessionIdByRefreshTokenAsync(string refreshTokenHash, CancellationToken cancellationToken = default)
        {
            var session = await _db.UserSessions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash && !x.IsRevoked);


            return session.Id;
        }

        public async Task<List<UserSessionDto>> GetUserSessionsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var currentId = _currentUser.GetUserId();

            if (!string.Equals(userId, currentId.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Cannot view sessions for another user");
            }

            var userSession = _db.UserSessions.AsNoTracking().Where(
                x => x.UserId == userId &&
                !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow).OrderByDescending(s => s.LastActivityAt);

            return userSession.Select(x => MapToDto(x, false)).ToList();
        }

        public async Task<List<UserSessionDto>> GetUserSessionsForAdminAsync(string userId, CancellationToken cancellationToken = default)
        {
            var userSession = await _db.UserSessions.AsNoTracking().
                Where(x => x.UserId == userId && !x.IsRevoked &&
                DateTime.UtcNow > x.LastActivityAt).OrderByDescending(x => x.LastActivityAt).ToListAsync();

            return userSession.Select(x => MapToDto(x, false)).ToList();
        }

        public async Task<int> RevokeAllSessionsAsync(string userId, string revokedBy, Guid? exceptSessionId = null, string? reason = null, CancellationToken cancellationToken = default)
        {

            var currentUserId = _currentUser.GetUserId().ToString();
            if (!string.Equals(userId, currentUserId, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Cannot revoke sessions for another user");
            }

            var query = _db.UserSessions
                .Where(s => s.UserId == userId && !s.IsRevoked);

            if (exceptSessionId.HasValue)
            {
                query = query.Where(s => s.Id != exceptSessionId.Value);
            }

            var sessions = await query.ToListAsync(cancellationToken);


            foreach (var session in sessions)
            {
                session.Revoke(revokedBy, reason ?? "User requested logout from all devices");
            }

            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Revoked {Count} sessions for user {UserId}", sessions.Count, userId);

            return sessions.Count;
        }

        public async Task<int> RevokeAllSessionsForAdminAsync(string userId, string revokedBy, string? reason = null, CancellationToken cancellationToken = default)
        {

            var sessions = await _db.UserSessions
                .Where(s => s.UserId == userId && !s.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var session in sessions)
            {
                session.Revoke(revokedBy, reason ?? "Admin requested");
            }

            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Admin {AdminId} revoked {Count} sessions for user {UserId}",
                revokedBy, sessions.Count, userId);

            return sessions.Count;
        }

        public async Task<bool> RevokeSessionAsync(
            Guid sessionId, string revokedBy,
            string? reason = null, CancellationToken cancellationToken = default)
        {
            var currentUserId = _currentUser.GetUserId();


            var session = await _db.UserSessions.
                FirstOrDefaultAsync(x => x.Id == sessionId && !x.IsRevoked, cancellationToken);

            if (!string.Equals(currentUserId.ToString(), session.UserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Cannot revoke session for another user");

            if (session == null)
                return false;

            session.Revoke(revokedBy, reason, reason);

            await _db.SaveChangesAsync();
            _logger.LogInformation("Session {SessionId} revoked by {RevokedBy}", sessionId, revokedBy);
            return true;


        }


        public async Task<bool> RevokeSessionForAdminAsync(Guid sessionId, string revokedBy, string? reason = null, CancellationToken cancellationToken = default)
        {
            var session = await _db.UserSessions.
                 FirstOrDefaultAsync(x => x.Id == sessionId && !x.IsRevoked, cancellationToken);

            if (session == null) return false;

            session.Revoke(revokedBy, reason);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Session {SessionId} revoked by {RevokedBy}", sessionId, revokedBy);

            return true;

        }

        public async Task UpdateSessionActivityAsync(string refreshTokenHash, CancellationToken cancellationToken = default)
        {

            var session = await _db.UserSessions
                .FirstOrDefaultAsync(s => s.RefreshTokenHash == refreshTokenHash && !s.IsRevoked, cancellationToken);

            if (session is not null)
            {
                session.UpdateActivity();
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateSessionRefreshTokenAsync(string oldRefreshTokenHash, string newRefreshTokenHash, DateTime newExpiresAt, CancellationToken cancellationToken = default)
        {
            var session = await _db.UserSessions
                .FirstOrDefaultAsync(s => s.RefreshTokenHash == oldRefreshTokenHash && !s.IsRevoked, cancellationToken);
            if (session is not null)
            {
                session.UpdateRefreshToken(newRefreshTokenHash, newExpiresAt);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ValidateSessionAsync(string refreshTokenHash, CancellationToken cancellationToken = default)
        {
            var sessionExists = _db.UserSessions
                .AsNoTracking()
                .Any(s => s.RefreshTokenHash == refreshTokenHash && !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
            if (!sessionExists)
            {
                _logger.LogWarning("Invalid session attempt with refresh token hash {RefreshTokenHash}", refreshTokenHash);
                return false;

            }

            return true;
        }

        private static UserSessionDto MapToDto(UserSession session, bool isCurrentSession)
        {
            return new UserSessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                UserName = session.User?.UserName,
                UserEmail = session.User?.Email,
                IpAddress = session.IpAddress,
                DeviceType = session.DeviceType,
                Browser = session.Browser,
                BrowserVersion = session.BrowserVersion,
                OperatingSystem = session.OperatingSystem,
                OsVersion = session.OsVersion,
                CreatedAt = session.CreatedAt,
                LastActivityAt = session.LastActivityAt,
                ExpiresAt = session.ExpiresAt,
                IsActive = !session.IsRevoked && session.ExpiresAt > DateTime.UtcNow,
                IsCurrentSession = isCurrentSession
            };
        }

    }
}