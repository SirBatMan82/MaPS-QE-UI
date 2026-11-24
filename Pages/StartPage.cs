using Microsoft.Playwright;

namespace Maps.HolidayEntitlement.Tests.Pages;

public sealed class StartPage : BasePage
{
    public EntitlementPageStartLocators Locators { get; }

    public StartPage(IPage page) : base(page)
    {
        Locators = new EntitlementPageStartLocators(page);
    }

    public async Task OpenAsync(string url, bool dismissCookies = true)
    {
        await Page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
        if (dismissCookies)
        {
            await DismissCookieBannerIfPresentAsync();
        }
    }

    public async Task<SmartAnswerQuestionPage> ClickStartNowAsync()
    {
        await Locators.StartNowLink.ClickAsync();
        return new SmartAnswerQuestionPage(Page);
    }
}
