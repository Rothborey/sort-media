using System;
using SortMediaFiles.Constants;
using SortMediaFiles.Helpers;

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
        static void Main(string[] args)
        {
			try
			{
				FileProcessingHelper.CreateDestFolders();

				//move image files
				var imageFileDetails = FileProcessingHelper.GetFileDetailsByType(
					Common.SearchFolder, 
					Common.ImageFilters);
				FileProcessingHelper.MoveFiles(imageFileDetails, Common.ImageDestPath);

                //move video files
				var videoFileDetails = FileProcessingHelper.GetFileDetailsByType(
					Common.SearchFolder, 
					Common.VideoFilters);
				FileProcessingHelper.MoveFiles(videoFileDetails, Common.VideoDestPath);
                
                //Clean up empty folders
				FileProcessingHelper.ProcessDirectory(Common.SearchFolder);
			}  
			catch (Exception e)
			{
				LogHelper.LogMessageToFile(e.ToString(), Common.NewFolderPath);
			}
        }
    }
}
