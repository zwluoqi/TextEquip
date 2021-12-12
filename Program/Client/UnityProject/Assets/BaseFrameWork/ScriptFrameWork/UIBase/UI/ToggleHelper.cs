using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleHelper : UIItemPool {
	
	public delegate void ToggleHelperEvent (ToggleHelper go,bool bo);

	public ToggleHelperEvent toggleCallBack;
	bool ignoreValChanged = false;
	public string toggleTag = "";

	void Awake(){
		GetComponent<Toggle> ().onValueChanged.AddListener (delegate(bool arg0) {
			if(ignoreValChanged){
				return ;
			}
			if(arg0){
				// SoundManager.Instance.PlayClip ("audio/ui/Back");
				toggleCallBack(this,arg0);
			}

		});
	}


	public void SetOff ()
	{
		ignoreValChanged = true;
		GetComponent<Toggle> ().isOn = false;
		ignoreValChanged = false;
	}

	public void SetOn ()
	{
		ignoreValChanged = true;
		GetComponent<Toggle> ().isOn = true;
		ignoreValChanged = false;
	}
}
