using Microsoft.Playwright;

namespace Maps.HolidayEntitlement.Tests.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
    }

    public async Task DismissCookieBannerIfPresentAsync()
    {
        var rejectCookieButton = Page.GetByRole(AriaRole.Button, new() { Name = "Reject additional cookies" });
        if (await rejectCookieButton.IsVisibleAsync())
        {
            await rejectCookieButton.ClickAsync();
        }
        else
        {
            var acceptCookieButton = Page.GetByRole(AriaRole.Button, new() { Name = "Accept additional cookies" });
            if (await acceptCookieButton.IsVisibleAsync())
            {
                await acceptCookieButton.ClickAsync();
            }
        }

        var hideCookieMessageButton = Page.GetByRole(AriaRole.Button, new() { Name = "Hide cookie message" });
        if (await hideCookieMessageButton.IsVisibleAsync())
        {
            await hideCookieMessageButton.ClickAsync();
        }
    }
}