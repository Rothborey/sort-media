using System;

namespace SortMediaFiles.Constants
{
	public static class Common
	{
		public const string BaseLocation = @"/Volumes/Seagate Expansion Drive/";
		public const string SearchFolder = BaseLocation + "Apple Photos";
		public const string NewFolderPath = BaseLocation + "Processed Files";
		public const string ImageDestPath = NewFolderPath + "/Images";
		public const string VideoDestPath = NewFolderPath + "/Videos";
		public static readonly string[] ImageFilters = { "jpg", "jpeg", "png", "gif", "tiff", "tif", "psd", "nef", "bmp", "heic", "cr2" };
		public static readonly string[] VideoFilters = { "mp4", "avi", "mov", "mpg", "m4v" };
	}
}
