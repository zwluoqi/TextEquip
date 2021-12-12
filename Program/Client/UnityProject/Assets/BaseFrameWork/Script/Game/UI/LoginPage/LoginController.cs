using System;
using XZXD.UI;
using UnityEngine;
using AssetPlugin;
using Script;
using XZXD;


public enum LoginStateEnum{
	

	

	NONE,
	// CheckAgreeMent,
	// CheckVersion,
	// SelectUpdateApp,
	// UpdateApp,
	CheckConfig,
	Wait,
}

public class LoginState:FSM.FSMBaseState<LoginStateEnum,LoginController>{


	public LoginState(LoginStateEnum state,LoginController controller):base(state,controller)
	{

	}
}


public class LoginController:FSM.FSMBaseCtrl<LoginStateEnum,LoginState>
{
	public SimpleLoginPage page;



	public LoginController(SimpleLoginPage page):base(){


		this.page = page;
//
		VersionTool.url = "http://114.67.88.112:8080/xzxdweb/";
//
		NetManager.ResetZoneUrl (VersionTool.url);

		AssetFileToolUtilManager.Instance.txt.InitInnerSetting ("Dict/md5.txt");
		// AssetFileToolUtilManager.Instance.texture.InitInnerSetting ("textures/md5");
		// AssetFileToolUtilManager.Instance.atlas.InitInnerSetting ("atlas/md5");


		AddState (new LoginWait (LoginStateEnum.Wait,this ));
		AddState (new LoginCheckConfig (LoginStateEnum.CheckConfig,this));
		// AddState (new LoginCheckVersion (LoginStateEnum.CheckVersion,this));
		// AddState (new LoginUpdateApp (LoginStateEnum.UpdateApp,this));
		// AddState (new LoginSelectUpdateApp (LoginStateEnum.SelectUpdateApp,this));
		// AddState (new LoginCheckAgreeMent (LoginStateEnum.CheckAgreeMent,this));

		SetDefaultState (LoginStateEnum.CheckConfig);


	}
}