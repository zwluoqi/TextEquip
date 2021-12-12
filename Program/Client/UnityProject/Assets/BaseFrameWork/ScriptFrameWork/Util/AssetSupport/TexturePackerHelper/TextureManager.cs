// /*
//                #########
//               ############
//               #############
//              ##  ###########
//             ###  ###### #####
//             ### #######   ####
//            ###  ########## ####
//           ####  ########### ####
//          ####   ###########  #####
//         #####   ### ########   #####
//        #####   ###   ########   ######
//       ######   ###  ###########   ######
//      ######   #### ##############  ######
//     #######  #####################  ######
//     #######  ######################  ######
//    #######  ###### #################  ######
//    #######  ###### ###### #########   ######
//    #######    ##  ######   ######     ######
//    #######        ######    #####     #####
//     ######        #####     #####     ####
//      #####        ####      #####     ###
//       #####       ###        ###      #
//         ###       ###        ###
//          ##       ###        ###
// __________#_______####_______####______________
//
//                 我们的未来没有BUG
// * ==============================================================================
// * Filename:TextureManager.cs
// * Created:2018/4/4
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using XZXD;

public class TexturePicPath{
	public const string HeroPic = "HeroPic/";
	public const string GirlPic = "GirlPic/";
	public const string MapPic = "ChapterScene/";
}

public class TextureManager
{

	private static TextureManager _instance;

	public static TextureManager Instance{
		get{
			if (_instance == null) {
				_instance = new TextureManager ();
			}
			return _instance;
		}
	}


	public Dictionary<string,TexturePacker> textureDict = new Dictionary<string, TexturePacker>();


	public Texture2D GetMainTex(string textureName){
		// if (Main.simpleIconStyle) {
		// 	return GetMainTex0 ("SimpleStyle/0");
		// } 
		// else 
		{
			var s = GetMainTex0 (textureName);
			if (s == null) {
				return GetMainTex0 ("SimpleStyle/0");
			} else {
				return s;
			}
		}
	}

	Texture2D GetMainTex0 (string textureName)
	{
		if (!textureDict.ContainsKey (textureName)) {
			textureDict [textureName] = new TexturePacker ();
			textureDict [textureName].LoadTexture (textureName);
		}
		return textureDict [textureName].GetMainTex ();
	}



	public void Release ()
	{
		foreach (var kv in textureDict) {
			kv.Value.Destroy();
		}
		textureDict.Clear ();
	}
}

