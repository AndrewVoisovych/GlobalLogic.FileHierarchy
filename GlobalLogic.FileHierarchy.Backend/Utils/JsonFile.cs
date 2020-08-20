using System;
using System.IO;

namespace GlobalLogic.FileHierarchy.Backend.Utils
{
    /// <summary>
    /// Class that has methods for creating a json file
    /// </summary>
    public static class JsonFile
    {
        /// <summary>
        /// Json file creation
        /// </summary>
        /// <param name="json">json text/string</param>
        /// <param name="path">the path to the file</param>
        /// <param name="folderName">selected folder name</param>
        /// <returns></returns>
        public static string Create(string json, string path, string folderName)
        {
            // create directory for save json
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Prohibited characters for file name
            folderName = String.Join("", folderName.Split(':', '*', '?', '<', '>', '+', '"','|', '\\', '/', '/'))
                .TrimEnd(' ').TrimEnd('.');

            string fileName = $"Json-{folderName}-{DateTime.Now.Date.ToShortDateString().Replace('/', '_')}.json";
            string filePath = path + fileName;
           
            try
            {
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, json);
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return filePath;

        }
    }
}