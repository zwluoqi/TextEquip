// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using XZXD;
//
// public class SoundManager : MonoBehaviour {
//
// 	public static SoundManager Instance{
// 		get{
// 			return _instance;
// 		}
// 	}
//
// 	private static SoundManager _instance;
//
//
// 	public AudioSource backGround;
//
// 	public AudioSource clipGround;
//
// 	void Awake(){
// 		_instance = this;
// 		float musicVal = PlayerPrefs.GetFloat ("music", 1);
// 		float clipVal = PlayerPrefs.GetFloat ("clip", 1);
// 		this.backGround.volume = musicVal;
// 		this.clipGround.volume = clipVal;
// 	}
//
// 	public string currentBack="";
// 	public void PlayBack(string path){
// 		if (XZXD.Main.Instance.platformData.mudMode) {
// 			return;
// 		}
// 		if (currentBack == path) {
// 			if (!backGround.isPlaying) {
// 				backGround.UnPause ();
// 			}
// 			return;
// 		}
// 		currentBack = path;
// 		AudioClip clip = AssetLoaderManager.Instance.LoadResourceBlock (path) as AudioClip;
// 		backGround.clip = clip;
// 		backGround.Play ();
// 	}
// 	public void StopBack(){
// 		backGround.Pause ();
// 	}
//
// 	public void PlayClip(AudioClip clip){
// 		clipGround.PlayOneShot (clip);
// 	}
//
// 	public void PlayClip(string path){
// 		AudioClip clip = AssetLoaderManager.Instance.LoadResourceBlock (path) as AudioClip;
// 		clipGround.PlayOneShot (clip);
// 	}
//
// 	public AudioSource PlaySourceClip(string path){
//
// 		AudioClip clip = AssetLoaderManager.Instance.LoadResourceBlock (path) as AudioClip;
// 		if (clip == null) {
// 			return null;
// 		}
//
// 		AudioSource newSource = this.gameObject.AddComponent<AudioSource> ();
// 		newSource.loop = false;
// 		newSource.clip = clip;
// 		newSource.volume = this.clipGround.volume;
// 		if (XZXD.Main.Instance.platformData.mudMode) {
// 			
// 		} else {
// 			newSource.Play ();
// 		}
// 		GameObject.Destroy (newSource, clip.length*Time.timeScale);
// 		return newSource;
// 	}
//
//
//
// }
