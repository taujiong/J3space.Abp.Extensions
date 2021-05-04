namespace J3space.Blogging.Files.Dto
{
    public class RawFileDto
    {
        public byte[] Bytes { get; init; }
        public bool IsFileEmpty => Bytes == null || Bytes.Length == 0;

        public static RawFileDto EmptyResult()
        {
            return new() {Bytes = System.Array.Empty<byte>()};
        }
    }
}