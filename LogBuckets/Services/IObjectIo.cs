namespace LogBuckets.Services
{

    internal interface IObjectIo
    {
        void Save<T>(ref T obj, string filename);
        T Load<T>(string filename);
    }
}
