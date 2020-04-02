using LogBuckets.Models;

namespace LogBuckets.Services
{
    internal interface IConfigurationManager
    {
        Configuration Config { get; }

        void Initialize(string path);
    }
}
