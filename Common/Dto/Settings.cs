namespace Model
{
    public class Settings
    {
        public static string ImageUrl { get; set; }
        //public Url Url { get; set; }
        public ApiVersion ApiVersion { get; set; }
    }

    public class ApiVersion
    {
        public string Title { get; set; }
        public string Num { get; set; }
        public string Version { get; set; }
    }
    public class Url
    {
        public string ImageUrl { get; set; }
    }
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
    }

    
}
