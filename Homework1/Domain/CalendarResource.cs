namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Значения ресурсов для календаря
/// </summary>
public class CalendarResource
{
    public static readonly CalendarResource Instance = new();

    private static readonly string[] MonthNames;

    public static readonly string January;

    public static readonly string February;

    static CalendarResource()
    {
        MonthNames = InitMonthNames();
        January = GetMonthByNumber(0);
        February = GetMonthByNumber(1);
    }

    private static string[] InitMonthNames() => new[] {
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"
        };

    private static string GetMonthByNumber(int number) => MonthNames[number];

    public string this[Month month] => Enum.IsDefined(typeof(Month), month) && (int)month >= 0 && (int)month < MonthNames.Length ? MonthNames[(int)month] : throw new ArgumentOutOfRangeException("Нельзя получить название месяца для несуществующего Month.");

}

public enum Month
{
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December,
}