using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.Eventing;


public class OutboxMessage
{
    public Guid Id { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public string Type { get; set; } = default!;

    public string Payload { get; set; } = default!;



    public DateTime? ProcessedOnUtc { get; set; }

    public int RetryCount { get; set; }

    public string? LastError { get; set; }

    public bool IsDead { get; set; }
}

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    private readonly string _schema;

    public OutboxMessageConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("OutboxMessages", _schema);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Type)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(o => o.Payload)
            .IsRequired();



        builder.Property(o => o.CreatedOnUtc)
            .IsRequired();
    }
}
