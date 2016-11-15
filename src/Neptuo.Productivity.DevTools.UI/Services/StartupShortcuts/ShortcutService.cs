using Neptuo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.DevTools.Services.StartupShortcuts
{
    public class ShortcutService
    {
        private readonly string companyName;
        private readonly string suiteName;
        private readonly string productName;

        public ShortcutService(string companyName, string suiteName, string productName)
        {
            Ensure.NotNull(companyName, "companyName");
            Ensure.NotNull(suiteName, "suiteName");
            Ensure.NotNull(productName, "productName");
            this.companyName = companyName;
            this.suiteName = suiteName;
            this.productName = productName;
        }

        private string GetFileName()
        {
            return productName + ".appref-ms";
        }

        public bool Exists(Environment.SpecialFolder folder)
        {
            return File.Exists(Path.Combine(Environment.GetFolderPath(folder), GetFileName()));
        }

        public void Create(Environment.SpecialFolder folder)
        {
            //if (ApplicationDeployment.IsNetworkDeployed)

            string sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), companyName, suiteName, GetFileName());
            string targetPath = Path.Combine(Environment.GetFolderPath(folder), GetFileName());
            if (File.Exists(sourcePath))
                File.Copy(sourcePath, targetPath, true);
        }

        public void Delete(Environment.SpecialFolder folder)
        {
            //if (ApplicationDeployment.IsNetworkDeployed)

            string targetPath = Path.Combine(Environment.GetFolderPath(folder), GetFileName());
            if (File.Exists(targetPath))
                File.Delete(targetPath);
        }
    }
}
