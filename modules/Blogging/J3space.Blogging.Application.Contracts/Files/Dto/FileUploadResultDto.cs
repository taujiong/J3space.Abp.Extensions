namespace J3space.Blogging.Files.Dto
{
    public class FileUploadResultDto
    {
        public string RawName { get; set; }
        public string UniqueName { get; init; }
        public string WebUrl { get; init; }
    }
}