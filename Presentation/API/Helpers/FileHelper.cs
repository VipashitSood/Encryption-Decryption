using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace API.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Save base64 file
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="base64Data"></param>
        /// <returns></returns>
        public static bool Savefile(string filepath,string fullPath, string base64Data)
        {
            bool result = false;
            try
            {
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                // Check if the file already exists and delete it if needed
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                // Save file from base64
                File.WriteAllBytes(fullPath, Convert.FromBase64String(base64Data));
                result = true;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
