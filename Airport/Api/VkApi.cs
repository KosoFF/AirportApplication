using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Airport.Api
{
    public class VkApi
    {
        private const int scope = (int) (VkontakteScopeList.friends | VkontakteScopeList.email);
        private const string appId = "5284249";

        public static void SignIn()
        {
            var webBrowser1 = new WebView();
            webBrowser1.Visibility = Visibility.Visible;
            webBrowser1.Navigate(
                new Uri(
                    string.Format(
                        "http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token",
                        appId, scope)));
        }

        private enum VkontakteScopeList
        {
            /// <summary>
            ///     Пользователь разрешил отправлять ему уведомления.
            /// </summary>
            notify = 1,

            /// <summary>
            ///     Доступ к друзьям.
            /// </summary>
            friends = 2,

            /// <summary>
            ///     Доступ к фотографиям.
            /// </summary>
            photos = 4,

            /// <summary>
            ///     Доступ к аудиозаписям.
            /// </summary>
            audio = 8,

            /// <summary>
            ///     Доступ к видеозаписям.
            /// </summary>
            video = 16,

            /// <summary>
            ///     Доступ к предложениям (устаревшие методы).
            /// </summary>
            offers = 32,

            /// <summary>
            ///     Доступ к вопросам (устаревшие методы).
            /// </summary>
            questions = 64,

            /// <summary>
            ///     Доступ к wiki-страницам.
            /// </summary>
            pages = 128,

            /// <summary>
            ///     Добавление ссылки на приложение в меню слева.
            /// </summary>
            link = 256,

            /// <summary>
            ///     Доступ заметкам пользователя.
            /// </summary>
            notes = 2048,

            /// <summary>
            ///     (для Standalone-приложений) Доступ к расширенным методам работы с сообщениями.
            /// </summary>
            messages = 4096,

            /// <summary>
            ///     Доступ к обычным и расширенным методам работы со стеной.
            /// </summary>
            wall = 8192,

            /// <summary>
            ///     Доступ к документам пользователя.
            /// </summary>
            docs = 131072,

            /// <summary>
            ///     Доступ к e-mail
            /// </summary>
            email = 4194304
        }
    }
}