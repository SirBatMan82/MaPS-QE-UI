using Maps.HolidayEntitlement.Tests.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Interfaces;

namespace Maps.HolidayEntitlement.Tests.Tests;

[Parallelizable(ParallelScope.Self)]
public abstract class PlaywrightTestBase : PageTest
{
    protected TestSettings Settings { get; private set; } = null!;

    [SetUp]
    public async Task SetUpBase()
    {
        Settings = new TestSettings();
        Page.SetDefaultTimeout(Settings.DefaultTimeoutMs);

        await Context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true, Sources = true });
    }

    [TearDown]
    public async Task TearDownBase()
    {
        var failed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;

        if (failed)
        {
            var safeName = string.Concat(TestContext.CurrentContext.Test.Name.Split(Path.GetInvalidFileNameChars()));
            var artifactDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "artifacts");

            Directory.CreateDirectory(artifactDir);

            await Page.ScreenshotAsync(new() 
            { 
                Path = Path.Combine(artifactDir, $"{safeName}.png"),
                FullPage = true
            });

            await Context.Tracing.StopAsync(new TracingStopOptions
            { 
                Path = Path.Combine(artifactDir, $"{safeName}-trace.zip") 
            });

        }
        else
        {
            await Context.Tracing.StopAsync();
        }
    }
}