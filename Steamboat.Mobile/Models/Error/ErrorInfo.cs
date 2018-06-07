using System;
namespace Steamboat.Mobile.Models.Error
{
    public class ErrorInfo
    {
        public Error Error { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public int LogEntryID { get; set; }
    }
}
