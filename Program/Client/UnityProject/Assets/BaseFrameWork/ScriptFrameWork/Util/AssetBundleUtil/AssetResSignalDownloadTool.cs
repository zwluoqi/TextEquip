using System;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Net;
using System.Collections;

using System.Collections.Generic;
using AssetPlugin;


public class AssetResSignalDownloadTool
{
	
	public AssetResDownload parentAssetResDownLoadTool;
	
	public AssetResSignalDownloadTool (AssetResDownload _parentAssetResDownLoadTool,AssetCell _ac)
	{
		
		this.parentAssetResDownLoadTool = _parentAssetResDownLoadTool;
		this.ac = _ac;
	}
	
	
	
	
	
	
	public AssetCell ac;
	
	
	public long loadedSize = 0;

	public Thread downloadThread;
	private float unscaledTime = 0;
	private float threadRunTickTime = 0;
	public float speed;
	
	public void Tick(){
		unscaledTime = Time.unscaledTime;
		if (unscaledTime - threadRunTickTime > 30)
		{
			Debug.LogError("Tick下载超时：" + ac.path +" saveFileName:"+ac.saveFileName);
			OnApplicationQuit();
		}
	}
	
	public DownLoadState downloadState {
		private set;
		get;
	}
	public enum DownLoadState{
		NONE,
		DOWNLOADING,
		SUCESS,
		FAILED,
		TIMEOUT,
	}


	
	public void StartWWWDownload(string webUrl){
		XZXDDebug.Log("StartWWWDownload:"+webUrl);
		requestDownLoad = true;
		unscaledTime = Time.unscaledTime;
		threadRunTickTime = unscaledTime;
		downloadState = DownLoadState.DOWNLOADING;
		
		RunCoroutine.Run(_StartWWWDownload(webUrl));
		
	}
	
	private IEnumerator _StartWWWDownload(string webUrl){
		bool success = false;
		long loadCounter = 0;
		WWW www = new WWW(webUrl);
		
		while (true){
			yield return new WaitForSeconds(0.5f);
			threadRunTickTime = unscaledTime;
			if(www.isDone && string.IsNullOrEmpty(www.error)){
				success = true;
				break;
			}
			
			if(!string.IsNullOrEmpty(www.error)){
				XZXDDebug.Error("下载超时：" + ac.path +" saveFileName:"+ac.saveFileName);
				success = false;
				break;
			}
			loadedSize = www.bytesDownloaded;
			loadCounter++;
			speed = loadedSize/loadCounter*2;
		}
		
		if(success){
			string toPath = parentAssetResDownLoadTool.localTmpSavePath + "/" + ac.saveFileName;
			FileUtils.CreateDir(Path.GetDirectoryName (toPath));
			FileStream fileStream = new FileStream(toPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			
			fileStream.Seek(0, SeekOrigin.Begin);
			fileStream.Write(www.bytes,0,www.bytes.Length);
			fileStream.Flush();
			fileStream.Close();
			fileStream.Dispose();
			XZXDDebug.Log("加入移动列表：" + ac.path+" saveFileName:"+ac.saveFileName);
			downloadState = DownLoadState.SUCESS;
		}else{
			downloadState = DownLoadState.FAILED;
		}
		yield return null;
	}

	long GetLength(string _fileUrl)
	{
		//XZXDDebug.Error("GetLength = " + _fileUrl);
		try{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_fileUrl);
			request.ReadWriteTimeout = 5000;
			if (request != null)
			{
				request.Method = "HEAD";
				HttpWebResponse res = (HttpWebResponse)request.GetResponse();
				if (res != null)
				{
					//XZXDDebug.Error("res.ContentLength=" + res.ContentLength);
					return res.ContentLength;
				}
				XZXDDebug.Error("res is null! _fileUrl=" + _fileUrl);
			}else{
				XZXDDebug.Error("连接错误！ _fileUrl=" + _fileUrl);
			}
		}catch(Exception e){
			XZXDDebug.Error("连接错误！ _fileUrl=" + _fileUrl+" e:"+e.Message);
		}
		return -1;
	}

	
	
