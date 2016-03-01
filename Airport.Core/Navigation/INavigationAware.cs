using Windows.UI.Xaml.Navigation;

namespace Airport.Core.Navigation
{
    public interface INavigationAware
    {
        void OnNavigatedTo(NavigationEventArgs e);

        void OnNavigatedFrom(NavigationEventArgs e);
    }
}