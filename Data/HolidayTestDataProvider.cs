using System.Text.Json;
using System.Text.Json.Serialization;
using Maps.HolidayEntitlement.Tests.Pages;
using NUnit.Framework;

namespace Maps.HolidayEntitlement.Tests.Data;

public sealed record DateParts(string Day, string Month, string Year);

public sealed record HolidayTestCase
{
    public required string TestName { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HolidayCalculationBasis? CalculationBasis { get; init; }

    public List<string> RadioSelections { get; init; } = [];
    public DateParts? StartDate { get; init; }
    public string? NumericInput { get; init; }
    public List<string> ExpectedPhrases { get; init; } = [];
}

public static class HolidayTestDataProvider
{
    public static IEnumerable<TestCaseData> LoadCases()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "HolidayTestCases.json");
        var json = File.ReadAllText(path);
        var cases = JsonSerializer.Deserialize<List<HolidayTestCase>>(json)
                    ?? throw new InvalidOperationException("Failed to deserialize HolidayTestCases.json");

        foreach (var item in cases)
        {
            yield return new TestCaseData(item).SetName($"HolidayCalc_{item.TestName}");
        }
    }
}