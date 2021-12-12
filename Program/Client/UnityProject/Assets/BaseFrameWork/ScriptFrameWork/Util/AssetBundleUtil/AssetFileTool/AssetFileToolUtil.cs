using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AssetPlugin
{
	public class AssetFileToolUtil
	{
		/// <summary>
		/// //bag inner txtconfig
		/// </summary>
		/// <value>The inner setting text asset file tool.</value>
		public AssetFileTool innerSettingTxtAssetFileTool{ get; private set; }

		/// <summary>
		/// //remote txtconfig,the version must bigger or eq then innner
		/// </summary>
		/// <value>The remote setting text asset file tool.</value>
		public AssetFileTool remoteSettingTxtAssetFileTool{ get; private set; }

		public bool recordNeedUpdate{ get; private set; }

		public void InitInnerSetting (string md5Path)
		{
			//
			var txt = Resources.Load<TextAsset> (md5Path);
			innerSettingTxtAssetFileTool = AssetFileTool.LoadFromString (txt.text);
		}

		public void SetRemoteSetting (AssetFileTool remote)
		{
			remoteSettingTxtAssetFileTool = remote;
		}

		public void RecordNeedUpdate (bool b)
		{
			this.recordNeedUpdate = b;		
		}

		public string GetRealSettingTxtMd5FileName (string settingTxtName, out bool useInnerSettingTxt)
		{
			AssetCell innerAc = null;
			AssetCell remoteAc = null;
			if (innerSettingTxtAssetFileTool == null) {
				useInnerSettingTxt = true;
				return settingTxtName;
			};
			innerSettingTxtAssetFileTool.assetCells.TryGetValue (settingTxtName, out innerAc);
		
			if (remoteSettingTxtAssetFileTool != null) {
				remoteSettingTxtAssetFileTool.assetCells.TryGetValue (settingTxtName, out remoteAc);
			} else {
				useInnerSettingTxt = true;
				if (innerAc == null) {
					return settingTxtName;
				}
				return innerAc.localFileName;
			}
			if (remoteAc == null) {
				if (innerAc == null) {
					Debug.LogError ("不可能远程和本地都没有本表:" + settingTxtName);
				}
				useInnerSettingTxt = true;
				return innerAc.localFileName;
			}
			if (innerAc == null) {
				useInnerSettingTxt = false;
				return remoteAc.saveFileName;		
			}
			if (innerAc.hashCode != remoteAc.hashCode) {
				if (File.Exists( PathTool.TxtSavePath + remoteAc.saveFileName))
				{
					useInnerSettingTxt = false;
					return remoteAc.saveFileName;
				}
				else
				{
					useInnerSettingTxt = true;
					return innerAc.localFileName;
				}
			}
			useInnerSettingTxt = true;
			return innerAc.localFileName;
		}



	}
}