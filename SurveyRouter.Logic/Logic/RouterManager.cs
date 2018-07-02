using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SurveyRouter.Distributor;
using SurveyRouter.Logic.Data;
using SurveyRouter.Logic.Data.Models;
using SurveyRouter.Logic.Data.Models.PrecisionSample;
using SurveyRouter.Logic.Data.Models.YourSurveys;
using SurveyRouter.Logic.Repository.Listrak;
using SurveyRouter.Logic.Repository.PrecisionSample;
using SurveyRouter.Logic.Repository.YourSurvey;
using RouterHost = SurveyRouter.Logic.Data.Enums.RouterHost;

namespace SurveyRouter.Logic.Logic
{
    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

    public class RouterManager
    {
        public string CurrentEmail { get; set; }
        public event ProgressEventHandler Progress;
        protected virtual void OnProgress(ProgressEventArgs e)
        {
            Progress?.Invoke(this, e);
        }

        public bool RunUserLoad(int runDaysInterval)
        {
            //insert new users
            GloshareDbContext db = new GloshareDbContext();

            OnProgress(new ProgressEventArgs($"Inserting router contacts for {runDaysInterval} days."));


            int numberRouterContactsInserted = 0;
            List<RouterContactInsertsReturnModel> result = db.ExecuteRouterContactInserts(runDaysInterval, out numberRouterContactsInserted);

            OnProgress(new ProgressEventArgs($"Inserted {numberRouterContactsInserted} router contacts."));

            //get users
            //List<RouterContact> missingPrecisionSampleContacts = db.RouterContacts.Include("RouterContactPrecisionSamples").ToList();

            //OnProgress(new ProgressEventArgs($"Inserting Precision Sample contacts."));

            //int psContacts = 0;
            //foreach (var routerContact in missingPrecisionSampleContacts)
            //{
            //    if (routerContact.RouterContactPrecisionSamples.Any())
            //    {
            //        continue;
            //    }

            //    CreatePrecisionSampleUser(routerContact);
            //    psContacts++;
            //}

            //OnProgress(new ProgressEventArgs($"Inserted {psContacts} precision sample contacts."));


            return true;
        }

