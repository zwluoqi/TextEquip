using System;

using UnityEngine;

public sealed class PathTool
{
	

	public static string FixedPath(string sourcePath){


		while (sourcePath.IndexOf ('\\') >= 0) {
			sourcePath = sourcePath.Replace ('\\', '/');
		}
		while (sourcePath.IndexOf ("//") >=0) {
			sourcePath = sourcePath.Replace ("//", "/");
		}
		return sourcePath;

	}

	public static string DataSavePath = Application.persistentDataPath+ "/Data/";

	public static string TxtSavePath = Application.persistentDataPath+ "/Txt/";
	public static string TxtTmpSavePath = Application.persistentDataPath+ "/TmpTxt/";

	public static string TextureSavePath = Application.persistentDataPath+ "/Texture/";
	public static string TextureTmpSavePath = Application.persistentDataPath+ "/TmpTexture/";

	public static string AtlasSavePath = Application.persistentDataPath+ "/Atlas/";
	public static string AtlasTmpSavePath = Application.persistentDataPath+ "/TmpAtlas/";

    public static string DllsSavePath = Application.persistentDataPath + "/Dlls/";


}


