using NetWork.Layer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Script.Game.Grow.NetData
{
    public class AccountUtil
    {
        public static void RequestLogin(bool register,string playerName)
        {
            JObject jObject = new JObject();
            jObject["activeCode"] = "0";
            jObject["authCode"] = playerName;
            jObject["authPass"] = playerName;
            jObject["loginType"] = 0;
            jObject["platformUid"] = "platformUid";
            jObject["zoneID"] = GrowFun.Instance.remote_zone_id;

            jObject["deviceModel"] = SystemInfo.deviceModel;
            jObject["deviceName"] = SystemInfo.deviceName;
            jObject["deviceType"] = SystemInfo.deviceType.ToString ();
            jObject["operatingSystemFamily"] = SystemInfo.operatingSystemFamily.ToString ();
            jObject["operatingSystem"] = SystemInfo.operatingSystem;
            jObject["deviceUniqueIdentifier"] = SystemInfo.deviceUniqueIdentifier;
            jObject["sha1"] = "sha1";
            jObject["register"] = register;
            // if (cacheDatas.Count > 0) {
            //     msg.platformUserId = (string)cacheDatas ["uid"];
            //     msg.platformUserName = (string) cacheDatas ["name"];
            //     msg.platformUserData =   (string) cacheDatas ["data"];
            // }
            Login(jObject);
        }
        
        
        static void Login(JObject jObject)
        {
            NetManager.Instance.SendHttp("cs_login", jObject.ToString(), delegate(Packet data, bool success)
            {
                if (success)
                {
                    BoxManager.CreatePopTis("登陆成功!");
                    var  scmsg = JObject.Parse(data.kBody);
                    // JToken scmsg = data.kBody;
                    var player = scmsg["player"];
                    GrowFun.Instance.remote_id = (long)player["id"];
                    Grow.GrowFun.Instance.growData.growPlayer.playerName = (string)jObject["authCode"];
                    GrowFun.Instance.SaveData();
                    NotificationCenter.Default.PostNotification((int) GameMessageId.SCLoginDone,(bool)jObject["register"]);
                }
            });
        }
    }
}