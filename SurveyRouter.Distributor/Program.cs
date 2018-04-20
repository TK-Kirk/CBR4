using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyRouter.Distributor.Properties;
using SurveyRouter.Logic.Logic;
using SurveyRouter.Logic.Repository.Listrak;

namespace SurveyRouter.Distributor
{
    class Program
    {
        static void Main(string[] args)
        {
            var mgr = new RouterManager();

            mgr.Progress += new ProgressEventHandler(UpdateProgress);

            bool result = mgr.RunUserLoad(Settings.Default.UserLoadDaysInterval);

            try
            {
                mgr.SendEmailToAllRouterContacts(Settings.Default.MaxEmailsSent, Settings.Default.SafetyHours);
            }
            catch (Exception e)
            {
                string errorFilename = $"error-main-{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.txt";
                string msg = $"Current Email: {mgr.CurrentEmail}{Environment.NewLine}{e.Message}";
                Exception inner = e.InnerException;
                while (inner != null)
                {
                    msg += $"{Environment.NewLine}{inner.Message}";
                    inner = inner.InnerException;
                }

                msg += $"{Environment.NewLine}{e.StackTrace}";
                File.AppendAllText(@"Logs\" + errorFilename,msg);
                throw;
            }
        }

        private static void UpdateProgress(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
