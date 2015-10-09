using DownloadPackageManager.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadPackageManager
{
    public class DownloadInfo
    {
        public event InitProgressDownload OnInitProgressDownload;
        public event ProgressDownloading OnProgressDownloading;

        public string Url { get; set; }
        public string DestinationFileName { get; set; }

        private int _downloadPercent;
        private bool firstInitProgress;

        public int DownloadPercent
        {
            get { return _downloadPercent; }
            set
            {
                _downloadPercent = value;
                if (value == 0 && !firstInitProgress)
                {
                    SetInitProgressDownload();
                    firstInitProgress = true;
                }
                SetOnProgressDownloading();
            }
        }

        private void SetInitProgressDownload()
        {
            if (OnInitProgressDownload != null)
                OnInitProgressDownload(this, new DownloadProgressArgs() { Percent = 0 });
        }

        private void SetOnProgressDownloading()
        {
            if (OnProgressDownloading != null)
                OnProgressDownloading(this, new DownloadProgressArgs { Percent = _downloadPercent });
        }
    }
}

