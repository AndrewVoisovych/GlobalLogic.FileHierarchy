using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GlobalLogic.FileHierarchy.Backend.Models;
using Newtonsoft.Json;


namespace GlobalLogic.FileHierarchy.Backend
{
    /// <summary>
    /// Сlass that contains methods for reading the hierarchy of files and folders
    /// </summary>
    public static class HierarchyReader
    {
        /// <summary>
        /// Save the name of the root directory for later use
        /// </summary>
        public static string RootDirectoryName {get; private set;}
        
        /// <summary>
        /// Read the hierarchy of folders and files along the selected path
        /// </summary>
        /// <param name="path">The path to the folder/files</param>
        /// <returns>Json hierarchy</returns>
        /// <exception cref="DirectoryNotFoundException">If the directory is not found</exception>
        public static string Read(string path)
        {
            RootDirectory rootDirectory = new RootDirectory();
            DirectoryInfo currentDirectory = new DirectoryInfo(path);
            RootDirectoryName = currentDirectory.Name;

            if (!currentDirectory.Exists)
            {
                throw new DirectoryNotFoundException("This directory does not exist");
            }

            var hierarchy = ReadingRecursively(currentDirectory, rootDirectory);
            // Formatting.Indented -> formatting for file
            var jsonHierarchy = JsonConvert.SerializeObject(hierarchy, Formatting.Indented);

            return jsonHierarchy;
        }

        private static RootDirectory ReadingRecursively(DirectoryInfo directoryInfo, RootDirectory rootDirectory)
        {
            rootDirectory.Name = directoryInfo.Name;
            rootDirectory.DateCreated = directoryInfo.CreationTime
                .ToString("dd-MMM-yy h:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
            rootDirectory.Children = new List<RootDirectory>();

            // If there are still folders, pass depth and folder to recursion
            if (directoryInfo.EnumerateDirectories().Any())
            {   
                foreach (var directory in directoryInfo.EnumerateDirectories())
                {
                    try
                    {
                        rootDirectory.Children.Add(
                            // Recursion
                            ReadingRecursively(directory, new RootDirectory())
                            );
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // If access to folders is denied
                        continue;
                    }

                }
            }

            rootDirectory.Files = new List<Files>();
            foreach (var file in directoryInfo.EnumerateFiles())
            {
                try
                {
                    rootDirectory.Files.Add(new Files
                    {
                        Name = file.Name,
                        Size = $"{ file.Length} B",
                        Path = file.FullName
                    });
                }
                catch (UnauthorizedAccessException)
                {
                    //If access to files is denied
                    continue;
                }
            }


            return rootDirectory;
        }

       
    }
}