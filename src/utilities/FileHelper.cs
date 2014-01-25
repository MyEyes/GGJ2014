using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Otherworld
{
    public static class FileHelper
    {
        [DllImport ("Shlwapi.dll")]
        private static extern bool PathRelativePathTo (StringBuilder result, string src,
            FileAttributes fromAttr, string dest, FileAttributes destAttr);

        public static void EnsureFileFolderExists (string file)
        {
            var info = new FileInfo (file);
            if (info.Directory == null)
                throw new InvalidOperationException();

            if (!info.Directory.Exists)
                info.Directory.Create();
        }

        public static bool GetRelative (string src, string dest, ref string result)
        {
            var getAttr = (Func<String, FileAttributes>)(i =>
                IsPathDirectory (i) ? FileAttributes.Directory : 0);

            var builder = new StringBuilder (260 /*Max path size*/);
            try
            {
                var success = PathRelativePathTo (builder,
                    src, getAttr (src),
                    dest, getAttr (dest));
                result = builder.ToString();
                return success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsPathDirectory (string path)
        {
            return (File.GetAttributes (path) & FileAttributes.Directory)
                == FileAttributes.Directory;
        }

        public static String PathWithoutExtension (string path)
        {
            var extension = Path.GetExtension (path);
            var extensionIndex = path.Length - extension.Length;
            var newPath = path.Remove (extensionIndex, extension.Length);

            return newPath;
        }

        public static Object QueryFilesRecursive (DirectoryInfo rootDir, Func<FileInfo, Object> action, String filePattern = "*")
        {
            foreach (FileInfo file in rootDir.EnumerateFiles (filePattern))
            {
                Object result = action (file);
                if (result != null)
                    return result;
            }

            return rootDir
                .EnumerateDirectories()
                .Select (dir => QueryFilesRecursive (dir, action, filePattern))
                .FirstOrDefault (o => o != null);
        }
    }
}
