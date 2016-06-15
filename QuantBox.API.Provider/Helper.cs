using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQuant;
using System.IO;
using System.Reflection;

namespace QuantBox.APIProvider
{
    static class Helper
    {
        public static readonly Uri RootPath;

        static Helper()
        {
            //Assembly.GetExecutingAssembly().
            RootPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        public static string MakeRelativePath(string path)
        {
            if (path == null)
                return null;

            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri)) {
                return Uri.UnescapeDataString(RootPath.MakeRelativeUri(uri).ToString());
            }
            return Uri.UnescapeDataString(path);
        }
        public static string MakeAbsolutePath(string path)
        {
            if (path == null)
                return null;

            Uri uri;
            if (Uri.TryCreate(RootPath, path, out uri)) {
                return Uri.UnescapeDataString(uri.LocalPath);
            }
            return Uri.UnescapeDataString(path);
        }
    }
}
