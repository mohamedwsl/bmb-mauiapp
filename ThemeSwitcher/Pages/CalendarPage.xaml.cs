using Microsoft.Maui.Controls;
using Plugin.LocalNotification;
using System.Collections.ObjectModel;
using System.Text.Json;
using Syncfusion.Maui.Calendar;

namespace ThemeSwitcher.Pages
{
    public partial class CalendarPage : ContentPage
    {
        private const string EventsKey = "SavedEvents";
        public ObservableCollection<CalendarEvent> Events { get; set; } = new();

        public CalendarPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadEvents();
        }

        private async void OnAddEventClicked(object sender, EventArgs e)
        {
            var title = await DisplayPromptAsync("New Event", "Enter event title:");
            if (string.IsNullOrWhiteSpace(title)) return;

            var dateString = await DisplayPromptAsync("Event Date", "Enter date (yyyy-MM-dd HH:mm):");
            if (!DateTime.TryParse(dateString, out var eventDate)) return;

            var newEvent = new CalendarEvent { Title = title, Date = eventDate };
            Events.Add(newEvent);
            SaveEvents();
            ScheduleNotification(newEvent);
        }

        private async void OnEventTapped(object sender, EventArgs e)
        {
            // Replace Frame with Border to fix CS0618
            if (sender is Border border && border.BindingContext is CalendarEvent ev)
            {
                string action = await DisplayActionSheet("Event Options", "Cancel", null, "Edit", "Delete");

                if (action == "Edit")
                {
                    var newTitle = await DisplayPromptAsync("Edit Event", "Enter new title:", initialValue: ev.Title);
                    if (!string.IsNullOrWhiteSpace(newTitle))
                    {
                        ev.Title = newTitle;
                        SaveEvents();
                    }
                }
                else if (action == "Delete")
                {
                    Events.Remove(ev);
                    SaveEvents();
                }
            }
        }

        private void ScheduleNotification(CalendarEvent ev)
        {
            var request = new NotificationRequest
            {
                NotificationId = ev.Id,
                Title = "Upcoming Event",
                Description = ev.Title,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = ev.Date.AddMinutes(-1)
                }
            };

            LocalNotificationCenter.Current.Show(request);
        }

        private void SaveEvents()
        {
            var json = JsonSerializer.Serialize(Events);
            Preferences.Default.Set(EventsKey, json);
        }

        private void LoadEvents()
        {
            if (Preferences.Default.ContainsKey(EventsKey))
            {
                var json = Preferences.Default.Get(EventsKey, "");
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        var savedEvents = JsonSerializer.Deserialize<ObservableCollection<CalendarEvent>>(json);
                        if (savedEvents != null)
                        {
                            Events.Clear();
                            foreach (var ev in savedEvents)
                                Events.Add(ev);
                        }
                    }
                    catch { }
                }
            }
        }

        private void Calendar_SelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
        {
            if (e.NewValue is DateTime selectedDate)
            {
                // Example: scroll or filter events by the selected date
                var selectedDayEvents = Events
                    .Where(ev => ev.Date.Date == selectedDate.Date)
                    .OrderBy(ev => ev.Date)
                    .ToList();

                if (selectedDayEvents.Count == 0)
                {
                    DisplayAlert("No Events", $"No events found for {selectedDate:MMMM dd, yyyy}.", "OK");
                }
                else
                {
                    var summary = string.Join("\n", selectedDayEvents.Select(ev => $"{ev.Date:HH:mm} - {ev.Title}"));
                    DisplayAlert("Events", summary, "OK");
                }
            }
        }

        private async void EventList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is CalendarEvent selectedEvent)
            {
                string action = await DisplayActionSheet("Event Options", "Cancel", null, "Edit", "Delete");

                if (action == "Edit")
                {
                    var newTitle = await DisplayPromptAsync("Edit Event", "Enter new title:", initialValue: selectedEvent.Title);
                    if (!string.IsNullOrWhiteSpace(newTitle))
                    {
                        selectedEvent.Title = newTitle;
                        SaveEvents();
                        // Refresh UI if needed (e.g., CollectionView.ItemsSource = null; then reassign Events)
                    }
                }
                else if (action == "Delete")
                {
                    Events.Remove(selectedEvent);
                    SaveEvents();
                }

                // Clear selection so the user can tap it again later
                if (sender is CollectionView collectionView)
                {
                    collectionView.SelectedItem = null;
                }
            }
        }

    }

    public class CalendarEvent
    {
        public int Id { get; set; } = new Random().Next(1, 1000000);
        public required string Title { get; set; }
        public DateTime Date { get; set; }
    }
}