        public void SendEmailToAllRouterContacts(int max, int safetyHours)
        {
            DateTime startTime = DateTime.Now;
            List<DateTime> TimeFor10 = new List<DateTime>(){DateTime.Now};
            var errors = new List<string>();
            string errorFilename = $"error-{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.txt";

            //create it here and pass so the auth headers are only created once.
            var listrak = new ListrakRest();
            var routerContacts = GetRouterContactsForEmailRun(max, safetyHours);

            OnProgress(new ProgressEventArgs($"{routerContacts.Count} routercontact records to process."));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int counter = 0;
            try
            {
                foreach (RouterContact routerContact in routerContacts)
                {
                    CurrentEmail = routerContact.Email;
                    if (counter > 0 && counter % 10 == 0)
                    {
                        TimeFor10.Add(DateTime.Now);
                        OnProgress(new ProgressEventArgs($"{counter} records processed."));
                    }
                    counter++;
                    //Check safety hours. This prevents emails from being sent out too 
                    //close to when the last one was set. Initial setting is 12 hours.
                    if (routerContact.DailySurveyEmailSentDate.HasValue)
                    {
                        var debug = routerContact.Email;
                        if (routerContact.DailySurveyEmailSentDate.Value.AddHours(safetyHours) > DateTime.Now)
                        {
                            OnProgress(new ProgressEventArgs($"Skipping {routerContact.Email}"));

                            continue;
                        }
                    }

                    bool error = false;
                    string result = SendSurveyEmail(routerContact, out error, listrak);
                    if (!error)
                    {
                        //var db2 = new GloshareDbContext();
                        routerContact.DailySurveyEmailSentDate = DateTime.Now;
                        OnProgress(new ProgressEventArgs($"Sent {routerContact.Email}."));
                    }
                    if (error)
                    {
                        if (result.ToLower().Contains("unsubscribe") || result.ToLower().Contains("suppressed"))
                        {
                            routerContact.Removed = true;
                        }
                        else
                        {
                            routerContact.ErrorOut = true;
                            var rc = new RouterContact()
                            {
                                Email = routerContact.Email,
                                InsertDate = routerContact.InsertDate,
                                RouterContactId = routerContact.RouterContactId,
                                UniqueId = routerContact.UniqueId
                            };
                            string err = ($"{result}{Environment.NewLine}{JsonConvert.SerializeObject(rc)}{Environment.NewLine}{new string('=',50)}{Environment.NewLine}");
                            File.AppendAllText(@"Logs\" + errorFilename,err);
                            OnProgress(new ProgressEventArgs($"Error: {err}"));
                        }
                    }

                    SetStatusForRouterContact(routerContact);

                    if (stopwatch.Elapsed.TotalMinutes > 58)
                    {
                        OnProgress(new ProgressEventArgs($"Exit early due to running for {stopwatch.Elapsed.TotalMinutes} minutes."));
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                WriteRunStats(startTime, TimeFor10, routerContacts.Count, e);
                throw;
            }

            WriteRunStats(startTime, TimeFor10, routerContacts.Count, null);

        }

        private void WriteRunStats(DateTime startTime, List<DateTime> timeFor10, int routerContactsCount, Exception exception)
        {
            DateTime endTime = DateTime.Now;
            TimeSpan totalElaspsedTime = endTime.Subtract(startTime);
           

            string statsFilename = $"stats-{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.txt";

            string content = $"Stats.{Environment.NewLine}";
            content += $"Start: {startTime}.{Environment.NewLine}";
            content += $"End {endTime}.{Environment.NewLine}";
            content += $"Total Records: {routerContactsCount}{Environment.NewLine}";
            content += $"Total Time: {totalElaspsedTime:hh\\:mm\\:ss}{Environment.NewLine}";
            content += $"Total Seconds: {totalElaspsedTime.TotalSeconds}{Environment.NewLine}";
            content +=
                $"Average: {totalElaspsedTime.TotalSeconds / routerContactsCount} seconds per contact.{Environment.NewLine}";
            content += Environment.NewLine;
            content += $"Time for 10{Environment.NewLine}";
            for (int i = 1; i < timeFor10.Count; i++)
            {
                var span = timeFor10[i].Subtract(timeFor10[i - 1]);
                content += $"{i * 10}. Minutes: {span:mm\\:ss}{Environment.NewLine}";
            }

            if (exception != null)
            {
                content += $"Ended on Exception: {exception.Message}";
                content += $"Current Email: {CurrentEmail}";
            }

            File.WriteAllText(@"Logs\" + statsFilename,content);

        }

        private void SetStatusForRouterContact(RouterContact routerContact)
        {
            var db = new GloshareDbContext();
            var rcToUpdate = db.RouterContacts.Find(routerContact.RouterContactId);
            rcToUpdate.DailySurveyEmailSentDate = routerContact.DailySurveyEmailSentDate;
            rcToUpdate.Removed = routerContact.Removed;
            rcToUpdate.ErrorOut = routerContact.ErrorOut;
            db.SaveChanges();
        }

        private static List<RouterContact> GetRouterContactsForEmailRun(int max, int safetyHours)
        {
            safetyHours = safetyHours * -1;
            DateTime safetyTime = DateTime.Now.AddHours(safetyHours);
            var db = new GloshareDbContext();
            var routerContacts = db.RouterContacts
                .Where(r => r.Removed == null && r.ErrorOut == null &&
                            (r.DailySurveyEmailSentDate == null || r.DailySurveyEmailSentDate < safetyTime))
                .OrderByDescending(r => r.RouterContactId)
                .Take(max).ToList();
            return routerContacts;
        }

        public Guid?  CreatePrecisionSampleUser(string emailadress)
        {
            GloshareDbContext db = new GloshareDbContext();
            var contact = db.RouterContacts.FirstOrDefault(r => r.Email == emailadress);
            if (contact is null)
            {
                throw new ObjectNotFoundException(emailadress);
            }

            return CreatePrecisionSampleUser(contact);
            
        }

        public  Guid? CreatePrecisionSampleUser(RouterContact routerContact)
        {
            GloshareDbContext db = new GloshareDbContext();

            var existingPS =
                db.RouterContactPrecisionSamples.FirstOrDefault(p =>
                    p.RouterContactId == routerContact.RouterContactId);
            if (existingPS != null)
            {
                return existingPS.UserGuid;
            }

            var ps = new PrecisionSample();

            var leads = db.OptInLeads.Where(o => o.EmailAddress == routerContact.Email).ToList();
            FormUrlEncodedContent content = null;
            if (leads.Any())
            {
                OptInLead lead = leads.First();
                if (leads.Count == 2)
                { lead = MergeLeads(leads);}
                content = ps.CreatePostContent(routerContact, lead);
            }

            //use the mobile lead over the opt in lead if both emails exist
            var mobileleads = db.MobilelLeads.Where(m => m.EmailAddress == routerContact.Email);
            if (mobileleads.Any())
            {
                content = ps.CreatePostContent(routerContact, mobileleads.First());
            }

            bool error = false;
            string userguid = ps.CreateUser(routerContact, content, out error);

            if (error)
            {
                OnProgress(new ProgressEventArgs(new string('=',50)));
                OnProgress(new ProgressEventArgs($"Failed Creating Precision Sample User:{routerContact.Email}{Environment.NewLine}"));
                //contains raw message on fail
                OnProgress(new ProgressEventArgs($"{userguid}"));
                return null;
            }
            RouterContactPrecisionSample rcps = new RouterContactPrecisionSample
            {
                UserGuid = Guid.Parse(userguid),
                RouterContactId = routerContact.RouterContactId
            };
            db.RouterContactPrecisionSamples.Add(rcps);
            db.SaveChanges();

            return Guid.Parse(userguid);
        }

        private OptInLead MergeLeads(List<OptInLead> leads)
        {
            //linq not working for first and last here. 
            //only handling situations where there are 2 leads

            OptInLead lead1 = null;
            OptInLead lead2 = null;
            foreach (var lead in leads)
            {
                if (lead1 is null)
                {
                    lead1 = lead;
                    continue;
                }

                lead2 = lead;
                break;
            }

            lead1.Firstname = lead1.Firstname ?? lead2?.Firstname;
            lead1.Lastname = lead1.Lastname ?? lead2?.Lastname;
            lead1.Address = lead1.Address ?? lead2?.Address;
            lead1.BirthdayDay = lead1.BirthdayDay ?? lead2?.BirthdayDay;
            lead1.BirthdayMonth = lead1.BirthdayMonth ?? lead2?.BirthdayMonth;
            lead1.BirthdayYear = lead1.BirthdayYear ?? lead2?.BirthdayYear;
            lead1.City = lead1.City ?? lead2?.City;
            lead1.State = lead1.State ?? lead2?.State;
            lead1.Zip = lead1.Zip ?? lead2?.Zip;

            return lead1;
        }

        public Result LoadYourSurveySurveys(RouterUser user)
        {   //Gets the current surveys for this user
            var db = new GloshareDbContext();

            //LoadPrecisionSampleSurveys();

            YourSurvey ys = new YourSurvey();

            var rawSurveyResult = ys.GetRawSurveys(user).Result;

            if (rawSurveyResult.status == "failure" || rawSurveyResult.status == "error")
            {
                rawSurveyResult.surveys = new Survey[0];
                return rawSurveyResult;
            }

            List<int> existingProjectIDs = db.RouterSurveyYourSurveys.Select(s => s.ProjectId).ToList();
            foreach (Survey survey in rawSurveyResult.surveys)
            {
                if (!existingProjectIDs.Contains((int)survey.project_id))
                {
                    RouterSurveyYourSurvey rsps = new RouterSurveyYourSurvey()
                    {
                        Name = survey.name,
                        ProjectId = Convert.ToInt32(survey.project_id),
                        ConversionRate = Convert.ToDecimal(survey.conversion_rate),
                        Url = survey.entry_link,
                        Cpi = Convert.ToDecimal(survey.cpi),
                        Loi = survey.loi,
                        RemainingCompletes = survey.remaining_completes,
                        StudyType = survey.study_type
                    };
                    db.RouterSurveyYourSurveys.Add(rsps);
                    db.SaveChanges();
                }
            }

            return rawSurveyResult;

        }

        private Surveys  LoadPrecisionSampleSurveys(RouterUser user)
        {
            var db = new GloshareDbContext();
            PrecisionSample ps = new PrecisionSample();

            Surveys rawSurveys = ps.GetRawSurveys(user).Result;

            List<int> existingProjectIDs = db.RouterSurveyPrecisionSamples.Select(s => s.ProjectId).ToList();

            foreach (SurveysSurvey survey in rawSurveys.Survey)
            {
                if (!existingProjectIDs.Contains((int)survey.ProjectId))
                {
                    RouterSurveyPrecisionSample rsps = new RouterSurveyPrecisionSample()
                    {
                        Name = survey.SurveyName,
                        ProjectId = Convert.ToInt32(survey.ProjectId),
                        SurveyLength = survey.SurveyLength,
                        ConversionRate = survey.IR,
                        GrossRevenue = survey.GrossRevenue,
                        RewardValue = survey.RewardValue,
                        TrafficType = survey.SurveyTrafficType,
                        Url = survey.SurveyUrl,
                        VerityCheckRequired = survey.VerityCheckRequired == "Yes"
                    };
                    db.RouterSurveyPrecisionSamples.Add(rsps);
                    db.SaveChanges();
                }
            }

            return rawSurveys;
        }
        
        public RouterReturnContainer GetUserSurveys(Guid userId, string ipAddress)
        {
            //get the common user to send to the survey apis.
            //Since this is coming from the LT Email
            //it's just being made from OIL + RouterContact
            RouterUser user = GetRouterUser(userId);
            user.IpAddress = ipAddress;

            Result yourSurveys = LoadYourSurveySurveys(user);

            RouterReturnContainer returnContainer = new RouterReturnContainer();

            if (yourSurveys.status == "failure")
            {
                returnContainer.Message = yourSurveys.messages[0];
            }
            if (yourSurveys.status == "error")
            {
                returnContainer.Message = string.Join(new string('=',50) + Environment.NewLine, yourSurveys.messages);
            }

            List<RouterReturn> ysReturns = Mapper.Map(yourSurveys, user);

            if (!yourSurveys.surveys.Any())
            {
                return returnContainer;
            }

            //Surveys psSurveys = LoadPrecisionSampleSurveys(user);
            //ysReturns.AddRange(Mapper.Map(psSurveys, user));

            var sorted =  ysReturns.OrderByDescending(y=>y.EarningPotential).ToList();

            returnContainer.RouterReturnList = sorted;
            return returnContainer;
        }

        private RouterUser GetRouterUser(string emailAddress)
        {
            //this is used for creating the router backend for a mobilelead
            //during registration.
            //This is also used for testing or add hoc emails
            //we will create a router contact if we don't have one
            var db = new GloshareDbContext();
            RouterContact user = db.RouterContacts.Include("RouterContactPrecisionSamples").FirstOrDefault(u => u.Email == emailAddress);
            if (user == null)
            {
                user = RouterContactFullSetup(emailAddress);
            }
            else
            {
                if (!user.RouterContactPrecisionSamples.Any())
                {
                    //4/20/2018 MH Drop PrecisionSample
                    //CreatePrecisionSampleUser(user);
                }
            }

            if (MapRouterUserFromOil(user, out var routerUser1)) return routerUser1;

            throw new ArgumentException("Contact not found,", nameof(emailAddress));
        }

        public RouterContact RouterContactFullSetup(string emailAddress)
        {
            GloshareDbContext db = new GloshareDbContext();
            RouterContact user = db.RouterContacts.FirstOrDefault(r => r.Email == emailAddress);
            if (user == null)
            {
                user = new RouterContact()
                {
                    Email = emailAddress
                };
                db.RouterContacts.Add(user);
                db.SaveChanges();
            }

            //4/20/2018 MH Drop PrecisionSample
            //CreatePrecisionSampleUser(user);
            return user;
        }


        private bool MapRouterUserFromOil(RouterContact user, out RouterUser routerUser1)
        {
            routerUser1 = null;
            GloshareDbContext db = new GloshareDbContext();
            var oils = db.VwOptInLeadSurveys.Where(v => v.EmailAddress == user.Email).OrderByDescending(o => o.OptInDate);
            if (oils.Any())
            {
                if (oils.Count()== 1 || oils.Count() > 2)
                {
                    var oil = oils.First();
                    routerUser1 = new RouterUser()
                    {
                        Email = oil.EmailAddress,
                        First = oil.Firstname,
                        Last = oil.Lastname, 
                        Address = oil.Address,
                        City = oil.City,
                        State = oil.State,
                        Zip = oil.Zip,
                        UniqueId = user.UniqueId.ToString(),
                        Gender = GetGender(oil.Gender),
                        DOB = GetBirthDate(oil.BirthdayDay, oil.BirthdayMonth, oil.BirthdayYear),
                        PrecisionSampleUserID = GetPrecisionSampleUserId(user),
                        RouterContactId = user.RouterContactId,
                        IpAddress = oil.Ip
                    };
                    return true;
                }
                //merge the 2
                bool second = false;
                VwOptInLeadSurvey oil1 = oils.First();
                VwOptInLeadSurvey oil2 = null;
                foreach (VwOptInLeadSurvey optInLeadSurvey in oils)
                {
                    if (second)
                    {
                        oil2 = optInLeadSurvey;
                    }

                    second = true;
                }
                //var oil2 = oils.Last();  This doesn't work for some reason so loop thing above
                routerUser1 = new RouterUser()
                {
                    Email = oil1.EmailAddress,
                    First = oil1.Firstname ?? oil2.Firstname,
                    Last = oil1.Lastname ?? oil2.Lastname,
                    Address = oil1.Address ?? oil2.Address,
                    City = oil1.City ?? oil2.City,
                    State = oil1.State ?? oil2.State,
                    Zip = oil1.Zip ?? oil2.Zip,
                    UniqueId = user.UniqueId.ToString(),
                    Gender = GetGender(oil1.Gender ?? oil2.Gender),
                    DOB = GetBirthDate(oil1.BirthdayDay ?? oil2.BirthdayDay, oil1.BirthdayMonth ?? oil2.BirthdayMonth, oil1.BirthdayYear ?? oil2.BirthdayYear),
                    PrecisionSampleUserID = GetPrecisionSampleUserId(user),
                    RouterContactId = user.RouterContactId,
                    IpAddress = oil1.Ip //?? oil2.Ip
                };
                return true;
            }

            return false;
        }
        private bool MapRouterUserFromMobileLead(RouterContact user, out RouterUser routerUser2)
        {
            routerUser2 = null;
            GloshareDbContext db = new GloshareDbContext();
            var ml = db.MobilelLeads.FirstOrDefault(v => v.EmailAddress == user.Email);
            if (ml != null)
            {
                {
                    routerUser2 = new RouterUser()
                    {
                        Email = ml.EmailAddress,
                        First = ml.Firstname,
                        Last = ml.Lastname,
                        Address = ml.Address,
                        City = ml.City,
                        State = ml.State,
                        Zip = ml.Zip,
                        UniqueId = user.UniqueId.ToString(),
                        Gender = ml.Gender,
                        DOB = ml.Dob,
                        PrecisionSampleUserID = GetPrecisionSampleUserId(user),
                        RouterContactId = user.RouterContactId,
                        IpAddress = ml.Ip
                    };
                    return true;
                }
            }

            return false;
        }
        private static Guid? GetPrecisionSampleUserId(RouterContact user)
        {
            if (user.RouterContactPrecisionSamples.Any())
            {
                return user.RouterContactPrecisionSamples.First().UserGuid;
            }

            return null;
        }


        private string GetGender(string rawGender)
        {
            if (string.IsNullOrWhiteSpace(rawGender))
            {
                return null;
            }

            if (rawGender.Length == 1)
            {
                return rawGender;
            }

            return rawGender.Substring(0, 1);
        }

        private static DateTime? GetBirthDate(int? birthdayDay, int? birthdayMonth, int? birthdayYear)
        {
            DateTime returnDate;
            if (DateTime.TryParse($"{birthdayMonth}/{birthdayDay}/{birthdayYear}", out returnDate))
            {
                return returnDate;
            }
            return null;
        }

        public RouterAction SetSurveyAction(int routerContactId, int hostId, int projectId, string surveyUrl, string ipAddress)
        {
            var db = new GloshareDbContext();
            RouterAction ra = null;

            RouterHost host = (RouterHost) hostId;

            if (host == RouterHost.PrecisionSample)
            {
               ra = db.RouterActions.FirstOrDefault(s =>
                    s.RouterContactId == routerContactId && s.RouterHostId == hostId && s.RouterSurveyPrecisionSample.ProjectId == projectId);
            }

            if (host == RouterHost.YourSurvey)
            {
                ra = db.RouterActions.FirstOrDefault(s =>
                    s.RouterContactId == routerContactId && s.RouterHostId == hostId && s.RouterSurveyYourSurvey.ProjectId == projectId);
            }

            if (ra == null)
            {
                int? psID = null;
                int? ysID = null;
                if (host == RouterHost.PrecisionSample)
                {
                    psID = db.RouterSurveyPrecisionSamples.FirstOrDefault(s => s.ProjectId == projectId)?.RouterSurveyPrecisionSampleId;
                }

                if (host == RouterHost.YourSurvey)
                {
                    ysID = db.RouterSurveyYourSurveys.FirstOrDefault(s => s.ProjectId == projectId)?.RouterSurveyYourSurveyId;
                }

                ra = new RouterAction()
                {
                    RouterContactId = routerContactId,
                    Ip = ipAddress,
                    RouterHostId = hostId,
                    RouterSurveyPrecisionSampleId = psID,
                    RouterSurveyYourSurveyId = ysID
                };
                db.RouterActions.Add(ra);
                db.SaveChanges();
            }

            return ra;
        }


        

        public string SendSurveyEmail(string emailAddress, string ipAddress)
        {
            RouterUser user = null;
            try
            {
                user = GetRouterUser(emailAddress);
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            var routerReturnContainer = GetUserSurveys(Guid.Parse(user.UniqueId), ipAddress);

            //var user = mgr.GetRouterUser(Guid.Parse("B1E269BC-99A5-4B96-A095-06A9CD56D446"));

            var l = new ListrakRest();
            string message;
            var result = l.SendDailySurveysEmail(user, routerReturnContainer.RouterReturnList, out message);

            return result ? "Email Sent" : "Email was not sent successfully. " + message;
        }

        public string SendSurveyEmail(RouterContact routerContact, out bool error, ListrakRest listrak)
        {
            error = false;
            RouterUser user = null;
            try
            {
                user = GetRouterUser(routerContact.RouterContactId);
            }
            catch (Exception e)
            {
                error = true;
                return e.ToString();
            }

            //if (user.PrecisionSampleUserID == null)
            //{
            //    error = true;
            //    return $"No precision sample id for {routerContact.Email}";
            //}

            var routerReturnContainer = GetUserSurveys(Guid.Parse(user.UniqueId), user.IpAddress);

            string message = null;
            if (!routerReturnContainer.RouterReturnList.Any())
            {
                error = true;
                message = $"No surveys found.{Environment.NewLine}{routerReturnContainer.Message}";
                return message;
            }

            if (routerReturnContainer.RouterReturnList.Count > 2)
            {
                var result = listrak.SendDailySurveysEmail(user, routerReturnContainer.RouterReturnList, out message);
                if (result)
                {
                    return "Email Sent";
                }
            }
            else
            {
                error = true;
                return "Less than 3 surveys. Email not sent.";
            }



            error = true;
            return "Email was not sent successfully. " + message; 
        }


        public Guid GetRouterUserFromPrecisionSampleId(string ug)
        {
            var db = new GloshareDbContext();
            Guid userGuid = Guid.Parse(ug);
            var user = db.RouterContacts.FirstOrDefault(r => r.RouterContactPrecisionSamples.Any(p => p.UserGuid == userGuid));

            return user.UniqueId;
        }

        public Guid GetRouterContactUnqueId(int routerContactId)
        {
            var db = new GloshareDbContext();
            RouterContact user = db.RouterContacts.FirstOrDefault(u => u.RouterContactId == routerContactId);

            return user.UniqueId;

        }

        public RouterUser GetRouterUser(int routerContactId)
        {
            //used when coming from the mysurveys email
            var db = new GloshareDbContext();
            RouterContact user = db.RouterContacts.Include("RouterContactPrecisionSamples").FirstOrDefault(u => u.RouterContactId == routerContactId);
            if (user != null)
            {
                if (MapRouterUserFromOil(user, out var routerUser1)) return routerUser1;
                if (MapRouterUserFromMobileLead(user, out var routerUser2)) return routerUser2;
            }
            throw new ArgumentException("Contact not found,", nameof(routerContactId));
        }

        public RouterUser GetRouterUser(Guid uniqueId)
        {
            //used when coming from the mysurveys email
            var db = new GloshareDbContext();
            RouterContact user = db.RouterContacts.Include("RouterContactPrecisionSamples").FirstOrDefault(u => u.UniqueId == uniqueId);
            if (user != null)
            {
                if (MapRouterUserFromOil(user, out var routerUser1)) return routerUser1;
                if (MapRouterUserFromMobileLead(user, out var routerUser2)) return routerUser2;
            }
            throw new ArgumentException("Contact not found,", nameof(uniqueId));
        }

        public void SetRouterActionUrl(RouterAction ra)
        {
            var db = new GloshareDbContext();

            var routeraction = db.RouterActions.Find(ra.RouterActionId);

            routeraction.PostedUrl = ra.PostedUrl;

            db.SaveChanges();
        }


        public MinimumInfoExistsResult CheckIfMinimumInfoExistsForEmail(string emailAddress)
        {
            var db = new GloshareDbContext();

            var oilLeads = db.OptInLeads.Where(o => o.EmailAddress == emailAddress).ToList();

            if (!oilLeads.Any())
            {
                return new MinimumInfoExistsResult(){EmailNotFound = true};
            }

            OptInLead lead = oilLeads.First();
            if (oilLeads.Count > 1)
            {
                lead = MergeLeads(oilLeads);
            }

            MinimumInfoExistsResult minimumInfoExistsResult = new MinimumInfoExistsResult()
            {
                NeedsDOB = true,
                NeedsGender = string.IsNullOrWhiteSpace(lead.Gender),
                NeedsZip = string.IsNullOrWhiteSpace(lead.Zip)
            };
            string dobString = $"{lead.BirthdayMonth}/{lead.BirthdayDay}/{lead.BirthdayYear}";
            DateTime dob;
            if (DateTime.TryParse(dobString, out dob))
            {
                minimumInfoExistsResult.NeedsDOB = false;
            }

            minimumInfoExistsResult.HasMinimumInfo = !minimumInfoExistsResult.NeedsDOB &&
                                                     !minimumInfoExistsResult.NeedsZip &&
                                                     !minimumInfoExistsResult.NeedsGender;

            if (minimumInfoExistsResult.HasMinimumInfo)
            {
                var routerContact = db.RouterContacts.FirstOrDefault(r => r.Email == emailAddress);
                if (routerContact != null)
                {
                    minimumInfoExistsResult.HasRounterContact = true;
                    minimumInfoExistsResult.RouterContactUniqueId = routerContact.UniqueId;
                }
            }

            return minimumInfoExistsResult;
        }

        public void UpdateMinimumSurveyInfo(MinimumSurveyInfo minimumSurveyInfo)
        {
            var db = new GloshareDbContext();

            var optInLeads = db.OptInLeads.Where(o => o.EmailAddress == minimumSurveyInfo.EmailAddress);

            foreach (OptInLead lead in optInLeads)
            {
                if (!string.IsNullOrWhiteSpace(minimumSurveyInfo.Gender)) lead.Gender = minimumSurveyInfo.Gender;
                if (minimumSurveyInfo.Dob != null) lead.BirthdayDay = minimumSurveyInfo.Dob.Value.Day;
                if (minimumSurveyInfo.Dob != null) lead.BirthdayMonth = minimumSurveyInfo.Dob.Value.Month;
                if (minimumSurveyInfo.Dob != null) lead.BirthdayYear = minimumSurveyInfo.Dob.Value.Year;
                if (!string.IsNullOrWhiteSpace(minimumSurveyInfo.Zip)) lead.Zip = minimumSurveyInfo.Zip;
            }

            db.SaveChanges();
        }
    }

    public class MinimumInfoExistsResult      
    {
        public Guid RouterContactUniqueId { get; set; }
        public bool NeedsDOB { get; set; }
        public bool NeedsGender { get; set; }
        public bool NeedsZip { get; set; }

        public bool EmailNotFound { get; set; }
        public bool HasMinimumInfo { get; set; }
        public bool HasRounterContact { get; set; }
    }

    public class MinimumSurveyInfo
    {
        public string Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string Zip { get; set; }
        public string EmailAddress { get; set; }

    }
}
