using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using AssetPlugin;

public class DictFileReader
{


	public delegate void LoadFinishedCallBack (DictFileReader fileReader);

	private TextReader m_reader;
	private MemoryStream m_memoryStream = null;
	private readonly char[] m_splitChars = { '\t' };

	private LoadFinishedCallBack m_loadFinishedCallBack;

	public RandomUtil randomUtil;

	public const bool LOAD_DYNAMIC = true;
	public static int lala = 46;

	public DictFileReader (string filePath, LoadFinishedCallBack callBack)
	{
		m_loadFinishedCallBack = callBack;
		this.randomUtil = new RandomUtil ();
		this.randomUtil.SetSeed (this.GetHashCode ());
		//
        
		byte[] bytes = null;
		//编辑器扩展工具用Resources.Load；运行时用AssetLoaderManager

		if (Application.isPlaying && LOAD_DYNAMIC) {
			bool useInnerSettingTxt = false;
			string fileName = filePath.Substring (filePath.LastIndexOf ('/') + 1);
			string dir = filePath.Substring (0, filePath.LastIndexOf ('/') + 1);
			var realFileName = AssetFileToolUtilManager.Instance.txt.GetRealSettingTxtMd5FileName (fileName, out useInnerSettingTxt);
			if (useInnerSettingTxt) {
//				XZXDDebug.LogWarning ("use local file:" + fileName);
				bytes = ResTools.ReadBytes (dir+realFileName);
			} else {
				XZXDDebug.LogWarning ("use update file:" + fileName + " save:" + realFileName);
				var fileRead = File.OpenRead (PathTool.TxtSavePath + realFileName);
				bytes = new byte[fileRead.Length];
				fileRead.Read (bytes, 0, (int)fileRead.Length);
				fileRead.Close ();
			}
		}

//		XZXDDebug.LogWarning ("load done");
//		if (bytes == null) {
////			Debug.LogError ("load inner");
//			bytes = ResTools.ReadBytes (filePath);
//		}

		var decrys = new byte[bytes.Length - 16];
		for (int i = 16; i < bytes.Length; i++) {
			var s = bytes [i] ^ lala;
			decrys [i-16] = Convert.ToByte(s);
		}


//		if(filePath.Contains("shuangxiu")){
//			string str = System.Text.Encoding.Default.GetString ( decrys );
//			Debug.LogWarning (str);
//		}


		m_memoryStream = new MemoryStream (decrys);

		m_reader = new StreamReader (m_memoryStream);


		string[] typeNames = this.ReadRow();
		typeName2Index = new Dictionary<string, int> ();
		for(int i=0;i<typeNames.Length;i++) {
			typeName2Index [typeNames[i]] = i;
		}

		//
		if (m_loadFinishedCallBack != null) {
			m_loadFinishedCallBack (this);
		}
	}
	public Dictionary<string,int> typeName2Index = new Dictionary<string, int> ();

	public string[] ReadRow ()
	{
		string line = m_reader.ReadLine ();
		if (line == null) {
			return null;
		}
		return line.Split (m_splitChars, System.StringSplitOptions.None);
	}

	public string[] ReadLine (int i)
	{
		List<string> lines = new List<string> ();
		string line = null;
		do {
			line = m_reader.ReadLine ();
			if (line != null) {
				string[] rowLines = line.Split (m_splitChars, System.StringSplitOptions.None);
				lines.Add (rowLines [i]);
			}
		} while (line != null);
		return lines.ToArray ();
	}

	public void Dispose ()
	{
		m_reader.Close ();
		m_reader.Dispose ();
		m_reader = null;
		if (m_memoryStream != null) {
			m_memoryStream.Close ();
			m_memoryStream.Dispose ();
		}
		m_memoryStream = null;
	}
}