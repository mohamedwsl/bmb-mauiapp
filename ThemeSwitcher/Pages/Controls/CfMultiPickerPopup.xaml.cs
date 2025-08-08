using System.Collections.ObjectModel;
namespace ThemeSwitcher.Pages.Controls;

public partial class CfMultiPickerPopup : ContentPage
{
    public static readonly BindableProperty SelectedItemsProperty =
    BindableProperty.Create(
        nameof(SelectedItems),
        typeof(ObservableCollection<string>),
        typeof(CfMultiPickerPopup),  // Your class name here
        defaultValue: new ObservableCollection<string>(),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: SelectedItemsPropertyChanged);

    private static void SelectedItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CfMultiPickerPopup)bindable;
        var selectedItems = newValue as ObservableCollection<string>;

        // You can update internal state, UI, or fire events here
        Console.WriteLine($"Selected items changed: {string.Join(", ", selectedItems ?? [])}");
    }

    public ObservableCollection<ThemeOption> ThemeOptions { get; set; } = new();

    public ObservableCollection<string> SelectedItems
    {
        get => (ObservableCollection<string>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    public CfMultiPickerPopup()
    {
        InitializeComponent();
        BindingContext = this;

        // Predefined available themes
        ThemeOptions.Add(new ThemeOption { Name = "Light" });
        ThemeOptions.Add(new ThemeOption { Name = "Dark" });
        ThemeOptions.Add(new ThemeOption { Name = "High Contrast" });
    }
    public event EventHandler<ObservableCollection<string>>? SelectionConfirmed;

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        SelectionConfirmed?.Invoke(this, SelectedItems);
        await Navigation.PopModalAsync();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}

public class ThemeOption
{
    public required string Name { get; set; }
    public bool IsSelected { get; set; }
}