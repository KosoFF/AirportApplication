using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Airport.Core.Navigation
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        IList<PageStackEntry> BackStack { get; }
        int BackStackDepth { get; }
        event NavigatedEventHandler Navigated;
        event NavigatingCancelEventHandler Navigating;
        event NavigationFailedEventHandler NavigationFailed;
        event NavigationStoppedEventHandler NavigationStopped;
        void Initialize(Frame frame, PageResolver pageResolver);
        bool Navigate(string pageKey);
        bool NavigateWithoutHistory(string pageKey);
        bool Navigate(string pageKey, object parameter);
        void GoBack();
        void ResetNavigationHistory();
    }
}