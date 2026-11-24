using Microsoft.Playwright;

namespace Maps.HolidayEntitlement.Tests.Pages;

public sealed class SmartAnswerQuestionPage : BasePage
{
    public SmartAnswerQuestionPage(IPage page) : base(page) { }

    public ILocator Heading => Page.Locator("h1").First;
    public ILocator ErrorSummary => Page.Locator(".govuk-error-summary");

    public async Task SelectRadioOptionAsync(string optionLabel)
    {
        var radio = Page.GetByRole(AriaRole.Radio, new()
        {
        NameRegex = new System.Text.RegularExpressions.Regex(
            System.Text.RegularExpressions.Regex.Escape(optionLabel.Trim()),
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        }).First;

        await radio.CheckAsync();
    }

    public async Task FillNumberInputAsync(string value)
    {
        var input = Page.Locator("input[type='text'], input[type='number']").First;
        await input.FillAsync(value);
    }

    public async Task FillDateAsync(string day, string month, string year)
    {
        await Page.GetByLabel("Day", new() { Exact = true }).FillAsync(day);
        await Page.GetByLabel("Month", new() { Exact = true }).FillAsync(month);
        await Page.GetByLabel("Year", new() { Exact = true }).FillAsync(year);
    }

    public async Task ClickContinueAsync()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
    }
}