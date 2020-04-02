using LogBuckets.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBuckets.Services
{
    internal sealed class BucketService: IBucketService
    {

        #region Private Fields

        private readonly IObjectIo _io;
        private readonly IConfigurationManager _configMgr;

        #endregion

        #region Ctor

        public BucketService(
            IObjectIo io,
            IConfigurationManager configMgr)
        {
            _io = io ?? throw new ArgumentNullException(nameof(io));
            _configMgr = configMgr ?? throw new ArgumentNullException(nameof(configMgr));
        }


        #endregion

        #region Public Interface

        public IEnumerable<string> GetIds()
        {
            foreach (var file in Directory.EnumerateFiles(_configMgr.Config.BucketDirectory, $"*.{_configMgr.Config.BucketExtension}"))
                yield return GetIdFromPath(file);
        }

        public string GetPathForId(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var filename = Path.ChangeExtension(id, _configMgr.Config.BucketExtension);
            return Path.Combine(_configMgr.Config.BucketDirectory, filename);
        }

        public string GetIdFromPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            return Path.ChangeExtension(Path.GetFileName(path), null);
        }

        public BucketDto Load(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var path = GetPathForId(id);
            if (!File.Exists(path)) throw new FileNotFoundException("File not found", path);

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<BucketDto>(json);
        }

        public void Save(BucketDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var path = GetPathForId(dto.Id);
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            var json = JsonConvert.SerializeObject(dto);
            File.WriteAllText(path, json);
        }

        public void Delete(string id) => File.Delete(GetPathForId(id));

        #endregion


    }
}
