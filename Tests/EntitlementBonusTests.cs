using System.Text.RegularExpressions;
using Maps.HolidayEntitlement.Tests.Pages;
using Microsoft.Playwright;
using NUnit.Framework;

namespace Maps.HolidayEntitlement.Tests.Tests;

[Category("Bonus")]
public sealed class EntitlementBonusTests : PlaywrightTestBase
{
    [Test]
    public async Task RegularPage_MissingBasisSelection_DisplaysAccessibleErrorSummary()
    {
        var regularPage = new EntitlementPageRegular(Page);
        await regularPage.OpenAsync();

        await regularPage.ClickContinueAsync();

        await Expect(regularPage.Locators.ErrorSummary).ToBeVisibleAsync();
        await Expect(regularPage.Locators.ErrorSummary).ToContainTextAsync("There is a problem");
    }

    [Test]
    public async Task RegularPage_ChangeIrregularHoursLink_NavigatesBackToIrregularHoursQuestion()
    {
        var regularPage = new EntitlementPageRegular(Page);
        await regularPage.OpenAsync();

        await regularPage.ClickChangeIrregularHoursAsync();

        var questionPage = new SmartAnswerQuestionPage(Page);
        await Expect(questionPage.Heading).ToContainTextAsync(
            new Regex("irregular hours or for part of the year", RegexOptions.IgnoreCase));
    }

    [Test]
    public async Task RegularPage_StartAgainLink_ResetsJourneyToStartOrFirstQuestion()
    {
        var regularPage = new EntitlementPageRegular(Page);
        await regularPage.OpenAsync();

        await regularPage.ClickStartAgainAsync();

        await Expect(Page).ToHaveURLAsync(new Regex("/calculate-your-holiday-entitlement"));
    }

    [Test]
    [TestCase(HolidayCalculationBasis.DaysWorkedPerWeek, "Full leave year", "5", null, "28 days")]
    [TestCase(HolidayCalculationBasis.HoursWorkedPerWeek, "Full leave year", "37.5", "5", "210 hours")]
    public async Task RegularPage_FullLeaveYear_CalculatesStatutoryEntitlementAcrossBases(
        HolidayCalculationBasis basis,
        string leaveYearOption,
        string firstNumericAmount,
        string? daysPerWeek,
        string expectedEntitlementText)

    {
        var regularPage = new EntitlementPageRegular(Page);
        await regularPage.OpenAsync();

        await regularPage.SelectBasisAsync(basis);
        var questionPage = await regularPage.ClickContinueAsync();

        await questionPage.SelectRadioOptionAsync(leaveYearOption);
        await questionPage.ClickContinueAsync();

        await questionPage.FillNumberInputAsync(firstNumericAmount);
        await questionPage.ClickContinueAsync();

        if (!string.IsNullOrWhiteSpace(daysPerWeek))
        {
            await questionPage.FillNumberInputAsync(daysPerWeek);
            await questionPage.ClickContinueAsync();
        }

        var resultPage = new EntitlementResultPage(Page);
        await Expect(resultPage.MainContent).ToContainTextAsync(expectedEntitlementText);
}

    [Test]
    public async Task StartPage_AccessibilityLandmarksAndMainHeading_ArePresent()
    {
        var startPage = new StartPage(Page);
        await startPage.OpenAsync(Settings.BaseUrl, dismissCookies: false);

        await Expect(Page.Locator("main#content")).ToBeVisibleAsync();
        await Expect(startPage.Locators.MainHeading).ToBeVisibleAsync();
        await Expect(startPage.Locators.SkipToMainContent).ToHaveAttributeAsync("href", "#content");
    }
}