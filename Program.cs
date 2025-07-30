using System;

namespace BeebopNotify

{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var notifier = new IBMNotifier
            {
                Type = "popup",
                Title = "System Update",
                Subtitle = "Security patch available",
                BarTitle = "IT Department",
                IconPath = "/System/Library/CoreServices/CoreTypes.bundle/Contents/Resources/ToolbarInfo.icns",
                Timeout = 60,
                AlwaysOnTop = true,

                MainButtonLabel = "Install Now",
                MainButtonCtaType = "open",
                MainButtonCtaPayload = "https://update.server/patch",

                SecondaryButtonLabel = "Remind Later",
                TertiaryButtonLabel = "Release Notes"
            };

            // Execute and handle results
            var (exitCode, output, error) = notifier.Run();

            Console.WriteLine($"Exit Code: {exitCode}");
            Console.WriteLine($"Output: {output}");
            Console.WriteLine($"Error: {error}");

            // Additional application logic
        }
    }
}