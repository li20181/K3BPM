using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace K3BPMServiceLibrary
{
    public class ThingSpeak
    {
        public class Channel
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string field1 { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public int last_entry_id { get; set; }
        }

        public class Feed
        {
            public string created_at { get; set; }
            public int entry_id { get; set; }
            public string field1 { get; set; }
        }

        public class ThingSpeakData
        {
            public Channel channel { get; set; }
            public List<Feed> feeds { get; set; }
        }

        private string GetResponseData(HttpWebResponse response)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            Stream responseStream = response.GetResponseStream();

            int count = 0;

            do
            {
                count = responseStream.Read(buf, 0, buf.Length);
                if (count != 0)
                {
                    sb.Append(Encoding.ASCII.GetString(buf, 0, count));
                }
            }
            while (count > 0);

            return sb.ToString();
        }
        public ThingSpeakData FetchData(string ChannelID, string ReadAPIKey)
        {
            string url = string.Format("http://api.thingspeak.com/channels/{0}/feed.json?key={1}",
            ChannelID, ReadAPIKey);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                        response.StatusCode, response.StatusDescription));
                }

                string JSONString = GetResponseData(response);
                //var data = JsonConvert.DeserializeObject<ThingSpeakData>(JSONString);
                ThingSpeakData data = JsonConvert.DeserializeObject<ThingSpeakData>(JSONString);

                return data;
            }
        }
    }
    
}
