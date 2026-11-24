using Microsoft.Playwright;

namespace Maps.HolidayEntitlement.Tests.Pages;

public sealed class EntitlementResultPage : BasePage
{
    public EntitlementResultPage(IPage page) : base(page) { }

    public ILocator MainContent => Page.Locator("main#content");

    public async Task ClickChangeAsync(string rowLabel)
    {
        var row = Page.Locator(".govuk-summary-list__row").Filter(new() { HasText = rowLabel });
        await row.GetByRole(AriaRole.Link, new() { Name = "Change" }).ClickAsync();
    }
}
