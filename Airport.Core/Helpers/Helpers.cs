using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.UI.Core;
using Airport.Core.MessageServices;

namespace Airport.Core.Helpers
{
    public static class Helpers
    {
        public static string ComputeMd5(string str)
        {
            if (str == null) return null;
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            var buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

        public static async Task ShowErrors(Dictionary<string, string[]> errorsDictionary,
            IMessageService messageService)
        {
            if (messageService == null || errorsDictionary == null || errorsDictionary.Count == 0) return;

            foreach (var error in errorsDictionary)
            {
                foreach (var errorMessage in error.Value)
                {
                    await messageService.ShowAsync(error.Key, errorMessage);
                }
            }
        }

        public static string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public static bool IsInternetConnected()
        {
            var connections = NetworkInformation.GetInternetConnectionProfile();
            var internet = connections != null &&
                           connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        public static async Task RunOnTheUiThread(Func<Task> action)
        {
            await
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    async () => { await action(); });
        }
    }
}