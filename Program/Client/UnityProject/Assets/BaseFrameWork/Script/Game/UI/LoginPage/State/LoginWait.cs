using XZXD.UI;
using System.Collections.Generic;
using UnityEngine;
using XZXD;
using System.Text;
using Script.Game.System;


public class LoginWait:LoginState{


	public LoginWait(LoginStateEnum state,LoginController controller):base(state,controller){

	}

	protected override void Enter (FSM.FSMParam<LoginStateEnum> enterParam)
	{
		DictDataManager.Release ();
		DictDataManager.Instance.Init("Dict", null);
		// BoxManager.OpenSimpleGongGao ();
		//TODO
		
		if (PluginUtil.CheckModifuApp ()) {
			BoxManager.CreatOneButtonBox ("独立开发游戏不易，给点面子，不要用修改器。",delegate(bool bo) {
				Application.Quit ();	
			});
		}
		else
		{
			GameSystem.Instance.Start();
		}
	}

	protected override void Leave (FSM.FSMParam<LoginStateEnum> leaveParam)
	{
	}


}
