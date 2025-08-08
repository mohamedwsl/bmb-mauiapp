using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace ThemeSwitcher
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            var theme = Preferences.Default.Get("AppTheme", "Light");
            ThemePicker.SelectedIndex = theme == "Dark" ? 1 : 0;
        }
        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"),
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(0),
                Font = Font.SystemFontOfSize(18),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }

        public static async Task DisplayToastAsync(string message)
        {
            // Toast is currently not working in MCT on Windows
            if (OperatingSystem.IsWindows())
                return;

            var toast = Toast.Make(message, textSize: 18);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }

        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTheme = ThemePicker.SelectedIndex == 1 ? "Dark" : "Light";

            Preferences.Default.Set("AppTheme", selectedTheme);

            if (Application.Current is App app)
                app.LoadTheme(selectedTheme);
        }
    }
}
