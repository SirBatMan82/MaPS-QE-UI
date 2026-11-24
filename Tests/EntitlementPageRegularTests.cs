using System.Text;
using Maps.HolidayEntitlement.Tests.Pages;
using Microsoft.Playwright;
using NUnit.Framework;

namespace Maps.HolidayEntitlement.Tests.Tests;

public sealed class EntitlementPageRegularTests : PlaywrightTestBase
{
    [Test]
    [Order(0)]
    [Category("PreFlight")]
    public async Task PreFlight_AllMappedRegularPageElementsExist()
    {
        var pageObject = new EntitlementPageRegular(Page);
        await pageObject.OpenAsync();

        var failures = new StringBuilder();

        foreach (var (name, (locator, expectVisibleOnLoad)) in pageObject.Locators.AllMappedElements())
        {
            var count = await locator.CountAsync();
            if (count == 0)
            {
                failures.AppendLine($"- Missing locator '{name}' (count = 0).");
                continue;
            }

            if (expectVisibleOnLoad && !await locator.First.IsVisibleAsync())
            {
                failures.AppendLine($"- Locator '{name}' exists in DOM but is not visible on load.");
            }
        }

        Assert.That(failures.Length, Is.EqualTo(0), $"Regular page pre-flight failed:\n{failures}");
    }

    [Test]
    public async Task RegularPage_SelectsDaysWorkedPerWeekAndContinues()
    {
        var pageObject = new EntitlementPageRegular(Page);
        await pageObject.OpenAsync();

        await Expect(pageObject.Locators.QuestionHeading).ToContainTextAsync("Is the holiday entitlement based on:");
        await pageObject.SelectBasisAsync(HolidayCalculationBasis.DaysWorkedPerWeek);

        var nextQuestion = await pageObject.ClickContinueAsync();
        await Expect(nextQuestion.Heading).ToBeVisibleAsync();
    }
}