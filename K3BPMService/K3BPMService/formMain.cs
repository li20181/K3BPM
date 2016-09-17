using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K3BPMServiceLibrary;

namespace K3BPMService
{
    public partial class formMain : Form
    {
        Mail objMail = new Mail();
        ThingSpeak objThingSpeak = new ThingSpeak();
        Log objLog = new Log();
        Config objConfig = new Config();

        string HtmlMailBody = "";
        public formMain()
        {
            InitializeComponent();
        }

        private void Init()
        {
            txtEmail.Text = "thequy@gmail.com";
            txtInterval.Text = "1000";

            //Read settings
            Dictionary<string, string> settings = objConfig.ReadAllSettings();
            if (settings.Any())
            {
                txtChannelID.Text = settings["ChannelID"];
                txtReadAPIKey.Text = settings["ReadAPIKey"];

                objMail.SMTPServer = settings["SMTPServer"];
                objMail.Port = Convert.ToInt32(settings["Port"]);
                objMail.EnableSSL = Convert.ToBoolean(settings["EnableSSL"]);
                objMail.UserName = settings["Sender"];
                objMail.Password = settings["Password"];
                objMail.FromAddress = settings["Sender"];
                objMail.ToAddress = txtEmail.Text;
                btnSendMail.Enabled = true;
            }
            else
            {
                btnSendMail.Enabled = false;
            }
            
        }
        private void formMain_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void btnFetchData_Click(object sender, EventArgs e)
        {
            try
            {
                //Fetch data
                objLog.WriteToTextBox(ref txtLog, "Get data from ThingSpeak...");
                ThingSpeak.ThingSpeakData objData = objThingSpeak.FetchData(txtChannelID.Text, txtReadAPIKey.Text);
                HtmlMailBody = string.Format("<p><strong>[Channel Information]:</strong></p>"
                    + "<p>ID: {0}; Name: {1}; Description: {2}; CreateAt: {3}; UpdateAt: {4}; LastEntryID: {5}</p>",
                    objData.channel.id, objData.channel.name, objData.channel.description,
                    objData.channel.created_at, objData.channel.updated_at, objData.channel.last_entry_id);
                foreach(var item in objData.feeds)
                {
                    HtmlMailBody += string.Format("<p>EntryID: {0}; CreateAt: {1}; Field1: {2}</p>", 
                        item.entry_id, item.created_at, item.field1);
                }

                //Write log
                string sLog = string.Format("[Channel Information][ID: {0};Name:{1};Description:{2};CreateAt:{3};UpdateAt:{4};LastEntryID:{5}]", 
                    objData.channel.id, objData.channel.name, objData.channel.description, 
                    objData.channel.created_at, objData.channel.updated_at, objData.channel.last_entry_id);

                objLog.WriteToTextBox(ref txtLog, sLog);
                objLog.WriteToTextBox(ref txtLog, "Done.");
            }
            catch (Exception ex)
            {
                objLog.WriteToTextBox(ref txtLog, ex.Message);
            }
        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                objLog.WriteToTextBox(ref txtLog, "Sending Email...");

                objMail.Subject = string.Format("[{0}] ThingSpeak Data from Channel {1}",
                    DateTime.Now, txtChannelID.Text);
                objMail.Body = HtmlMailBody;
                objMail.Send();

                objLog.WriteToTextBox(ref txtLog, "Done.");
            }
            catch (Exception ex)
            {
                objLog.WriteToTextBox(ref txtLog, ex.Message);
            }
            
        }
    }
}
