namespace J3space.Blogging
{
    public static class BloggingDbProperties
    {
        public const string ConnectionStringName = "Blogging";
        public static string DbTablePrefix { get; set; } = "Blg";

        public static string DbSchema { get; set; } = null;
    }
}