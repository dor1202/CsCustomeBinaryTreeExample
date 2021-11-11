namespace DataModels
{
    public class LogWithStatus
    {
        public string Output { get; set; }
        public UIPrompt Status { get; set; }
        public LogWithStatus(string output, UIPrompt status)
        {
            Output = output;
            Status = status;
        }
    }
}
