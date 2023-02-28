namespace MoneyExtension;
public static class Money
{
    public static int ToCents(this decimal amount)
    {
        if (amount <= 0) return 0;

        var value = amount
            .ToString("N2")
            .Replace(",", "")
            .Replace(".", "");

        if (string.IsNullOrEmpty(value)) return 0;

        int.TryParse(value, out var result);
        return result;
    }
}
