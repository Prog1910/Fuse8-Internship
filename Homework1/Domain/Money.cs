using Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Модель для хранения денег
/// </summary>
public class Money
{
    public const int MinMoney = 0;
    public const int MaxKopeks = 100;

    public Money(int rubles, int kopeks)
        : this(false, rubles, kopeks)
    {
    }

    public Money(bool isNegative, int rubles, int kopeks)
    {
        IsNegative = isNegative;
        if (isNegative == true && rubles == MinMoney && kopeks == MinMoney) throw new ArgumentException("Минус 0 рублей и 0 копеек быть не может");
        Rubles = (rubles >= MinMoney) ? rubles : throw new ArgumentException("Число рублей не может быть отрицательным");
        Kopeks = (kopeks >= MinMoney && kopeks < MaxKopeks) ? kopeks : throw new ArgumentException("Количество копеек должно быть больше 0 и меньше 99");
    }

    /// <summary>
    /// Отрицательное значение
    /// </summary>
    public bool IsNegative { get; }

    /// <summary>
    /// Число рублей
    /// </summary>
    public int Rubles { get; }

    /// <summary>
    /// Количество копеек
    /// </summary>
    public int Kopeks { get; }

    public static Money operator +(Money money1, Money money2)
    {
        bool isNegative = false;
        int total1 = money1.TotalAmount();
        int total2 = money2.TotalAmount();
        int total = total1 + total2;
        if (total < MinMoney)
        {
            total = -total;
            isNegative = true;
        }
        return new Money(isNegative, rubles: total / MaxKopeks, kopeks: total % MaxKopeks);
    }

    public static Money operator -(Money money1, Money money2)
    {
        bool isNegative = false;
        int total1 = money1.TotalAmount();
        int total2 = money2.TotalAmount();
        int total = total1 - total2;
        if (total < MinMoney)
        {
            total = -total;
            isNegative = true;
        }
        return new Money(isNegative, rubles: total / MaxKopeks, kopeks: total % MaxKopeks);
    }

    public static bool operator >(Money money1, Money money2)
    {
        int total1 = money1.TotalAmount();
        int total2 = money2.TotalAmount();
        return total1 > total2;
    }

    public static bool operator >=(Money money1, Money money2)
    {
        if (money1.Equals(money2)) return true;
        return money1 > money2;
    }

    public static bool operator <(Money money1, Money money2)
    {
        int total1 = money1.TotalAmount();
        int total2 = money2.TotalAmount();
        return total1 < total2;
    }

    public static bool operator <=(Money money1, Money money2)
    {
        if (money1.Equals(money2)) return true;
        return money1 < money2;
    }

    public override bool Equals(object? obj)
    {
        var money = obj as Money;
        if (money == null) return false;
        return TotalAmount() == money.TotalAmount();
    }

    public override int GetHashCode()
    {
        return TotalAmount();
    }

    public override string ToString()
    {
        return TotalAmount().ToString();
    }

    private int TotalAmount()
    {
        int total = checked(Rubles * MaxKopeks + Kopeks);
        return (IsNegative == false) ? total : -total;
    }
}