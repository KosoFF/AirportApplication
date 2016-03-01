namespace Airport.Core.DependencyInjection
{
    internal class InstanceBinding<TSource, TReal> : TypeBindingBase where TReal : TSource
    {
        private readonly TReal _instance;

        public InstanceBinding(TReal instance)
        {
            _instance = instance;
        }

        internal override object GetRealInstance()
        {
            return _instance;
        }
    }
}