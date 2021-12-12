using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using AssetPlugin;

public class AssetResDownload
{
	public static bool ifNeedQuestDownload = true;
	public string localSavePath;
	public string localTmpSavePath;
	public string remoteServerPath;
	public string remoteCdnPath;
	public string assetFileToolName;

	public AssetFileTool remoteAssetFileTool;

	private long totalSize;
	private int totalDownloadCount;
	private long downloadedSize;
	private int downloadedCount;
	protected List<AssetCell> preDownLoadList = new List<AssetCell> ();
	protected List<AssetCell> errorList = new List<AssetCell>();
	protected List<AssetCell> downLoadList = new List<AssetCell>();
	protected List<AssetResSignalDownloadTool> sigDownLoadList = new List<AssetResSignalDownloadTool> ();

	private float speed;

	public static List<AssetResDownload> resDownLoadTools = new List<AssetResDownload>();

	public static void AddPreLoadAssetDownLoadTool(AssetResDownload ardt){
		resDownLoadTools.Add (ardt);
	}
	public static bool NeedDownLoadAsset(){
		return resDownLoadTools.Count > 0;
	}



	protected AssetFileToolUtil afToolManager;
	public string name;
	public AssetResDownload Init(
		string _name,
		string _localSavePath,
		string _localTmpSavePath,
		string _remoteServerPath,
		string _assetFileToolName ,
		string _remoteCdnPath,AssetFileToolUtil util){
		this.name = _name;
		this.afToolManager = util;
		this.downLoadState = DownLoadState.NONE;
		this.localSavePath = _localSavePath;
		this.localTmpSavePath = _localTmpSavePath;
		this.remoteServerPath = _remoteServerPath;
		this.assetFileToolName = _assetFileToolName;
		this.remoteCdnPath = _remoteCdnPath;
		XZXDDebug.LogWarning ("this.localSavePath = "+this.localSavePath+
			"\n\t\tthis.localTmpSavePath = "+this.localTmpSavePath+
			"\n\t\tthis.remoteServerPath = "+this.remoteServerPath+
			"\n\t\tthis.assetFileToolName ="+ this.assetFileToolName+
			"\n\t\tthis.remoteCdnPath = "+this.remoteCdnPath);
		DoInit ();
		return this;
	}

	protected virtual void DoInit ()
	{
		
	}

	public virtual void StartPreDownLoad(){
		downLoadState = DownLoadState.PREDOWNLOADING;

		RunCoroutine.Run (DownloadRemoteAssetFile ());
	}

	private const int MAX_DOWNLOADING = 5;


	public void Tick(){
//		Debug.Log ("downLoadState:" + downLoadState);
		switch (downLoadState) {
		case DownLoadState.PREERROR:
			
			break;
		case DownLoadState.PREDOWNLOADING:
			
			break;
		case DownLoadState.WAIT_REQUEST_DOWNLOAD:

			break;

		case DownLoadState.DOWNLOADING:
			if (downLoadList.Count < MAX_DOWNLOADING) {
				DownloadPackage ();
			}
			List<AssetResSignalDownloadTool> tmps = new List<AssetResSignalDownloadTool> ();
			tmps.AddRange (sigDownLoadList);
			speed = 0;
			foreach (var sig in tmps) {
				speed += sig.speed;
				sig.Tick ();
				CheckDownloadPackageOver (sig);
			}
			if(tmps.Count>0){
				speed /=tmps.Count;
			}
//			Debug.Log("speed:"+speed);
			if (CheckAllAssetPackageDownLoadOver ()) {
				downLoadState = DownLoadState.SUCCESS_DOWNLOAD;
			}
			break;
		case DownLoadState.SUCCESS_DOWNLOAD:
			SuccessHandler ();
			break;
		case DownLoadState.COMPLETE:

			break;
		}
	}

	protected virtual void SuccessHandler(){
		downLoadState = DownLoadState.COMPLETE;
		afToolManager.SetRemoteSetting (remoteAssetFileTool);

//		remoteAssetFileTool.SaveToFile (localSavePath + "/" + assetFileToolName);
	}

	protected void RecordNeedUpdate (bool b)
	{
		afToolManager.RecordNeedUpdate (b);
	}


	private bool CheckAllAssetPackageDownLoadOver(){
		if (preDownLoadList.Count > 0) {
			return false;
		}
		if (downLoadList.Count > 0) {
			return false;
		}
		if (errorList.Count > 0) {
			return false;
		}

		return true;
	}

	private void DownloadPackage(){
		if (preDownLoadList.Count > 0)
		{
			AssetCell ac = preDownLoadList[0];
			preDownLoadList.RemoveAt(0);
			downLoadList.Add(ac);
			AssetResSignalDownloadTool asdt = new AssetResSignalDownloadTool (this, ac);
			sigDownLoadList.Add(asdt);
			StartOneModeDownload(asdt);
		}
		else if (errorList.Count > 0)
		{
			AssetCell ac = errorList[0];
			if (errorCount.ContainsKey(ac.hashCode))
			{
				errorCount[ac.hashCode] = errorCount[ac.hashCode] + 1;
			}
			else
			{
				errorCount[ac.hashCode] = 1;
			}
			//十次下载不到就放弃了
			if (errorCount[ac.hashCode] > 10)
			{
				errorList.RemoveAt(0);
			}
			else
			{
				errorList.RemoveAt(0);
				downLoadList.Add(ac);
				AssetResSignalDownloadTool asdt = new AssetResSignalDownloadTool(this, ac);
				sigDownLoadList.Add(asdt);
				StartOneModeDownload(asdt);
			}
		}
	}
	public Dictionary<string,int> errorCount = new Dictionary<string, int>();

