using UnityEngine;
using XZXD.UI;

using System.Collections.Generic;
using AssetPlugin;
using System;
using System.IO;

public class LoginCheckConfig:LoginState{
	public LoginCheckConfig(LoginStateEnum state,LoginController controller):base(state,controller){

	}
	Queue<AssetResDownload> queues = new Queue<AssetResDownload>();

    protected override void Enter(FSM.FSMParam<LoginStateEnum> enterParam)
    {
        this._ctrl.page.ShowCheckUpdate();

        string txtUrl = "";
        string txtcdn = string.IsNullOrEmpty(VersionTool.cdnDownloadUrl) ? "" : VersionTool.cdnDownloadUrl + "/text_equip/txt";
        waitRepreLoad = false;
        var config = new DynamicResDownload();
        config.Init("文本",PathTool.TxtSavePath, PathTool.TxtTmpSavePath, txtUrl, "md5.txt.bytes", txtcdn, AssetFileToolUtilManager.Instance.txt);
        queues.Enqueue(config);

  //       string textureUrl = VersionTool.url + "download/texture/";
  //       string texturecdn = string.IsNullOrEmpty(VersionTool.cdnDownloadUrl) ? "" : VersionTool.cdnDownloadUrl + "download/texture";
  //       var texture = new DynamicResDownload();
		// texture.Init("图片",PathTool.TextureSavePath, PathTool.TextureTmpSavePath, textureUrl, "md5.txt", texturecdn, AssetFileToolUtilManager.Instance.texture);
  //       queues.Enqueue(texture);
  //
  //       string atlasUrl = VersionTool.url + "download/atlas/";
  //       string atlascdn = string.IsNullOrEmpty(VersionTool.cdnDownloadUrl) ? "" : VersionTool.cdnDownloadUrl + "download/atlas";
  //       var atlas = new DynamicResDownload();
		// atlas.Init("图集",PathTool.AtlasSavePath, PathTool.AtlasTmpSavePath, atlasUrl, "md5.txt", atlascdn, AssetFileToolUtilManager.Instance.atlas);
  //       queues.Enqueue(atlas);
        

    }
	public bool waitRepreLoad = false;

	protected override void Tick (float delta)
	{
		this._ctrl.page.childUpdateResText.text = "";
		if (queues.Count == 0) {
			Goto (LoginStateEnum.Wait);
		}else{
			var dstd = queues.Peek();
			dstd.Tick ();
			if (dstd.downLoadState == AssetResDownload.DownLoadState.NONE) {
				dstd.StartPreDownLoad ();
			}
			else if (dstd.downLoadState == AssetResDownload.DownLoadState.WAIT_REQUEST_DOWNLOAD) {
				dstd.RequestDownload ();
			}
			else if (dstd.downLoadState == AssetResDownload.DownLoadState.COMPLETE) {
				queues.Dequeue ();
			}
			else if (dstd.downLoadState == AssetResDownload.DownLoadState.DOWNLOADING) {
				if (this._ctrl.page.childUpdateTitleText != null) {
					this._ctrl.page.childUpdateTitleText.text = dstd.GetCurrentDownloadTitle ();
				}

				this._ctrl.page.childUpdateResSilder.value = dstd.GetDownloadedSize ()*1.0f / dstd.GetTotalSize ()*1.0f;
				this._ctrl.page.childUpdateResSpeed.text = (dstd.GetDownloadedSize ()*0.001).ToString("F2") + "/" + (dstd.GetTotalSize ()*0.001).ToString("F2") +"KB";
			}
			else if (dstd.downLoadState == AssetResDownload.DownLoadState.PREERROR && !waitRepreLoad) {
				queues.Dequeue ();
				Debug.LogError ("down done:"+dstd.downLoadState );
			}
		}
	}

    

    protected override void Leave (FSM.FSMParam<LoginStateEnum> leaveParam)
	{
		this._ctrl.page.childUpdateResText.text = "";
	}
}