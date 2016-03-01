namespace Airport.Core.DependencyInjection
{
    internal class TypeBinding<TSource, TReal> : TypeBindingBase
        where TReal : TSource, new()
    {
        private readonly object _singletonSyncObject = new object();
        private TReal _singletonInstance;

        internal override object GetRealInstance()
        {
            object result = null;
            if (Mode == LifetimeMode.Singleton)
            {
                if (_singletonInstance == null)
                {
                    lock (_singletonSyncObject)
                    {
                        if (_singletonInstance == null)
                        {
                            _singletonInstance = new TReal();
                        }
                    }
                }

                result = _singletonInstance;
            }
            else
            {
                result = new TReal();
            }

            return result;
        }
    }
}