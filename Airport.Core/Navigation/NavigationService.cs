using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Airport.Core.Navigation
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;
        private PageResolver _pageResolver;
        private INavigationAware _previousPage;
        public event NavigatedEventHandler Navigated;
        public event NavigatingCancelEventHandler Navigating;
        public event NavigationFailedEventHandler NavigationFailed;
        public event NavigationStoppedEventHandler NavigationStopped;

        public void Initialize(Frame frame, PageResolver pageResolver)
        {
            if (frame == null)
            {
                throw new ArgumentNullException(nameof(frame));
            }

            _pageResolver = pageResolver;
            _frame = frame;
            _frame.Navigated += OnNavigated;
            _frame.Navigating += OnNavigating;
            _frame.NavigationFailed += OnNavigationFailed;
            _frame.NavigationStopped += OnNavigationStopped;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Debug.WriteLine("Frame initialized");
        }


        public void ResetNavigationHistory()
        {
            _frame?.BackStack?.Clear();
        }

        public bool Navigate(string pageKey)
        {
            var page = _pageResolver.GetPageByKey(pageKey);
            return _frame.Navigate(page);
        }

        public bool NavigateWithoutHistory(string pageKey)
        {
            var page = _pageResolver.GetPageByKey(pageKey);
            var res = _frame.Navigate(page);
            ResetNavigationHistory();
            return res;
        }

        public bool Navigate(string pageKey, object parameter)
        {
            var page = _pageResolver.GetPageByKey(pageKey);
            return _frame.Navigate(page, parameter);
        }

        public void GoBack()
        {
            _frame.GoBack();
        }

        public bool CanGoBack
        {
            get { return _frame.CanGoBack; }
        }

        public IList<PageStackEntry> BackStack
        {
            get { return _frame.BackStack; }
        }

        public int BackStackDepth
        {
            get { return _frame.BackStackDepth; }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (CanGoBack) GoBack();
        }


        private void OnNavigationStopped(object sender, NavigationEventArgs e)
        {
            var handler = NavigationStopped;
            handler?.Invoke(sender, e);
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            var handler = NavigationFailed;
            handler?.Invoke(sender, e);
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var handler = Navigating;
            handler?.Invoke(sender, e);

            //if (!e.Cancel)
            //{
            //    switch (e.NavigationMode)
            //    {
            //        case NavigationMode.Back:
            //            NavigatedFrom(e);
            //            break;
            //        case NavigationMode.Forward:
            //            break;
            //        case NavigationMode.New:
            //            NavigatedFrom(e);
            //            break;
            //        case NavigationMode.Refresh:
            //            break;
            //    }
            //}
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var handler = Navigated;
            handler?.Invoke(sender, e);

            switch (e.NavigationMode)
            {
                case NavigationMode.Back:
                    NavigatedTo(e);
                    break;
                case NavigationMode.Forward:
                    break;
                case NavigationMode.New:
                    NavigatedTo(e);
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = CanGoBack
            //    ? AppViewBackButtonVisibility.Visible
            //    : AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        ///     Called when the implementer is being navigated away from.
        /// </summary>
        /// <param name="args">Provides data for navigation methods and event handlers that cannot cancel the navigation request.</param>
        private void NavigatedFrom(NavigatingCancelEventArgs args)
        {
            //var navigationAware = GetNavigationAware(frame.Content);
            //if (navigationAware == null) return;
            //navigationAware.OnNavigatedFrom(args);
        }

        /// <summary>
        ///     Called when the implementer has been navigated to.
        /// </summary>
        /// <param name="args">Provides data for navigation methods and event handlers that cannot cancel the navigation request.</param>
        private void NavigatedTo(NavigationEventArgs args)
        {
            if (_previousPage != null)
                _previousPage.OnNavigatedFrom(args);

            _previousPage = GetNavigationAware(args.Content);
            if (_previousPage == null) return;
            _previousPage.OnNavigatedTo(args);
        }

        private static INavigationAware GetNavigationAware(object content)
        {
            if (content == null) return null;
            var frameworkElement = content as FrameworkElement;
            if (frameworkElement == null) return null;
            return frameworkElement.DataContext as INavigationAware;
        }
    }
}