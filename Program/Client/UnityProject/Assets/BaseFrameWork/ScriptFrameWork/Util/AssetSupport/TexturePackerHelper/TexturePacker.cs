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
// * Filename:TexturePacker.cs
// * Created:2018/4/6
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//
using System;
using UnityEngine;
using System.IO;

public class TexturePacker
{
	Texture2D mainTexture;

	bool useInnerSettingTxt = true;

	private string texturePath;

	public Texture2D GetMainTex ()
	{
		return this.mainTexture;
	}

	public void LoadTexture (string textureName)
	{
		texturePath = "textures/" + textureName;
		byte[] all_bytes = null;
		useInnerSettingTxt = false;
		var realFileName = AssetFileToolUtilManager.Instance.texture.GetRealSettingTxtMd5FileName (textureName+".txt", out useInnerSettingTxt);
		if (useInnerSettingTxt) {
			this.mainTexture = AssetLoaderManager.Instance.LoadResourceBlock (texturePath) as Texture2D;
		} else {
			Debug.LogWarning ("use update file:" + realFileName);
			var fileRead = File.OpenRead (PathTool.TextureSavePath + realFileName);
			all_bytes = new byte[fileRead.Length];
			fileRead.Read (all_bytes, 0, (int)fileRead.Length);
			fileRead.Close ();

			byte[] bytes = new byte[all_bytes.Length - 16];
			byte[] width = new byte[4];
			byte[] height = new byte[4];
			byte[] ex1 = new byte[4];
			byte[] ex2 = new byte[4];
			Array.Copy (all_bytes, 0, width, 0, 4);
			Array.Copy (all_bytes, 4, height, 0, 4);
			Array.Copy (all_bytes, 8, ex1, 0, 4);
			Array.Copy (all_bytes, 12, ex2, 0, 4);
			Array.Copy (all_bytes, 16, bytes, 0, bytes.Length);

			var  width_val = BitConverter.ToInt32(width,0);
			var  height_val = BitConverter.ToInt32(height,0);
			var  ex1_val = BitConverter.ToInt32(ex1,0);
			var  ex2_val = BitConverter.ToInt32(ex2,0);

			this.mainTexture =  UITools.CreateTexture (width_val, height_val, bytes);
		}


	}


	public void Destroy ()
	{
		if (useInnerSettingTxt) {
			this.mainTexture = null;
			AssetLoaderManager.Instance.UnloadResource (this.texturePath);
		} else {
			GameObject.Destroy (this.mainTexture);
		}
	}
}