using System;
using System.Collections.Generic;
using System.Text;

namespace Module.Identity.Contract.Dtos
{
    public class PasswordExpiryStatusDto
    {
        public bool IsExpired { get; set; }
        public bool IsExpiringWithinWarningPeriod { get; set; }
        public int DaysUntilExpiry { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string Status => this switch
        {
            { IsExpired: true } => "Expired",
            { IsExpiringWithinWarningPeriod: true } => "Expiring Soon",
            _ => "Valid"
        };
    }
}
