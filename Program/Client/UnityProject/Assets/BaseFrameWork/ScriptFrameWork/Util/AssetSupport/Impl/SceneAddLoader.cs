using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XZXD
{
    /// <summary>
    /// 场景加载器
    /// </summary>
    public class SceneAddLoader : LoaderBase
    {
        private string m_ReplaseSceneName = "default_scene";

        public SceneAddLoader()
            : base(ELoaderType.eLoadSceneAdd, ELoaderPrior.ePrior1)
        {
        }

	}
}


