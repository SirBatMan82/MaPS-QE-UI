using Maps.HolidayEntitlement.Tests.Data;
using Maps.HolidayEntitlement.Tests.Pages;
using Microsoft.Playwright;
using NUnit.Framework;

namespace Maps.HolidayEntitlement.Tests.Tests;

public sealed class HolidayEntitlementTests : PlaywrightTestBase
{
    [Test]
    public async Task StartPage_DisplaysTitleAndStartsCalculatorFlow()
    {
        var startPage = new StartPage(Page);
        await startPage.OpenAsync(Settings.BaseUrl);

        await Expect(Page).ToHaveTitleAsync(new Regex("Calculate holiday entitlement"));

        var questionPage = await startPage.ClickStartNowAsync();
        await Expect(questionPage.Heading).ToBeVisibleAsync();
    }

    [Test]
    [TestCaseSource(typeof(HolidayTestDataProvider), nameof(HolidayTestDataProvider.LoadCases))]
    public async Task CalculatorFlow_CompletesJourneyAndDisplaysExpectedResult(HolidayTestCase testCase)
    {
        SmartAnswerQuestionPage questionPage;

        if (testCase.CalculationBasis.HasValue)
        {
            var regularPage = new EntitlementPageRegular(Page);
            await regularPage.OpenAsync();
            await Expect(regularPage.Locators.QuestionHeading).ToContainTextAsync("Is the holiday entitlement based on:");
            await regularPage.SelectBasisAsync(testCase.CalculationBasis.Value);
            questionPage = await regularPage.ClickContinueAsync();
        }
        else
        {
            var startPage = new StartPage(Page);
            await startPage.OpenAsync(Settings.BaseUrl);
            questionPage = await startPage.ClickStartNowAsync();
        }

        foreach (var option in testCase.RadioSelections)
        {
            await questionPage.SelectRadioOptionAsync(option);
            await questionPage.ClickContinueAsync();
        }

        if (testCase.StartDate is not null)
        {
            await questionPage.FillDateAsync(testCase.StartDate.Day, testCase.StartDate.Month, testCase.StartDate.Year);
            await questionPage.ClickContinueAsync();
        }

        if (!string.IsNullOrWhiteSpace(testCase.NumericInput))
        {
            await questionPage.FillNumberInputAsync(testCase.NumericInput);
            await questionPage.ClickContinueAsync();
        }

        var resultPage = new EntitlementResultPage(Page);
        foreach (var expected in testCase.ExpectedPhrases)
        {
            await Expect(resultPage.MainContent).ToContainTextAsync(expected);
        }
    }

    [Test]
    public async Task MissingSelection_ShowsAccessibleValidationErrorSummary()
    {
        var startPage = new StartPage(Page);
        await startPage.OpenAsync(Settings.BaseUrl);
        var questionPage = await startPage.ClickStartNowAsync();

        await questionPage.ClickContinueAsync();

        await Expect(questionPage.ErrorSummary).ToBeVisibleAsync();
    }
}