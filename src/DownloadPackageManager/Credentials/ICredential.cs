using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadPackageManager.Credentials
{
    public interface ICredential
    {
        string UserName { get; set; }
        string UserPassword { get; set; }
    }
}
