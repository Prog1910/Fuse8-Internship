namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class DomainExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection is null || collection?.Any() == false;

    public static string JointToString<T>(this IEnumerable<T> collection, string separator) => string.Join(separator, collection);

    public static int DaysCountBetween(this DateTimeOffset date1, DateTimeOffset date2) => date2.Date >= date1.Date ? (date2.Date - date1.Date).Days : (date1.Date - date2.Date).Days;
}