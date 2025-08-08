using CommunityToolkit.Mvvm.Input;
using ThemeSwitcher.Models;

namespace ThemeSwitcher.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}