namespace image_categorizer
{
    public interface ILogger
    {
        bool ShowLogFile();
        void WriteLog(string message, bool isError, bool writeNow = false);
    }
}