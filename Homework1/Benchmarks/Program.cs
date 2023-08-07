using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

// BenchmarkRunner.Run<StringInternBenchmark>();
// BenchmarkRunner.Run<AccountProcessorBenchmark>();

/* Comments
 * Из полученных результатов, можно видеть, что метод WordIsExistsIntern работает на ~20% быстрее метода WordIsExists.
 * Следует учесть, что при использовании метода WordIsExistsIntern строки будут храниться в пуле интернирования до завершения приложения.
 * Пусть есть баг, при котором слово не будет интернировано в заданном словаре. Тогда метод WordIsExistsIntern не посчитает равные слова за одинаковые.
 * Следовательно, использование метода WordIsExistsIntern должно быть оправданным.
 *
|             Method |            word |        Mean |     Error |    StdDev |      Median | Ratio | RatioSD | Rank |   Gen0 | Allocated | Alloc Ratio |
|------------------- |---------------- |------------:|----------:|----------:|------------:|------:|--------:|-----:|-------:|----------:|------------:|
| WordIsExistsIntern |       дискретка | 1,551.09 us | 30.366 us | 56.285 us | 1,561.31 us |  0.83 |    0.04 |    1 |      - |     129 B |        1.00 |
|       WordIsExists |       дискретка | 1,879.40 us | 36.917 us | 45.337 us | 1,896.15 us |  1.00 |    0.00 |    2 |      - |     129 B |        1.00 |
|                    |                 |             |           |           |             |       |         |      |        |           |             |
| WordIsExistsIntern |      мантисса/I | 1,185.17 us | 22.771 us | 21.300 us | 1,194.11 us |  0.84 |    0.01 |    1 |      - |     129 B |        1.00 |
|       WordIsExists |      мантисса/I | 1,414.65 us | 20.708 us | 17.292 us | 1,420.56 us |  1.00 |    0.00 |    2 |      - |     129 B |        1.00 |
|                    |                 |             |           |           |             |       |         |      |        |           |             |
| WordIsExistsIntern | Остроградский/G |    13.31 us |  0.255 us |  0.273 us |    13.11 us |  0.86 |    0.03 |    1 | 0.0153 |     128 B |        1.00 |
|       WordIsExists | Остроградский/G |    15.44 us |  0.303 us |  0.324 us |    15.68 us |  1.00 |    0.00 |    2 |      - |     128 B |        1.00 |
|                    |                 |             |           |           |             |       |         |      |        |           |             |
| WordIsExistsIntern |    приращение/K |   680.50 us | 13.473 us | 19.749 us |   678.40 us |  0.80 |    0.03 |    1 |      - |     128 B |        1.00 |
|       WordIsExists |    приращение/K |   856.16 us | 17.042 us | 24.441 us |   858.58 us |  1.00 |    0.00 |    2 |      - |     128 B |        1.00 |
|                    |                 |             |           |           |             |       |         |      |        |           |             |
| WordIsExistsIntern |        Хевисайд | 1,563.43 us | 30.934 us | 45.342 us | 1,557.98 us |  0.83 |    0.03 |    1 |      - |     129 B |        1.00 |
|       WordIsExists |        Хевисайд | 1,898.25 us | 28.828 us | 26.966 us | 1,898.71 us |  1.00 |    0.00 |    2 |      - |     129 B |        1.00 |
*/

#region StringInternBenchmark
[MemoryDiagnoser(displayGenColumns: true)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StringInternBenchmark
{
    private readonly List<string> _words = new();

    public StringInternBenchmark()
    {
        foreach (var word in File.ReadLines(@".\SpellingDictionaries\ru_RU.dic"))
            _words.Add(string.Intern(word));
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(SampleData))]
    public bool WordIsExists(string word)
        => _words.Any(item => word.Equals(item, StringComparison.Ordinal));

    [Benchmark]
    [ArgumentsSource(nameof(SampleData))]
    public bool WordIsExistsIntern(string word)
    {
        var internedWord = string.Intern(word);
        return _words.Any(item => ReferenceEquals(internedWord, item));
    }

    public IEnumerable<string> SampleData()
    {
        yield return new StringBuilder().Append("Остроград").Append("ский/G").ToString();
        yield return new StringBuilder().Append("прира").Append("щение/K").ToString();
        yield return new StringBuilder().Append("мантис").Append("са/I").ToString();
        yield return "дискретка";
        yield return "Хевисайд";
    }
}
#endregion

#region AccountProcessorBenchmark
[MemoryDiagnoser(displayGenColumns: true)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class AccountProcessorBenchmark
{
    private Fuse8_ByteMinds.SummerSchool.Domain.AccountProcessor accountProcessor = new();
    private Fuse8_ByteMinds.SummerSchool.Domain.BankAccount bankAccount = new();

    [Benchmark(Baseline = true)]
    public void Calculate()
    {
        accountProcessor.Calculate(bankAccount);
    }

    [Benchmark]
    public void CalculatePerformed()
    {
        accountProcessor.CalculatePerformed(in bankAccount);
    }
}
#endregion