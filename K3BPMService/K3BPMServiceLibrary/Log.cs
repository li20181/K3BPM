using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K3BPMServiceLibrary
{
    public class Log
    {
        private string BuildLogString(string sLog)
        {
            return string.Format("[{0}]: {1}", DateTime.Now.ToString(), sLog);
        }
        public void WriteToTextBox(ref RichTextBox richTextBox, string sLog)
        {
            try
            {
                richTextBox.Text = string.Format("{0}\r\n{1}", BuildLogString(sLog), richTextBox.Text);
            }
            catch (Exception ex)
            {
                richTextBox.Text += BuildLogString(ex.Message);
            }
        }
    }
}
