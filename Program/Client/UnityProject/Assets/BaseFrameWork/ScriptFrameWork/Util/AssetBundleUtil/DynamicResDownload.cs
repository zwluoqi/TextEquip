using System;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
//using NetWork.Layer;
using System.Collections;
using UnityEngine.Networking;
using AssetPlugin;


public class DynamicResDownload:AssetResDownload
{
	protected override bool CheckNeedUpdateAsset(){
		if(remoteAssetFileTool == null){
			Debug.LogError("remote txt config null");
			return false;
		}

		List<AssetCell> unneceList;
		List<AssetCell> lackList;
		List<AssetCell> obbList;
		AssetFileTool.Compare(afToolManager.innerSettingTxtAssetFileTool, remoteAssetFileTool, out unneceList, out lackList, out obbList);
		
		
		List<AssetCell> checkAssetcells = new List<AssetCell> ();
		checkAssetcells.AddRange (lackList);
		checkAssetcells.AddRange (obbList);
		
		//统计需要下载的资源包
		foreach (var item in checkAssetcells)
		{
			AssetCell ac = item;
			bool needMove = false;
			if (CheckIfDownload (ac, ref needMove)) {
				if (!preDownLoadList.Contains (ac)) {
					preDownLoadList.Add (ac);
				}
				XZXDDebug.LogWarning("数据包需要更新的的bundle数据为:" + ac.path+" saveFileName:"+ac.saveFileName);
				
			} else if (needMove) {
				FileUtils.MoveCoverage (localTmpSavePath+"/"+ac.saveFileName,localSavePath+"/"+ac.saveFileName );
			}
		}
		if (preDownLoadList.Count <= 0) {
			XZXDDebug.LogWarning ("不需要更新资源"+this.localSavePath);
		}
		return preDownLoadList.Count > 0;
	}
	
	public List<AssetCell> igonres = new List<AssetCell>();

	
	protected Dictionary<string ,int> fileLoadCount = new Dictionary<string, int>();
	protected override void StartOneModeDownload (AssetResSignalDownloadTool asdt)
	{
		if(!fileLoadCount.ContainsKey(asdt.ac.saveFileName)){
			fileLoadCount[asdt.ac.saveFileName] = 0;
		}
		fileLoadCount[asdt.ac.saveFileName] = fileLoadCount[asdt.ac.saveFileName]+1;
		if (string.IsNullOrEmpty(this.remoteServerPath))
		{
			asdt.StartWWWDownload(this.remoteCdnPath +"/" + asdt.ac.saveFileName);
		}
		else
		{
			if(fileLoadCount[asdt.ac.saveFileName] <=1 && !string.IsNullOrEmpty(remoteCdnPath)){
				asdt.StartWWWDownload(this.remoteCdnPath +"/" + asdt.ac.saveFileName);
			}else{
				asdt.StartWWWDownload(this.remoteServerPath + "?fileName=" + asdt.ac.saveFileName+"&channelId=pc");
			}
		}
	}
	
}
