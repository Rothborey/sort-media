using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SortMediaFiles.Constants;
using SortMediaFiles.Models;

namespace SortMediaFiles.Helpers
{
    public static class FileProcessingHelper
    {      
        public static List<FileDetails> GetFileDetailsByType(string srcFolder, string[] typeFilter)
        {         
            var files = GetFilesFrom(srcFolder, typeFilter, true);
            return GetFileDetailsFrom(files);   
        }
        
        public static void ProcessDirectory(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                ProcessDirectory(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        public static void MoveFiles(List<FileDetails> files, string destPath)
        {                  
            var progressCount = 0;
            var total = files.Count;

            using (var progress = new ProgressBar())
            {
                foreach (var file in files)
                {
                    var newFolderPath = destPath + "/" + file.NewFolderName;
                    var newFilePath = newFolderPath + "/" + file.Name;

                    //allows for duplicate filenames by appending _ to the beginning
                    var altFilePath = newFolderPath + "/_" + file.Name;
                    var alt2FilePath = newFolderPath + "/__" + file.Name;

                    CreateFolderLocation(newFolderPath);

                    if (!File.Exists(newFilePath))
                        File.Move(file.FullPath, newFilePath);
                    else if (!File.Exists(altFilePath))
                        File.Move(file.FullPath, altFilePath);
                    else if (!File.Exists(alt2FilePath))
                        File.Move(file.FullPath, alt2FilePath);
                    else
						LogHelper.LogMessageToFile("File already exists: " + newFilePath, Common.NewFolderPath);
                    
                    progress.Report((double)progressCount / total);
                    Thread.Sleep(20);               
                    progressCount++;
                }
            }
        }

        public static void CreateDestFolders()
        {
            CreateFolderLocation(Common.NewFolderPath);
			CreateFolderLocation(Common.ImageDestPath);
			CreateFolderLocation(Common.VideoDestPath);
        }

		static void CreateFolderLocation(string newFolderLocation)
        {
            if (!Directory.Exists(newFolderLocation))
                Directory.CreateDirectory(newFolderLocation);
        }

        static List<string> GetFilesFrom(string searchFolder, string[] filters, bool isRecursive)
        {
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            var files = Directory
                .GetFiles(searchFolder, "*", searchOption)
                .Where(file => filters.Any(file.ToLower().EndsWith))        //ignore casing
                .ToList();
            
            return files;         
        }
      
        static List<FileDetails> GetFileDetailsFrom(List<string> files)
        {
            var fileDetails = new List<FileDetails>();
            foreach(var file in files)
            {
                fileDetails.Add(new FileDetails
                {
                    Name = Path.GetFileName(file),
                    FullPath = file,
                    DateCreated = File.GetCreationTime(file),
                    DateModified = File.GetLastWriteTime(file),
                    NewFolderName = GetNewFolderName(File.GetCreationTime(file), File.GetLastWriteTime(file))
                });
            }

            return fileDetails;
        }
        
        static string GetNewFolderName(DateTime? dateCreated, DateTime? dateModified)
        {
            var dateFormat = "yyyy MMMM";
            var folderName = "Date Unspecified";

            if (dateModified.HasValue && dateCreated.HasValue)
            {
                if (dateModified.Value < dateCreated.Value)
                    folderName = dateModified.Value.ToString(dateFormat);
                else
                    folderName = dateCreated.Value.ToString(dateFormat);
            }
            else if (dateModified.HasValue)
            {
                folderName = dateModified.Value.ToString(dateFormat);
            }
            else if (dateCreated.HasValue)
            {
                folderName = dateCreated.Value.ToString(dateFormat);
            }

            return folderName;         
        }      
    }
}
