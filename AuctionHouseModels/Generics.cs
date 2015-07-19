using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace AuctionHouseModels
{
    [DataContract]
    public class ResultCode
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string ContextID { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    public class Helper
    {
        /// <summary>
        /// Serialize object in json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            if (obj == null) return String.Empty;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                string retVal = Encoding.UTF8.GetString(ms.ToArray());
                return retVal;
            }
        }

        /// <summary>
        /// Deserialize json string in obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            if (String.IsNullOrEmpty(json)) return default(T);

            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
                return obj;
            }
        }
    }
}
