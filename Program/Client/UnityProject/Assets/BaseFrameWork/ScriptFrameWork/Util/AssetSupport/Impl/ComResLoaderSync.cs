using UnityEngine;
using System.Collections;
using System.IO;

namespace XZXD
{
    /// <summary>
    /// 除了 场景资源 以外的资源的加载器，阻塞式同步加载
    /// </summary>
    public class ComResLoaderSync : LoaderBase
    {
        object resu = null;

        public ComResLoaderSync()
            : base(ELoaderType.eLoadComResSync, ELoaderPrior.ePrior2)
        {
        }

        public override void StartLoad()
        {
            if (string.IsNullOrEmpty(resName))
            {
				XZXDDebug.LogWarning("error! resName is empty in ComResLoader ");
                OnLoadFin(null, null);
            }
            else
            {
#if DEBUG
				XZXDDebug.LogWarning(Time.realtimeSinceStartup + "\t Load:" + resName);
#endif
				Object asset = AssetLoaderManager.Instance.LoadResourceBlock(resName);
                OnLoadFin(resName, asset);
            }
        }

        public override string GetResPath()
        {
            string path = Path.GetDirectoryName(resName);
            path += "/" + Path.GetFileNameWithoutExtension(resName);
            return path;
        }

		private void OnLoadFin(string path, Object asset)
        {
            if (asset != null)
            {
#if DEBUG
				XZXDDebug.LogWarning(Time.realtimeSinceStartup + "Load Scuess:" + resName);
#endif
				GameObjectPoolManager.Instance.Regist(resName,1,asset);
            }
            else
            {
                Debug.LogError(Time.realtimeSinceStartup + " \t Load Failed:" + resName);
            }
            resu = asset;
            if (null != sysCB)
                sysCB(this);
        }
        public override object GetLoadResult()
        {
            return resu;
        }
    }
}