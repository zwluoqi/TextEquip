using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimController:MonoBehaviour {


	public WrapMode wrap_mode;
	public bool localRewind;

	public string anim_asset_path;
	public string current_anim_name;
	public int play_count = 0;
	protected Dictionary<string,string> loaded_anims = new Dictionary<string, string> ();

	public virtual int getCurrentPlayCount(){
		return this.play_count;
	}

	public virtual void Init (Transform animTrans, string animAssetPath){
	}

	public virtual bool playAnim(string anim_name,WrapMode wrap_mode ,bool rewind,Action<string> playDoneFun){
		return false;
	}

	public virtual void unLoadResource(){}

	public virtual void Pause(){
		
	}

	public virtual void Continue(){

	}

}
