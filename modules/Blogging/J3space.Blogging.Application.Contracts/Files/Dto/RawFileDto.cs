namespace J3space.Blogging.Files.Dto
{
    public class RawFileDto
    {
        public RawFileDto()
        {
        }

        public byte[] Bytes { get; set; }
        public bool IsFileEmpty => Bytes == null || Bytes.Length == 0;

        public static RawFileDto EmptyResult()
        {
            return new RawFileDto() {Bytes = new byte[0]};
        }
    }
}