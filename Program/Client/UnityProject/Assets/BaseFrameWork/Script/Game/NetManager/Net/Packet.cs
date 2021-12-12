using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json.Linq;
using Object = System.Object;

namespace NetWork.Layer
{
	public class Packet
    {
        public string nOpCode;
        public string kBody;
		public bool background;

       
        public Packet()
        {
            nOpCode = "";
            kBody = "";
        }

        public Packet(string code, string msg)
        {
            nOpCode		= code;
            kBody		= msg;
        }
        
//		public Define.ServerType socketServerType;
//		public Packet(int code, object buff, Define.ServerType type)
//		{
//			nOpCode		= code;
//			kBody		= buff;
//			socketServerType = type;
//		}
    }

    // 解决
//    public class ExtraPacket : Packet
//    {
//        public Packet kExtraPacket;
//
//        public void DoPacket(Packet packet)
//        {
//            nOpCode = packet.nOpCode;
//            kBody = packet.kBody;
//            socketServerType = packet.socketServerType;
//        }
//
//        public void DoExtraPacket(Packet extraPacket)
//        {
//            kExtraPacket = extraPacket;
//        }
//    }
}

