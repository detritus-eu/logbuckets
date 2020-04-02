using Newtonsoft.Json;
using System;
using System.IO;

namespace LogBuckets.Services
{
    internal sealed class ObjectIo : IObjectIo
    {
        public void Save<T>(ref T obj, string filename)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));

            try
            {
                var dir = Path.GetDirectoryName(filename);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var json = JsonConvert.SerializeObject(obj);
                File.WriteAllText(filename, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"Error serializing object: {ex.Message}", ex);
            }
        }

        public T Load<T>(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename)) throw new FileNotFoundException($"Unable to locate file '{filename}'");

            try
            {
                var json = File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new IOException($"Error deserializing object: {ex.Message}", ex);
            }
        }
    }
}
