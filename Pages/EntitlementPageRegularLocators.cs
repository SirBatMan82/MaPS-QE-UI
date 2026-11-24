using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace Maps.HolidayEntitlement.Tests.Pages;

public sealed class EntitlementPageRegularLocators
{
    private readonly IPage _page;

    public EntitlementPageRegularLocators(IPage page)
    {
        _page = page;
    }

    // Question Heading & Radio Options
    public ILocator QuestionHeading => _page.Locator("main#content h1");
    public ILocator DaysWorkedPerWeekRadio => _page.GetByLabel("days worked per week", new() { Exact = false });
    public ILocator HoursWorkedPerWeekRadio => _page.GetByLabel("hours worked per week", new() { Exact = false });
    public ILocator AnnualisedHoursRadio => _page.GetByLabel("annualised hours", new() { Exact = false });
    public ILocator CompressedHoursRadio => _page.GetByLabel("compressed hours", new() { Exact = false });
    public ILocator ShiftsRadio => _page.GetByLabel("shifts", new() { Exact = false });

    // Primary Action & Post-Submit Validation Summary
    public ILocator ContinueButton => _page.GetByRole(AriaRole.Button, new() { Name = "Continue" });
    public ILocator ErrorSummary => _page.Locator(".govuk-error-summary");

    // "Your answers" Summary Section
    public ILocator StartAgainLink => _page.GetByRole(AriaRole.Link, new() { Name = "Start again" });
    public ILocator ChangeIrregularHoursLink => _page.GetByRole(AriaRole.Link, new(){NameRegex = new Regex("Change.*irregular hours or for part of the year", RegexOptions.IgnoreCase) });

    // Feedback & Footer Controls
    public ILocator FeedbackYesButton => _page.GetByRole(AriaRole.Button, new() { Name = "Yes this page is useful" });
    public ILocator FeedbackNoButton => _page.GetByRole(AriaRole.Button, new() { Name = "No this page is not useful" });
    public ILocator ReportProblemButton => _page.GetByRole(AriaRole.Button, new() { Name = "Report a problem with this page" });
    public ILocator AccessibilityStatementLink => _page.GetByRole(AriaRole.Link, new() { Name = "Accessibility statement" });
    public ILocator CookiesFooterLink => _page.GetByRole(AriaRole.Link, new() { Name = "Cookies" });

    public IReadOnlyDictionary<string, (ILocator Locator, bool ExpectVisibleOnLoad)> AllMappedElements() =>
        new Dictionary<string, (ILocator, bool)>
        {
            [nameof(QuestionHeading)] = (QuestionHeading, true),
            [nameof(DaysWorkedPerWeekRadio)] = (DaysWorkedPerWeekRadio, true),
            [nameof(HoursWorkedPerWeekRadio)] = (HoursWorkedPerWeekRadio, true),
            [nameof(AnnualisedHoursRadio)] = (AnnualisedHoursRadio, true),
            [nameof(CompressedHoursRadio)] = (CompressedHoursRadio, true),
            [nameof(ShiftsRadio)] = (ShiftsRadio, true),
            [nameof(ContinueButton)] = (ContinueButton, true),
            [nameof(StartAgainLink)] = (StartAgainLink, true),
            [nameof(ChangeIrregularHoursLink)] = (ChangeIrregularHoursLink, true),
            [nameof(FeedbackYesButton)] = (FeedbackYesButton, true),
            [nameof(FeedbackNoButton)] = (FeedbackNoButton, true),
            [nameof(ReportProblemButton)] = (ReportProblemButton, true),
            [nameof(AccessibilityStatementLink)] = (AccessibilityStatementLink, true),
            [nameof(CookiesFooterLink)] = (CookiesFooterLink, true),
        };
}