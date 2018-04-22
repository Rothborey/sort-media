using System;
namespace SortMediaFiles.Models
{
    public class FileDetails
    {
		public String Name { get; set; }
		public String FullPath { get; set; }      
		public DateTime? DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public String NewFolderName { get; set; }
    }
}
