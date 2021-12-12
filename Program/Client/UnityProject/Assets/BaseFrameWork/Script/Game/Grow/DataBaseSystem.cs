using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using TextEquip.System;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.Grow
{
    public class DataBaseSystem
    {

        public static void ReStart()
        {
            try
            {
                // File.Delete(PathTool.DataSavePath + "cur.record");
                PlayerPrefs.SetString("cur.record", "");
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
        
        
        public static bool ImportData(string record, out GrowData growData)
        {
            growData = null;
            try
            {
                var data = UnzipTextFromBase64String(record);
#if UNITY_EDITOR
                Debug.LogWarning(data);
#endif
                growData = JsonConvert.DeserializeObject<GrowData>(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                return false;
            }
            
            return true;
        }
        
        public static bool LoadData(out GrowData growData)
        {
            growData = null;
            string fileName = "";
            try
            {
                FileUtils.CreateDir(PathTool.DataSavePath);
                // fileName = File.ReadAllText(PathTool.DataSavePath + "cur.record");
                fileName = PlayerPrefs.GetString("cur.record", "");
                if (string.IsNullOrEmpty(PathTool.DataSavePath+fileName))
                {
                    Debug.LogError("存档文件为空");
                    return false;
                }
                // var data = File.ReadAllText(fileName);
                var data = UnzipTextFromBase64File(PathTool.DataSavePath+fileName);
                #if UNITY_EDITOR
                Debug.LogWarning(data);
                #endif
                growData = JsonConvert.DeserializeObject<GrowData>(data);
            }catch (Exception e)
            {
                Debug.LogError(e.ToString());
                return false;
            }

            return true;
        }


        public static string ExportData(GrowData growData)
        {
            string data = JsonConvert.SerializeObject(growData);
            return ZipTextToBase64String(data, "");
        }

        public static bool SaveData(GrowData growData)
        {
            try
            {
                FileUtils.CreateDir(PathTool.DataSavePath);
                TestSerializeObject();
                var data = JsonConvert.SerializeObject(growData);
                #if UNITY_EDITOR
                Debug.LogWarning(data);
                #endif
                var fileName =  DateTime.Now.ToString("yyyy-M-d-hh") + ".record";
                // File.WriteAllText(fileName,data);
                ZipTextToBase64String(data, PathTool.DataSavePath +fileName);
                PlayerPrefs.SetString("cur.record", fileName);
                // File.WriteAllText(PathTool.DataSavePath + "cur.record", fileName);
            }catch (Exception e)
            {
                Debug.LogError(e.ToString());
                return false;
            }
            return true;
        }

        private static void TestSerializeObject()
        {
            AbilityItem item = new AbilityItem();;
            var data = JsonConvert.SerializeObject(item);
            Debug.LogWarning(data);
        }


        public static long GetUniqueId()
        {
            var guid  = UnityEngine.PlayerPrefs.GetInt("DataBaseSystem.GetUniqueId", 9527);
            guid++;
            UnityEngine.PlayerPrefs.SetInt("DataBaseSystem.GetUniqueId", guid);
            return guid;
        }


        /// <summary>
        /// 压缩字符串为Base64字符串。
        /// </summary>
        public static string ZipTextToBase64String(string text,string filePath)
        {
            byte[] b = Encoding.UTF8.GetBytes(text);
            string base64Str = "";
            using (MemoryStream to = new MemoryStream())
            using (ZipOutputStream zip = new ZipOutputStream(to))
            {
                ZipEntry entry = new ZipEntry("ToBase64String");
                zip.PutNextEntry(entry);

                zip.Write(b, 0, b.Length);
                zip.CloseEntry(); // 必须，否则报不可预料的压缩文件末端
                zip.Finish(); // 必须，否则报这个压缩文件格式未知或者数据已经被损坏
                base64Str = Convert.ToBase64String(to.ToArray());
                if (!string.IsNullOrEmpty(filePath))
                {
                    File.WriteAllText(filePath, base64Str);
                }
            }

            return base64Str;
        }


        /// <summary>
        /// 解压缩Base64字符串到原字符串。
        /// </summary>
        public static string UnzipTextFromBase64File(string filePath)
        {
            var str = File.ReadAllText(filePath);
            return UnzipTextFromBase64String(str);
        }

        private static string UnzipTextFromBase64String(string str)
        {
            byte[] b = Convert.FromBase64String(str);
            using (MemoryStream from = new MemoryStream(b))
            using (ZipInputStream zip = new ZipInputStream(from))
            using (MemoryStream to = new MemoryStream())
            {
                ZipEntry entry = zip.GetNextEntry();
                byte[] buffer = new byte[1024];
                int len = 0;
                while ((len = zip.Read(buffer, 0, buffer.Length)) > 0)
                {
                    to.Write(buffer, 0, len);
                }
                b = to.ToArray();
                return Encoding.UTF8.GetString(b);
            }
        }
    }
}