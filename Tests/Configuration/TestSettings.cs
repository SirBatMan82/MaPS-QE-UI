namespace Maps.HolidayEntitlement.Tests.Configuration;


public sealed class TestSettings
{
    public string BaseUrl { get; init; } =
        Environment.GetEnvironmentVariable("Holiday_calc_url") ?? "https://www.gov.uk/calculate-your-holiday-entitlement";

    public string Browser { get; init; } =
        Environment.GetEnvironmentVariable("BROWSER") ?? "chromium";

    public bool Headless { get; init; } =
        Environment.GetEnvironmentVariable("HEADLESS")?.ToLower() == "true";

    public float DefaultTimeoutMs { get; init; } = 15_000;
    
}