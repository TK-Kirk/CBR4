using System;

namespace SurveyRouter.Distributor
{
    public  class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs()
        {
            
        }

        public ProgressEventArgs(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
