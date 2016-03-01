using System;
using System.Collections.Generic;

namespace Airport.Core.Navigation
{
    public class PageResolver
    {
        private readonly Dictionary<string, Type> _pages;

        public PageResolver(Dictionary<string, Type> pagesWrapper)
        {
            if (pagesWrapper == null) throw new ArgumentNullException(nameof(pagesWrapper));
            _pages = pagesWrapper;
        }

        public Type GetPageByKey(string pageKey)
        {
            Type page = null;
            if (_pages.TryGetValue(pageKey, out page))
            {
                return page;
            }

            throw new KeyNotFoundException(string.Format("{0} not found.", pageKey));
        }
    }
}