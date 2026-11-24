using Microsoft.Playwright;

namespace Maps.HolidayEntitlement.Tests.Pages;

public sealed class EntitlementPageStartLocators
{
    private readonly IPage _page;

    public EntitlementPageStartLocators(IPage page)
    {
        _page = page;
    }

    public ILocator SkipToMainContent => _page.GetByRole(AriaRole.Link, new() { Name = "Skip to main content" });
    public ILocator MainHeading => _page.Locator("main#content h1");
    public ILocator StartNowLink => _page.Locator("a.govuk-button--start, a, button").Filter(new() { HasTextRegex = new Regex("Start now", RegexOptions.IgnoreCase) }).First;
    public ILocator AcceptCookiesButton => _page.GetByRole(AriaRole.Button, new() { Name = "Accept additional cookies" });
    public ILocator RejectCookiesButton => _page.GetByRole(AriaRole.Button, new() { Name = "Reject additional cookies" });
    public ILocator ViewCookiesLink => _page.GetByRole(AriaRole.Link, new() { Name = "View cookies" });
    public ILocator HideCookieMessageButton => _page.GetByRole(AriaRole.Button, new() { Name = "Hide cookie message" });
    public ILocator BreadcrumbHomeLink => _page.Locator(".govuk-breadcrumbs").GetByRole(AriaRole.Link, new() { Name = "Home" });
    public ILocator RelatedAgriculturalLink => _page.GetByRole(AriaRole.Link, new() { Name = "Calculate your agricultural worker holiday entitlement" });
    public ILocator FeedbackYesButton => _page.GetByRole(AriaRole.Button, new() { Name = "Yes this page is useful" });
    public ILocator FeedbackNoButton => _page.GetByRole(AriaRole.Button, new() { Name = "No this page is not useful" });
    public ILocator ReportProblemLink => _page.GetByRole(AriaRole.Link, new() { Name = "Report a problem with this page" });
    public ILocator AccessibilityStatementLink => _page.GetByRole(AriaRole.Link, new() { Name = "Accessibility statement" });
    public ILocator CookiesFooterLink => _page.GetByRole(AriaRole.Link, new() { Name = "Cookies" });
    public ILocator OpenGovernmentLicenceLink => _page.GetByRole(AriaRole.Link, new() { Name = "Open Government Licence v3.0" });

    public IReadOnlyDictionary<string, (ILocator Locator, bool ExpectVisibleOnLoad)> AllMappedElements() =>
        new Dictionary<string, (ILocator, bool)>
        {
            [nameof(SkipToMainContent)] = (SkipToMainContent, false),
            [nameof(MainHeading)] = (MainHeading, true),
            [nameof(StartNowLink)] = (StartNowLink, true),
            [nameof(AcceptCookiesButton)] = (AcceptCookiesButton, true),
            [nameof(RejectCookiesButton)] = (RejectCookiesButton, true),
            [nameof(ViewCookiesLink)] = (ViewCookiesLink, true),
            [nameof(HideCookieMessageButton)] = (HideCookieMessageButton, false),
            [nameof(BreadcrumbHomeLink)] = (BreadcrumbHomeLink, true),
            [nameof(RelatedAgriculturalLink)] = (RelatedAgriculturalLink, true),
            [nameof(FeedbackYesButton)] = (FeedbackYesButton, true),
            [nameof(FeedbackNoButton)] = (FeedbackNoButton, true),
            [nameof(ReportProblemLink)] = (ReportProblemLink, true),
            [nameof(AccessibilityStatementLink)] = (AccessibilityStatementLink, true),
            [nameof(CookiesFooterLink)] = (CookiesFooterLink, true),
            [nameof(OpenGovernmentLicenceLink)] = (OpenGovernmentLicenceLink, true),
        };
}