namespace DTO
{
    public class AttachmentDTO: BaseDTO
    {
        public long BookId { get; set; }

        public string FileS3Key { get; set; }

        public string FileName { get; set; }
    }
}
