namespace EncompassLoadTest.DataInitialization
{
    public struct AttachmentData
    {
        public string DocumentId { get; set; }
        public string Title { get; set; }
        public string FileNameWithExtension { get; set; }
        public byte[] Content { get; set; }
    }
}