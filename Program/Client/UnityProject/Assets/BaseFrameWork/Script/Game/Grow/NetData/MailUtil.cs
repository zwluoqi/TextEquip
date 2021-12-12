using System.Collections.Generic;
using NetWork.Layer;
using Newtonsoft.Json.Linq;

namespace Script.Game.Grow.NetData
{
    public class MailUtil
    {
        // public static JArray mails;
        public static Dictionary<long, JToken> kvmails = new Dictionary<long, JToken>();
        public static void RequestMail()
        {
            NetManager.Instance.SendHttp("cs_get_mails", "", delegate(Packet data, bool success)
            {
                if (success)
                {
                    // var   = JsonConvert.DeserializeObject(data.kBody);
                    JObject scmsg = JObject.Parse(data.kBody);
                    var mails = (JArray)scmsg["mails"];
                    foreach (var mail in mails)
                    {
                        long id = (long)mail["id"];
                        kvmails.Add(id,mail);
                    }
                    // GrowFun.Instance.growData.growPlayer.remote_id = (long)player["id"];
                    NotificationCenter.Default.PostNotification((int) GameMessageId.SCMailList);
                }
            });
        }
        
        public static void DeleteMail(long guid)
        {
            JArray jArray = new JArray();
            jArray.Add(guid);
            JObject jObject = new JObject();
            jObject.Add("mail_guids",jArray);
            
            NetManager.Instance.SendHttp("cs_delete_mails", jObject.ToString(), delegate(Packet data, bool success)
            {
                if (success)
                {
                    // var   = JsonConvert.DeserializeObject(data.kBody);
                    // JObject scmsg = JObject.Parse(data.kBody);
                    kvmails.Remove(guid);
                    // GrowFun.Instance.growData.growPlayer.remote_id = (long)player["id"];
                    NotificationCenter.Default.PostNotification((int) GameMessageId.SCDeleteMail, guid);
                }
            });
        }
    }
}