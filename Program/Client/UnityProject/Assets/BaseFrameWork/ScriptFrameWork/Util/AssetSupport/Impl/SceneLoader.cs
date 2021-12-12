using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace XZXD
{
    /// <summary>
    /// 场景加载器
    /// </summary>
    public class SceneLoader : LoaderBase
    {
        private string m_ReplaseSceneName = "default_scene";

        public SceneLoader () : base (ELoaderType.eLoadScene, ELoaderPrior.ePrior1)
        {
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="cb"> 加载完毕回调. </param>
        public override void StartLoad ()
        {
			RunCoroutine.Run (_LoadLevel());
        }

		private IEnumerator _LoadLevel(){

			yield return SceneManager.LoadSceneAsync (this.resName);

			if (null != sysCB)
				sysCB(this);
		}

        
    }
}

