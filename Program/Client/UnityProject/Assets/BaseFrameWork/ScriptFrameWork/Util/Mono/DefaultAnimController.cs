//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;
//
//public class DefaultAnimController : AnimController {
//
//	public Animation unity_animation ;
//
//
//	public override void Init(Transform animTrans,string animAssetPath){
//		this.unity_animation = animTrans.GetComponent<Animation>();
//		this.anim_asset_path = animAssetPath;
//
//		this.loaded_anims = new Dictionary<string,string> ();
//	}
//
//
//
//	public override bool playAnim(string anim_name,WrapMode wrap_mode ,bool rewind,Action<string> playDoneFun)
//	{
//		
//		if (this.unity_animation == null) {
//			Debug.LogError (gameObject.name+ "..unity_animation nil:"+anim_name);
//			return false;
//		}
//		this.current_anim_name = anim_name;
//		this.wrap_mode = wrap_mode;
//		this.localRewind = rewind;
//		if (anim_name == "") {
//			return false;
//		}
//
//		if (this.unity_animation.GetClip(anim_name) ) {
//			this.palyAnim0 (anim_name, wrap_mode, rewind,playDoneFun);
//			this.play_count++;
//			return true;
//		}
//		else if (this.loaded_anims.ContainsKey(anim_name) ){
//			return false;
//		}
//		else{
//			this.loaded_anims [anim_name] = anim_name;
//			string res_path = this.anim_asset_path+"/"+anim_name;
//			AssetLoaderManager.Instance.LoadResourceAsync (res_path,
//				delegate(string str, UnityEngine.Object asset) {
//					if (asset == null) {
//						return;
//					}
//					this.unity_animation.AddClip (asset as AnimationClip, anim_name);
//
//					if (this.current_anim_name == anim_name) {
//						this.palyAnim0 (anim_name, wrap_mode, rewind,playDoneFun);
//						this.play_count++;
//					}
//				}
//			);
//			return false;
//		}
//
//	}
//
//	private void palyAnim0(string anim_name,WrapMode wrap_mode ,bool rewind,Action<string> playDoneFun){
//		this.unity_animation[anim_name].wrapMode = wrap_mode;
//		this.unity_animation [anim_name].speed = globalSpeed;
//
//		if (this.unity_animation.IsPlaying(anim_name)) {
//			if (rewind) {
//				this.unity_animation.Rewind(anim_name);
//				this.unity_animation.Play(anim_name);
//			}
//		}
//		else{
//			this.unity_animation.Play(anim_name);
//		}
//
//		RunCoroutine.Run (PlayDone (playDoneFun,anim_name,this.unity_animation.GetClip(anim_name).length));
//	}
//
//	private IEnumerator PlayDone(Action<string> playDoneFun,string anim_name,float duration){
//		yield return new WaitForSeconds (duration);
//		if (playDoneFun != null) {
//			playDoneFun (anim_name);
//		}
//	}
//		
//
//	public override void unLoadResource(){
//
//		//TODO
//
//	}
//
//	public float globalSpeed = 1;
//	public override void Pause ()
//	{
//		globalSpeed = 0;
//		if(this.ContainKey(current_anim_name)){
//			this.unity_animation [current_anim_name].speed = globalSpeed;
//		}
//	}
//
//	public override void Continue ()
//	{
//		globalSpeed = 1;
//		if (this.ContainKey (current_anim_name)) {
//			this.unity_animation [current_anim_name].speed = globalSpeed;
//		}
//	}
//
//
//
//	bool ContainKey (string current_anim_name)
//	{
//		var clip = this.unity_animation.GetClip (current_anim_name);
//		if (clip == null) {
//			return false;
//		} else {
//			return true;
//		}
//	}
//}
