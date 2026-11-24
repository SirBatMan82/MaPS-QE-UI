using Maps.HolidayEntitlement.Tests.Reporting;
using NUnit.Framework;

namespace Maps.HolidayEntitlement.Tests.Tests;

[SetUpFixture]
public class AssemblyHooks
{
    [OneTimeTearDown]
    public void GenerateHtmlReport()
    {
        var artifactsDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "artifacts");
        var reportFile = HtmlTestReport.WriteReport(artifactsDir);
        TestContext.Progress.WriteLine($"HTML Test Report written to: {reportFile}");
    }
}