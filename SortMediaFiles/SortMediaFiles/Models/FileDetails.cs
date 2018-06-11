using System;

namespace SortMediaFiles.Models
{
    public class FileDetails
    {
		public string Name { get; set; }
		public string FullPath { get; set; }      
		public DateTime? DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public string NewFolderName { get; set; }
    }
}
