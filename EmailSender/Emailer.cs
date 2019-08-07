using LogWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public class Emailer
    {
        //#TODO: Logging in Setters
        SmtpClient smtpclient;

        //Receipents , separeted list of emails.
        String Emails_To, Emails_Cc, Emails_Bcc;

        //Sender Properties
        String Host, Sender_Email, Sender_Password;
        int Port;

        MailMessage msg;
        public Emailer()
        {

            msg = new MailMessage();

            //Read Data from Config
            LoadDataFromConfig();

            //Intialize
            InitializeEmailer();
        }
        private void LoadDataFromConfig()
        {
            Logger.Instance.LogMethodStart();
            try
            {


                Emails_To = System.Configuration.ConfigurationManager.AppSettings["Emails_To"];
                Emails_Cc = System.Configuration.ConfigurationManager.AppSettings["Emails_Cc"];
                Emails_Bcc = System.Configuration.ConfigurationManager.AppSettings["Emails_Bcc"];

                Host = System.Configuration.ConfigurationManager.AppSettings["Host"];
                Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);

                Sender_Email = System.Configuration.ConfigurationManager.AppSettings["Sender_Email"];
                Sender_Password = System.Configuration.ConfigurationManager.AppSettings["Sender_Password"];

            }
            catch (Exception exp)
            {
                Logger.Instance.Log(Logger.MessageType.ERR, "Cofig File Reading Error: {0}", exp.Message);

            }

            Logger.Instance.LogMethodEnd();
        }
        private void InitializeEmailer()
        {
            try
            {
                //Create Creditials
                NetworkCredential Sender_Credintials = new NetworkCredential(Sender_Email, Sender_Password);

                //InitilizeSMTPclient
                smtpclient = new SmtpClient(Host, Port);
                smtpclient.EnableSsl = true;

                //Bind Smtp client with Credintials
                smtpclient.Credentials = Sender_Credintials;
            }
            catch (Exception exp)
            {
                Logger.Instance.Log(Logger.MessageType.ERR, "SMTP Intialization Error: {0}", exp.Message);

            }


        }




        public void CreateMailMessage(String Subject, String Body, List<Attachment> attachments = null)
        {
            try
            {
                msg.To.Add(Emails_To);
                msg.CC.Add(Emails_Cc);
                msg.Bcc.Add(Emails_Bcc);
                if (attachments != null)
                {
                    //Attach Attachments
                }

                msg.From = new MailAddress(Sender_Email);
                msg.Subject = Subject;
                msg.Body = Body;
            }
            catch (Exception exp)
            {
                Logger.Instance.Log(Logger.MessageType.ERR, "Message Creation Error: {0}", exp.Message);

            }
        }

        public void SendMessage()
        {

            //Send Message
            try
            {
                smtpclient.Send(msg);
                Logger.Instance.Log(Logger.MessageType.INF, "Email Delivered: {0}", msg.ToString());
            }
            catch (Exception exp)
            {
                Logger.Instance.Log(Logger.MessageType.ERR, "Message Sending Error: {0}", exp.Message);

            }

        }

    }
}
