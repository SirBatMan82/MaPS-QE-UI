using System.Collections.Concurrent;
using System.Net;
using System.Text;

namespace Maps.HolidayEntitlement.Tests.Reporting;

public sealed record TestExecutionRecord(
    string TestName,
    string ClassName,
    string Category,
    string Status,
    double DurationMs,
    string? ErrorMessage,
    string? ScreenshotPath,
    string? TracePath);

public static class HtmlTestReport
{
    private static readonly ConcurrentBag<TestExecutionRecord> Records = new();
    private static readonly DateTime RunStartedUtc = DateTime.UtcNow;

    public static void Record(TestExecutionRecord record) => Records.Add(record);

    public static string WriteReport(string outputDirectory)
    {
        Directory.CreateDirectory(outputDirectory);
        var reportPath = Path.Combine(outputDirectory, "TestReport.html");

        var ordered = Records
            .OrderBy(r => r.ClassName)
            .ThenBy(r => r.TestName)
            .ToList();

        int passed = ordered.Count(r => r.Status == "Passed");
        int failed = ordered.Count(r => r.Status == "Failed");
        int skipped = ordered.Count(r => r.Status == "Skipped");
        double totalSeconds = (DateTime.UtcNow - RunStartedUtc).TotalSeconds;

        var rows = new StringBuilder();
        foreach (var r in ordered)
        {
            string badgeClass = r.Status switch
            {
                "Passed" => "badge-pass",
                "Failed" => "badge-fail",
                _ => "badge-skip"
            };

            string errorHtml = string.IsNullOrWhiteSpace(r.ErrorMessage)
                ? "<span class='muted'>None</span>"
                : $"<pre class='error-box'>{WebUtility.HtmlEncode(r.ErrorMessage)}</pre>";

            var links = new List<string>();
            if (!string.IsNullOrWhiteSpace(r.ScreenshotPath) && File.Exists(r.ScreenshotPath))
                links.Add($"<a href='{Path.GetFileName(r.ScreenshotPath)}' target='_blank'>Screenshot</a>");
            if (!string.IsNullOrWhiteSpace(r.TracePath) && File.Exists(r.TracePath))
                links.Add($"<a href='{Path.GetFileName(r.TracePath)}' download>Trace ZIP</a>");

            string artifactHtml = links.Count > 0
                ? string.Join(" | ", links)
                : "<span class='muted'>—</span>";

            rows.AppendLine($"""
                <tr>
                    <td><span class="badge {badgeClass}">{WebUtility.HtmlEncode(r.Status)}</span></td>
                    <td><strong>{WebUtility.HtmlEncode(r.TestName)}</strong><br><span class="muted">{WebUtility.HtmlEncode(r.ClassName)}</span></td>
                    <td>{WebUtility.HtmlEncode(r.Category)}</td>
                    <td>{r.DurationMs:F0} ms</td>
                    <td>{errorHtml}</td>
                    <td>{artifactHtml}</td>
                </tr>
                """);
        }

        string html = $$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <title>MaPS QE UI Test Execution Report</title>
                <style>
                    body { font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif; margin: 2rem; background: #f8fafc; color: #0f172a; }
                    h1 { margin-bottom: 0.25rem; }
                    .meta { color: #475569; margin-bottom: 1.5rem; font-size: 0.95rem; }
                    .cards { display: flex; gap: 1rem; margin-bottom: 1.5rem; flex-wrap: wrap; }
                    .card { background: #fff; border: 1px solid #e2e8f0; border-radius: 8px; padding: 1rem 1.5rem; min-width: 140px; }
                    .card .num { font-size: 1.75rem; font-weight: 700; }
                    .pass { color: #15803d; } .fail { color: #b91c1c; } .skip { color: #a16207; }
                    table { width: 100%; border-collapse: collapse; background: #fff; border: 1px solid #e2e8f0; border-radius: 8px; overflow: hidden; }
                    th, td { text-align: left; padding: 0.75rem 1rem; border-bottom: 1px solid #e2e8f0; vertical-align: top; }
                    th { background: #f1f5f9; font-weight: 600; }
                    .badge { padding: 0.2rem 0.6rem; border-radius: 999px; font-size: 0.8rem; font-weight: 600; }
                    .badge-pass { background: #dcfce7; color: #166534; }
                    .badge-fail { background: #fee2e2; color: #991b1b; }
                    .badge-skip { background: #fef9c3; color: #854d0e; }
                    .muted { color: #64748b; font-size: 0.85rem; }
                    .error-box { background: #fef2f2; border: 1px solid #fecaca; padding: 0.5rem; border-radius: 4px; white-space: pre-wrap; font-size: 0.8rem; margin: 0; max-width: 460px; }
                </style>
            </head>
            <body>
                <h1>GOV.UK Holiday Entitlement — Automated Test Report</h1>
                <div class="meta">Generated (UTC): {{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}} | Total Duration: {{totalSeconds:F2}}s</div>
                <div class="cards">
                    <div class="card"><div>Total</div><div class="num">{{ordered.Count}}</div></div>
                    <div class="card"><div>Passed</div><div class="num pass">{{passed}}</div></div>
                    <div class="card"><div>Failed</div><div class="num fail">{{failed}}</div></div>
                    <div class="card"><div>Skipped</div><div class="num skip">{{skipped}}</div></div>
                </div>
                <table>
                    <thead>
                        <tr>
                            <th>Status</th>
                            <th>Test</th>
                            <th>Category</th>
                            <th>Duration</th>
                            <th>Diagnostics</th>
                            <th>Artifacts</th>
                        </tr>
                    </thead>
                    <tbody>
                        {{rows}}
                    </tbody>
                </table>
            </body>
            </html>
            """;

        File.WriteAllText(reportPath, html, Encoding.UTF8);
        return reportPath;
    }
}