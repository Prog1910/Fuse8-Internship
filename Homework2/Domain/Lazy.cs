namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Контейнер для значения, с отложенным получением
/// </summary>
public class Lazy<TValue>
{
    private Func<TValue>? _initValue;
    private TValue? _value;

    public Lazy(Func<TValue> initValue) => _initValue = initValue;

    public TValue? Value
    {
        get
        {
            if (_initValue != null)
            {
                _value = _initValue();
                _initValue = null;
            }
            return _value;
        }
    }
}