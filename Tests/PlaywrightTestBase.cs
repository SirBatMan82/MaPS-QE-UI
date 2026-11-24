using Maps.HolidayEntitlement.Tests.Configuration;
using Maps.HolidayEntitlement.Tests.Reporting;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Interfaces;

namespace Maps.HolidayEntitlement.Tests.Tests;

[Parallelizable(ParallelScope.Self)]
public abstract class PlaywrightTestBase : PageTest
{
    protected TestSettings Settings { get; private set; } = null!;
    private readonly System.Diagnostics.Stopwatch _stopwatch = new();

    [SetUp]
    public async Task SetUpBase()
    {
        Settings = new TestSettings();
        Page.SetDefaultTimeout(Settings.DefaultTimeoutMs);

        await Context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true, Sources = true });
        _stopwatch.Restart();
    }

    [TearDown]
    public async Task TearDownBase()
    {
        var failed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;
        string? savedScreenshot = null;
        string? savedTrace = null;

        if (failed)
        {
            var safeName = string.Concat(TestContext.CurrentContext.Test.Name.Split(Path.GetInvalidFileNameChars()));
            var artifactDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "artifacts");

            Directory.CreateDirectory(artifactDir);

            savedScreenshot = Path.Combine(artifactDir, $"{safeName}.png");
            savedTrace = Path.Combine(artifactDir, $"{safeName}-trace.zip");

            await Page.ScreenshotAsync(new()
            {
                Path = savedScreenshot,
                FullPage = true
            });

            await Context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = savedTrace
            });
        }
        else
        {
            await Context.Tracing.StopAsync();
        }

        _stopwatch.Stop();

        var category = TestContext.CurrentContext.Test.Properties["Category"].FirstOrDefault()?.ToString() ?? "Functional";

        HtmlTestReport.Record(new TestExecutionRecord(
            TestName: TestContext.CurrentContext.Test.Name,
            ClassName: TestContext.CurrentContext.Test.ClassName ?? string.Empty,
            Category: category,
            Status: TestContext.CurrentContext.Result.Outcome.Status.ToString(),
            DurationMs: _stopwatch.Elapsed.TotalMilliseconds,
            ErrorMessage: TestContext.CurrentContext.Result.Message,
            ScreenshotPath: savedScreenshot,
            TracePath: savedTrace
        ));
    }
}