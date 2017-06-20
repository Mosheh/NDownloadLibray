using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadPackageManager.Events
{
    public delegate void InitProgressDownload(object sender, DownloadProgressArgs args);
    public delegate void ProgressDownloading(object sender, DownloadProgressArgs args);
    public delegate void FinishProgressDownloading(object sender, DownloadProgressArgs args);
    public class DownloadProgressArgs
    {
        public int Percent { get; set; }
    }
}
