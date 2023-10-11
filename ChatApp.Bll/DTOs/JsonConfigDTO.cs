namespace ChatApp.Bll.DTOs
{
    public class JsonConfigDTO
    {
        public string BrowserOption { get; set; }
        public bool HeadlessOption { get; set; }
        public IEnumerable<string> OnlinerLinks { get; set; }
        public string OnlinerDate { get; set; }
        public IEnumerable<string> BeltaLinks { get; set; }
        public string BeltaDate { get; set; }
    }
}