	public void StartBaseHttpDownLoad(string webUrl){
		XZXDDebug.Log("StartBaseHttpDownLoad:"+webUrl);
		requestDownLoad = true;
		unscaledTime = Time.unscaledTime;
		threadRunTickTime = unscaledTime;
		downloadState = DownLoadState.DOWNLOADING;
		
		downloadThread = new Thread(() =>
		                            {
			threadRunTickTime = unscaledTime;
			long totalLength = GetLength(webUrl);//从服务器获取的文件长度
			if(totalLength == -1){
				XZXDDebug.Error("get file error！"+ac.saveFileName);
				downloadState = DownLoadState.FAILED;
				return;
			}
			string toPath = parentAssetResDownLoadTool.localTmpSavePath + "/" + ac.saveFileName;
			FileUtils.CreateDir(Path.GetDirectoryName (toPath));
			FileStream fileStream = new FileStream(toPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			long fileLength = fileStream.Length;//当前已下载的文件长度
			
			if (fileLength < totalLength)
			{
				XZXDDebug.Log("开始下载资源,ac.path =" + ac.path +" ac.size:"+ ac.size+" saveFileName:"+ac.saveFileName+ " toPath=" + toPath + "    webUrl=" + webUrl+" totalLength=" + totalLength + "    fileLength=" + fileStream.Length);
				
				Stream httpStream = null;
				try
				{
					HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(webUrl);
					request.ReadWriteTimeout = 5000;
					request.AddRange((int)fileLength);//server not support mid download
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
					if(response.StatusCode != HttpStatusCode.PartialContent && fileLength > 0){
						fileLength = 0;
						XZXDDebug.Error("server not support partialcontent");
					}
					threadRunTickTime = unscaledTime;
					
					fileStream.Seek(fileLength, SeekOrigin.Begin);
					httpStream = response.GetResponseStream();
					
					byte[] buffer = new byte[64 * 1024];
					int length = httpStream.Read(buffer, 0, buffer.Length);
					float pretime = unscaledTime;
					float prePercent = 0;
					
					long loadCounter = 0;
					
					while (length > 0 && requestDownLoad)
					{
						threadRunTickTime = unscaledTime;
						
						fileStream.Write(buffer, 0, length);
						fileLength += length;
						loadedSize += length;


						if (fileLength != prePercent)
						{
							pretime = unscaledTime;
							prePercent = fileLength;
						}
						else
						{
							if (unscaledTime - pretime > 30)
							{
								XZXDDebug.Error("下载超时：" + ac.path +" saveFileName:"+ac.saveFileName);
								downloadState = DownLoadState.TIMEOUT;
								break;
							}
						}
						loadCounter++;
						speed = loadedSize/loadCounter*50f;
						
						//						XZXDDebug.Log( ac.path+" saveFileName:"+ac.saveFileName+" speed："+speed+" loadCounter:"+loadCounter);
						
						Thread.Sleep(20);
						
						length = httpStream.Read(buffer, 0, buffer.Length);
					}
					XZXDDebug.Log("download over fileLength=" + fileLength.ToString() + " totalLength=" + totalLength.ToString());
				}
				catch (Exception ex)
				{
					XZXDDebug.Error(ac.path +" saveFileName:"+ac.saveFileName+ "下载中断，收到错误信息：" + ex.Message);
					downloadState = DownLoadState.FAILED;
				}
				finally
				{
					if (httpStream != null)
					{
						fileStream.Flush();
						fileStream.Close();
						fileStream.Dispose();
						
						httpStream.Close();
						httpStream.Dispose();
					}
				}
			}
			else
			{
				fileStream.Close();
				fileStream.Dispose();
			}
			if(!requestDownLoad){
				XZXDDebug.Error("cancle downlaod:"+ ac.path+" saveFileName:"+ac.saveFileName);
				downloadState = DownLoadState.FAILED;
				return;
			}
			if (fileLength == totalLength && totalLength >= 0)
			{
				XZXDDebug.Log("加入移动列表：" + ac.path+" saveFileName:"+ac.saveFileName);
				downloadState = DownLoadState.SUCESS;
			}else {
				if(fileLength > totalLength){
					if (File.Exists(toPath))
					{
						File.Delete(toPath);
					}
					XZXDDebug.Error("file lengh large delete！"+ac.saveFileName+" fileLength=" + fileLength.ToString() + " totalLength=" + totalLength.ToString());
				}
				XZXDDebug.Error("file lengh error！"+ac.saveFileName+" fileLength=" + fileLength.ToString() + " totalLength=" + totalLength.ToString());
				downloadState = DownLoadState.FAILED;
			}
		});
		downloadThread.IsBackground = true;
		downloadThread.Start();
	}
	
	public bool requestDownLoad = false;
	
	public void OnApplicationQuit(){
		if (downloadState == DownLoadState.DOWNLOADING) {
			requestDownLoad = false;
		}
		downloadState = DownLoadState.FAILED;
	}
	
	
	
	public void MoveCoverage()
	{
		string source = parentAssetResDownLoadTool.localTmpSavePath + "/" + ac.saveFileName; 
		string dest = parentAssetResDownLoadTool.localSavePath + "/" + ac.saveFileName;
		if (File.Exists(dest))
		{
			File.Delete(dest);
		}
		
		FileUtils.CreateDir(Path.GetDirectoryName (dest));
		
		if (File.Exists(source))
		{
			File.Move(source, dest);
		}
	}
}
