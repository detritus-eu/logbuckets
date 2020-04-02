using LogBuckets.Models;
using LogBuckets.Shared.Pipes;

namespace LogBuckets.Components.Buckets
{
    internal interface IBucketTab
    {
        string Header { get; }

        Configuration Config { get; set; }

        IBucket Bucket { get; } 
    }
}
