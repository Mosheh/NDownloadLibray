using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadPackageManager
{
    public interface IDownloadPackageManager
    {
        void Download(string url, string fileName);
        void Download(List<DownloadInfo> list);
        List<DownloadInfo> DownloadList { get; set; }
    }
}
