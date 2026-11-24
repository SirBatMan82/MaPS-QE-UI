using Microsoft.Playwright;

namespace Maps.HolidayEntitlement.Tests.Pages;

public enum HolidayCalculationBasis
{
    DaysWorkedPerWeek,
    HoursWorkedPerWeek,
    AnnualisedHours,
    CompressedHours,
    Shifts
}

public sealed class EntitlementPageRegular : BasePage
{
    public EntitlementPageRegularLocators Locators { get; }

    public EntitlementPageRegular(IPage page) : base(page)
    {
        Locators = new EntitlementPageRegularLocators(page);
    }

    public async Task OpenAsync(string url = "https://www.gov.uk/calculate-your-holiday-entitlement/y/regular", bool dismissCookies = true)
    {
        await Page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
        if (dismissCookies)
        {
            await DismissCookieBannerIfPresentAsync();
        }
    }

    public async Task SelectBasisAsync(HolidayCalculationBasis basis)
    {
        var radio = basis switch
        {
            HolidayCalculationBasis.DaysWorkedPerWeek => Locators.DaysWorkedPerWeekRadio,
            HolidayCalculationBasis.HoursWorkedPerWeek => Locators.HoursWorkedPerWeekRadio,
            HolidayCalculationBasis.AnnualisedHours => Locators.AnnualisedHoursRadio,
            HolidayCalculationBasis.CompressedHours => Locators.CompressedHoursRadio,
            HolidayCalculationBasis.Shifts => Locators.ShiftsRadio,
            _ => throw new ArgumentOutOfRangeException(nameof(basis), basis, null)
        };

        await radio.CheckAsync();
    }

    public async Task<SmartAnswerQuestionPage> ClickContinueAsync()
    {
        await Locators.ContinueButton.ClickAsync();
        return new SmartAnswerQuestionPage(Page);
    }

    public async Task ClickStartAgainAsync()
    {
        await Locators.StartAgainLink.ClickAsync();
    }

    public async Task ClickChangeIrregularHoursAsync()
    {
        await Locators.ChangeIrregularHoursLink.ClickAsync();
    }
}
