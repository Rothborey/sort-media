using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SortMediaFiles.Models;

/*
 * This program takes all image and movie files
 * for a given file directory and sorts into 
 * folders based on Created Date or Modified Date  
 * if availalbe
 * Folders will be named yyyy MMMM eg 2018 January
*/

namespace SortMediaFiles
{
    class Program
    {
		public const String _baseLocation = @"/Users/rothborey/Pictures/";
		public const String _searchFolder = _baseLocation + "Europe Trip";
		public const String _newFolderPath = _baseLocation + "Processed Files";
		public static readonly String[] _filters = { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "mp4", "avi", "mov" }; 
      
        static void Main(string[] args)
        {
			try
			{
                var files = GetFilesFrom(_searchFolder, _filters, true);
                var fileDetails = GetFileDetailsFrom(files);         
				MoveFiles(fileDetails);  	
			}  
			catch (Exception e)
			{
				LogHelper.LogMessageToFile(e.ToString());
			}
        }

		private static void MoveFiles(List<FileDetails> files)
		{                  
			var progressCount = 0;
			var total = files.Count;

			using (var progress = new ProgressBar())
			{
				foreach (var file in files)
				{
					var newFolderPath = _newFolderPath + "/" + file.NewFolderName;
					var newFilePath = _newFolderPath + "/" + file.NewFolderName + "/" + file.Name;

					CreateFolderLocation(newFolderPath);
					if (!File.Exists(newFilePath))
					    File.Move(file.FullPath, newFilePath);
                    
					progress.Report((double)progressCount / total);
					Thread.Sleep(20);               
                    progressCount++;
				}
			}
		}
      
		private static void CreateFolderLocation(String newFolderLocation)
		{
			if (!Directory.Exists(newFolderLocation))
				Directory.CreateDirectory(newFolderLocation);
		}
              
		private static List<String> GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
		{
			var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			var files = Directory
				.GetFiles(searchFolder, "*", searchOption)
				.Where(file => filters.Any(file.ToLower().EndsWith))        //ignore casing
                .ToList();

			return files;         
		}
      
        private static List<FileDetails> GetFileDetailsFrom(List<String> files)
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
        
		private static String GetNewFolderName(DateTime? dateCreated, DateTime? dateModified)
		{            
			if (dateCreated.HasValue)
				return dateCreated.Value.ToString("yyyy MMMM");
			else if (dateModified.HasValue)
				return dateModified.Value.ToString("yyyy MMMM");
			else
				return "Date Unspecified";
		}      

    }
}
