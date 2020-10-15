namespace ViewModels
{
    public class Attachment: Entity
    {
        public long BookId { get; set; }

        public string FileS3Key { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }
    }
}
