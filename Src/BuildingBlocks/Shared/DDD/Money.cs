namespace Shared.DDD;

using System;

public sealed record Money : IComparable<Money>
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currency);

        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
    }


    private Money()
    {
        Currency = "USD";
    }

    public static Money Zero(string currency = "USD") => new(0m, currency);
    public static Money Create(decimal amount, string currency) => new(amount, currency);

    public bool IsZero => Amount == 0m;
    public bool IsPositive => Amount > 0m;
    public bool IsNegative => Amount < 0m;

    public Money Add(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        EnsureSameCurrency(other);
        return this with { Amount = Amount + other.Amount };
    }

    public Money Subtract(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        EnsureSameCurrency(other);
        return this with { Amount = Amount - other.Amount };
    }

    public Money Multiply(decimal factor) =>
        this with { Amount = Amount * factor };

    public Money Divide(decimal divisor)
    {
        if (divisor == 0m)
            throw new DivideByZeroException("Cannot divide Money by zero.");

        return this with { Amount = Amount / divisor };
    }

    public Money ApplyDiscount(decimal percentage)
    {
        if (percentage < 0m || percentage > 100m)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Discount percentage must be between 0 and 100.");

        var discountAmount = Amount * (percentage / 100m);
        return this with { Amount = Amount - discountAmount };
    }

    public Money Round(int decimals = 2, MidpointRounding mode = MidpointRounding.AwayFromZero) =>
        this with { Amount = Math.Round(Amount, decimals, mode) };

    public Money Absolute() =>
        this with { Amount = Math.Abs(Amount) };

    // Operator Overloading
    public static Money operator +(Money left, Money right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Add(right);
    }

    public static Money operator -(Money left, Money right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Subtract(right);
    }

    public static Money operator *(Money left, decimal right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Multiply(right);
    }

    public static Money operator *(decimal left, Money right) => right * left;

    public static Money operator /(Money left, decimal right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Divide(right);
    }

    // Comparisons (Null-Safe)
    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        EnsureSameCurrency(other);
        return Amount.CompareTo(other.Amount);
    }

    public static bool operator <(Money? left, Money? right) =>
        left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(Money? left, Money? right) =>
        left is null || left.CompareTo(right) <= 0;

    public static bool operator >(Money? left, Money? right) =>
        left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(Money? left, Money? right) =>
        left is null ? right is null : left.CompareTo(right) >= 0;

    private void EnsureSameCurrency(Money other)
    {
        if (!string.Equals(Currency, other.Currency, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Cannot perform operation on Money with different currencies: '{Currency}' and '{other.Currency}'.");
        }
    }

    public override string ToString() => $"{Amount:N2} {Currency}";
}