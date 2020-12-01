using System;

namespace J3space.Blogging.Files
{
    public class BloggingFileConstant
    {
        /// <summary>
        /// Default value: 5242880
        /// </summary>
        public static int MaxFileSize { get; set; } = 5 * 1024 * 1024; //5MB

        public static int MaxFileSizeAsMegabytes => Convert.ToInt32((MaxFileSize / 1024f) / 1024f);
    }
}