	public void OnApplicationQuit(){
		foreach(var asdt in sigDownLoadList){
			asdt.OnApplicationQuit();
		}
	}

	protected virtual void StartOneModeDownload(AssetResSignalDownloadTool asdt){
		asdt.StartBaseHttpDownLoad (this.remoteCdnPath+"/"+asdt.ac.saveFileName);
	}

	private void CheckDownloadPackageOver(AssetResSignalDownloadTool sig){
		switch (sig.downloadState) {
		case AssetResSignalDownloadTool.DownLoadState.DOWNLOADING:

			break;
		case AssetResSignalDownloadTool.DownLoadState.FAILED:
		case AssetResSignalDownloadTool.DownLoadState.TIMEOUT:
			downLoadList.Remove (sig.ac);
			errorList.Add (sig.ac);
			sigDownLoadList.Remove (sig);
			break;
		case AssetResSignalDownloadTool.DownLoadState.SUCESS:
			sig.MoveCoverage ();
			downLoadList.Remove (sig.ac);
			sigDownLoadList.Remove (sig);
			downloadedCount++;
			downloadedSize += sig.ac.size;
			break;
		}
	}




	public DownLoadState downLoadState;
	public enum DownLoadState
	{
		NONE = 0,
		PREERROR = 1,
		PREDOWNLOADING ,
		WAIT_REQUEST_DOWNLOAD,
		DOWNLOADING ,
		SUCCESS_DOWNLOAD ,
		COMPLETE,

	}




	private IEnumerator DownloadRemoteAssetFile()
	{
		XZXDDebug.LogWarning ("over");
		if (!string.IsNullOrEmpty(remoteServerPath))
		{
			WWW www = new WWW (remoteServerPath+"?fileName="+assetFileToolName+"&channelId=pc");
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				downLoadState = DownLoadState.PREERROR;
				Debug.LogError ("download file error:" + remoteServerPath+"/"+assetFileToolName
				                +"   "+www.error);
			} else {
				DownloadRemoteAssetFileDown(www.text);
			}
		}
		else
		{
			
			WWW wwwCDN = new WWW (this.remoteCdnPath +"/" + assetFileToolName);
			yield return wwwCDN;
			if (!string.IsNullOrEmpty(wwwCDN.error))
			{
				downLoadState = DownLoadState.PREERROR;
				Debug.LogError ("download file error:" + remoteCdnPath+"/"+assetFileToolName
				                +"   "+wwwCDN.error);
			}
			else
			{
				DownloadRemoteAssetFileDown(wwwCDN.text);
			}
		}
	}

	protected void DownloadRemoteAssetFileDown(string data){
		remoteAssetFileTool = AssetFileTool.LoadFromString (data);
		
		if (CheckNeedUpdateAsset ()) {
			RecordNeedUpdate (true);
			totalDownloadCount = preDownLoadList.Count;
			foreach(var ac in preDownLoadList){
				totalSize += ac.size;
			}
			downLoadState = DownLoadState.WAIT_REQUEST_DOWNLOAD;
		} else {
			RecordNeedUpdate (false);
			downLoadState = DownLoadState.SUCCESS_DOWNLOAD;
		}
	}

	public void RequestDownload(){
		AssetResDownload.ifNeedQuestDownload = false;
		downLoadState = DownLoadState.DOWNLOADING;
	}
	
	protected virtual bool CheckNeedUpdateAsset(){
		return false;
	}




	public float GetDownloadSpeed(){
		return speed;
	}

	public long GetTotalSize(){
		return totalSize;
	}

	public long GetDownloadedSize(){
		long size = downloadedSize;
		foreach(var sig in sigDownLoadList){
			size+=sig.loadedSize;
		}
		return size;
	}

	public string GetCurrentDownloadFile(){
		if (sigDownLoadList.Count > 0) {
			return sigDownLoadList [0].ac.saveFileName;
		}
		return "";
	}

	public string GetCurrentDownloadTitle(){
		return "更新"+name;
	}

	/// <summary>
	/// 检查更新包是否需要下载,true=下载，false=不下载
	/// </summary>
	/// <param name="ac"></param>
	/// <returns></returns>
	protected bool CheckIfDownload(AssetCell ac,ref bool needMove)
	{
		needMove = false;
		//判断doc是否存在
		if (File.Exists(localSavePath + "/" + ac.saveFileName))
		{
			FileInfo fi = new FileInfo(localSavePath + "/" + ac.saveFileName);
			if (fi.Length == ac.size)
			{
				return false;
			}
			else
			{
				File.Delete(localSavePath + "/" + ac.saveFileName);
			}
		}

		if (!File.Exists(localTmpSavePath + "/" + ac.saveFileName))
		{
			return true;
		}
		else
		{
			FileInfo fi = new FileInfo(localTmpSavePath + "/" + ac.saveFileName);
			if (fi.Length != ac.size)
			{
				File.Delete(localTmpSavePath + "/" + ac.saveFileName);
				return true;
			}
			else
			{
				needMove = true;
				return false;
			}
		}
	}

}

