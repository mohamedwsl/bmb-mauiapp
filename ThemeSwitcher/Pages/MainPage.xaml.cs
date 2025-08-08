using System.Collections.ObjectModel;
using ThemeSwitcher.Models;
using ThemeSwitcher.PageModels;
using ThemeSwitcher.Pages.Controls;

namespace ThemeSwitcher.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var savedTheme = Preferences.Default.Get("AppTheme", "Light");
            ThemeSwitch.IsToggled = savedTheme == "Dark";
        }

        private void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            var newTheme = e.Value ? "Dark" : "Light";
            Preferences.Default.Set("AppTheme", newTheme);

            if (Application.Current is App app)
                app.LoadTheme(newTheme);
        }

        private async void OnOpenPickerClicked(object sender, EventArgs e)
        {
            var picker = new CfMultiPickerPopup();
            picker.SelectionConfirmed += OnThemesSelected;
            await Navigation.PushModalAsync(picker);
        }

        private void OnThemesSelected(object sender, ObservableCollection<string> selectedItems)
        {
            SelectedThemesLabel.Text = "Selected: " + string.Join(", ", selectedItems);
        }
    }

}