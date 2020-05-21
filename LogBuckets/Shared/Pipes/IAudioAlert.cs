using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBuckets.Shared.Pipes
{
    internal interface IAudioAlert: IStringEventPipe
    {
        string Filename { get; set; }
    }
}
