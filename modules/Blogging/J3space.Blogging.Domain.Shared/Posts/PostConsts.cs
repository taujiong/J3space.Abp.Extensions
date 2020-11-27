namespace J3space.Blogging.Posts
{
    public static class PostConsts
    {
        /// <summary>
        /// Default value: 512
        /// </summary>
        public static int MaxTitleLength { get; set; } = 512;

        /// <summary>
        /// Default value: 1024 * 1024
        /// </summary>
        public static int MaxContentLength { get; set; } = 1024 * 1024; //1MB

        /// <summary>
        /// Default value: 1000
        /// </summary>
        public static int MaxDescriptionLength { get; set; } = 1000;
    }
}