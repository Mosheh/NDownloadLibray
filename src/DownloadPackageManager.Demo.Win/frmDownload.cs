using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadPackageManager.Demo.Win
{
    public partial class frmDownload : Form
    {
        public frmDownload()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            try
            {
                var downInfo = new DownloadInfo() { Url = textBoxUrl.Text, DestinationFileName = textBoxDestFile.Text };
                downInfo.OnProgressDownloading += DownInfo_OnProgressDownloading;
                downInfo.OnInitProgressDownload += DownInfo_OnInitProgressDownload;
                new DownloadManager().Download(new List<DownloadInfo>() { downInfo });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void DownInfo_OnInitProgressDownload(object sender, Events.DownloadProgressArgs args)
        {
            System.Windows.Forms.Application.DoEvents();
            progressBar1.BeginInvoke(
               new Action(() =>
               {
                   progressBar1.Value = 0;
               }));
        }

        private void DownInfo_OnProgressDownloading(object sender, Events.DownloadProgressArgs args)
        {
            System.Windows.Forms.Application.DoEvents();
            progressBar1.BeginInvoke(
               new Action(() =>
               {
                   progressBar1.Value = args.Percent;
               }));
        }

        private void buttonFileDestination_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();

            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                textBoxDestFile.Text = saveDialog.FileName;
            }
        }
    }
}
