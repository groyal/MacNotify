using System.Diagnostics;
using System.Collections.ObjectModel;

namespace BeebopNotify;
public class IBMNotifier
{
    // Required properties
    public string? Type { get; set; }
    public string? Payload { get; set; }

    // Notification content properties
    public string? BarTitle { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? IconPath { get; set; }

    // Accessory view properties
    public string? AccessoryViewType { get; set; }
    public string? AccessoryViewPayload { get; set; }

    // Main button properties
    public string? MainButtonLabel { get; set; }
    public string? MainButtonCtaType { get; set; }
    public string? MainButtonCtaPayload { get; set; }

    // Secondary button properties
    public string? SecondaryButtonLabel { get; set; }
    public string? SecondaryButtonCtaType { get; set; }
    public string? SecondaryButtonCtaPayload { get; set; }

    // Tertiary button properties
    public string? TertiaryButtonLabel { get; set; }
    public string? TertiaryButtonCtaType { get; set; }
    public string? TertiaryButtonCtaPayload { get; set; }

    // Help button properties
    public string? HelpButtonCtaType { get; set; }
    public string? HelpButtonCtaPayload { get; set; }

    // Control properties
    public int? Timeout { get; set; }
    public bool AlwaysOnTop { get; set; }

    // this should be in the app.config
    // dont escape the blank 
    // I dropped the IBM notifier app into the /Applications folder. 
    private const string ExecutablePath = "/Applications/IBM Notifier.app/Contents/MacOS/IBM Notifier";

    public (int ExitCode, string Output, string Error) Run()
    {
        string expandedPath = ExecutablePath.Replace("~",
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        if (!File.Exists(expandedPath))
        {
            throw new FileNotFoundException("IBM Notifier not found at: " + expandedPath);
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = expandedPath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        // Add arguments dynamically
        AddArgumentIfSet(startInfo.ArgumentList, "-type", Type);
        AddArgumentIfSet(startInfo.ArgumentList, "-payload", Payload);
        AddArgumentIfSet(startInfo.ArgumentList, "-bar_title", BarTitle);
        AddArgumentIfSet(startInfo.ArgumentList, "-title", Title);
        AddArgumentIfSet(startInfo.ArgumentList, "-subtitle", Subtitle);
        AddArgumentIfSet(startInfo.ArgumentList, "-icon_path", IconPath);
        AddArgumentIfSet(startInfo.ArgumentList, "-accessory_view_type", AccessoryViewType);
        AddArgumentIfSet(startInfo.ArgumentList, "-accessory_view_payload", AccessoryViewPayload);
        AddArgumentIfSet(startInfo.ArgumentList, "-main_button_label", MainButtonLabel);
        AddArgumentIfSet(startInfo.ArgumentList, "-main_button_cta_type", MainButtonCtaType);
        AddArgumentIfSet(startInfo.ArgumentList, "-main_button_cta_payload", MainButtonCtaPayload);
        AddArgumentIfSet(startInfo.ArgumentList, "-secondary_button_label", SecondaryButtonLabel);
        AddArgumentIfSet(startInfo.ArgumentList, "-secondary_button_cta_type", SecondaryButtonCtaType);
        AddArgumentIfSet(startInfo.ArgumentList, "-secondary_button_cta_payload", SecondaryButtonCtaPayload);
        AddArgumentIfSet(startInfo.ArgumentList, "-tertiary_button_label", TertiaryButtonLabel);
        AddArgumentIfSet(startInfo.ArgumentList, "-tertiary_button_cta_type", TertiaryButtonCtaType);
        AddArgumentIfSet(startInfo.ArgumentList, "-tertiary_button_cta_payload", TertiaryButtonCtaPayload);
        AddArgumentIfSet(startInfo.ArgumentList, "-help_button_cta_type", HelpButtonCtaType);
        AddArgumentIfSet(startInfo.ArgumentList, "-help_button_cta_payload", HelpButtonCtaPayload);

        if (Timeout.HasValue)
        {
            startInfo.ArgumentList.Add("-timeout");
            startInfo.ArgumentList.Add(Timeout.Value.ToString());
        }

        if (AlwaysOnTop)
        {
            startInfo.ArgumentList.Add("-always_on_top");
        }

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        return (process.ExitCode, output, error);
    }

    private void AddArgumentIfSet(Collection<string> argumentList, string flag, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            argumentList.Add(flag);
            argumentList.Add(value);
        }
    }
}