using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogBuckets.Shared.Pipes
{
    internal sealed class AudioAlert: StringEventPipe, IAudioAlert
    {
        public static readonly string AudioDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "audio");


        private readonly SoundPlayer _soundPlayer;

        static AudioAlert()
        {
            if (!Directory.Exists(AudioDirectory))
                Directory.CreateDirectory(AudioDirectory);
        }


        public AudioAlert()
        {
            _soundPlayer = new SoundPlayer();
        }

        private string _filename;
        public string Filename
        {
            get { return _filename; }
            set
            {
                if (_filename != value)
                {
                    _filename = value;
                    _soundPlayer.SoundLocation = value;
                    _soundPlayer.Load();
                }
            }
        }

        protected override string Process(string data)
        {
            if (File.Exists(Filename) && _soundPlayer.IsLoadCompleted)
            {
                _soundPlayer.Play();
            }
            return data;
        }

    }
}
