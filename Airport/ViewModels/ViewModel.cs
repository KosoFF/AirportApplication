using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Airport.Core.Commands;
using Airport.Core.DependencyInjection;
using Airport.Core.Navigation;
using Airport.Core.Observable;

namespace Airport.ViewModels
{
    public class ViewModel : ObservableObject, INavigationAware
    {
        private bool _isLoading;

        public ViewModel()
        {
            NavigationService = ServiceLocator.Locator.Get<INavigationService>();
            GoBackCommand = new RelayCommand(GoBackCommandExecute, CanGobackCommandExecute);
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (Set(ref _isLoading, value))
                    OnIsLoadingChanged();
            }
        }

        public RelayCommand GoBackCommand { get; set; }
        public INavigationService NavigationService { get; protected set; }

        public virtual void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public virtual void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        ~ViewModel()
        {
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            // GoBackCommand.Execute();
        }


        private void GoBackCommandExecute()
        {
            NavigationService.GoBack();
        }

        private bool CanGobackCommandExecute()
        {
            return NavigationService.CanGoBack;
        }

        protected virtual void OnIsLoadingChanged()
        {
        }
    }
}