using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    [DataContract]
    public abstract class Payload
    {
        public string GetJsonString()
        {
            /*
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(this.GetType(), new DataContractJsonSerializerSettings()
            {
                KnownTypes = new Type[] { typeof(string), typeof(int[]), typeof(int) }
            });

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, this);
                return ms.ToArray();
            }
            */

            return Newtonsoft.Json.JsonConvert.SerializeObject(this);

        }

        public virtual HttpContent GetHttpContent()
        {
            string json = GetJsonString();

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
