using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class SimpleLoginPage:UIPage
{
    public TMP_Text childUpdateResText;
    public TMP_Text childUpdateTitleText;
    public Slider childUpdateResSilder;
    public TMP_Text childUpdateResSpeed;
    public GameObject childUpdateRes;
	LoginController controller;

    protected override void DoOpen()
    {

	    controller = new LoginController (this);
	    Debug.LogError (System.DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
	    
    }

    protected override void OnCurrentPageTick(float deltaTime)
    {
	    controller.Tick(Time.deltaTime);
    }


    public void ShowCheckUpdate ()
	{
		// HideAll ();
		childUpdateRes.SetActive (true);
		this.childUpdateResSilder.value = 0;
		this.childUpdateResSpeed.text = "";
	}
}