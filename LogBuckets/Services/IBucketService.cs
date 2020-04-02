using LogBuckets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBuckets.Services
{
    internal interface IBucketService
    {
        IEnumerable<string> GetIds();
        string GetPathForId(string id);
        string GetIdFromPath(string path);
        void Save(BucketDto dto);
        BucketDto Load(string id);
        void Delete(string id);
    }
}
