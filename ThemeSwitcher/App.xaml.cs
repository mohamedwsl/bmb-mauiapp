using ThemeSwitcher.Themes;
namespace ThemeSwitcher
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JEaF5cXmRCd0xyWmFZfVtgdVVMYFhbQX5PMyBoS35Rc0VrWXpfcHRQQmFZWUxwVEFd");
            InitializeComponent();
            LoadTheme();

        }
        public void LoadTheme(string? theme = null)
        {
            var themeToLoad = theme ?? Preferences.Default.Get("AppTheme", "Light");

            var dict = new ResourceDictionary();

            switch (themeToLoad)
            {
                case "Dark":
                    dict.MergedDictionaries.Add(new Dark());
                    break;
                default:
                    dict.MergedDictionaries.Add(new Light());
                    break;
            }

            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(dict);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Set the root page here instead of in the constructor
            return new Window(new AppShell());
        }
    }
}