using UnityEngine;
using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json.Linq;
using Script.Game.Grow;
using UnityEditor;
using Object = System.Object;

public class PacketBundle
{
    public static bool ToMsg(string id, JToken pbwhKBody, out byte[] msg)
    {
        JObject user = new JObject();
        user["id"] = id;
        user["msg"] = pbwhKBody;
        user["pid"] = GrowFun.Instance.remote_id;
        user["zoneid"] = GrowFun.Instance.remote_zone_id;
        msg = System.Text.Encoding.UTF8.GetBytes(user.ToString());
        return true;
    }

    public static bool ToObject(byte[] data, out string id, out string pbData)
    {
        var msg  =  Encoding.UTF8.GetString(data);
        JObject user = JObject.Parse(msg);
        id = (string)user["id"];
        pbData = user["msg"].ToString();
        // pbData = Encoding.UTF8.(data, data.Length);
        return true;
    }
}
