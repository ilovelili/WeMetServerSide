namespace NamecardScanner.Models
{
    public sealed class RecognizeResponse
    {
        public string TaskStatus { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string Phone { get; set; }
        public string Text { get; set; }
    }
}
