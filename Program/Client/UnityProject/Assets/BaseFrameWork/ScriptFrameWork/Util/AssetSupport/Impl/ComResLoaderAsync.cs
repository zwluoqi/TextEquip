using UnityEngine;
using System.Collections;
using System.IO;

namespace XZXD
{
    /// <summary>
    /// 除了 场景资源 以外的资源的加载器
    /// </summary>
    public class ComResLoaderAsync : LoaderBase
    {
        object resu = null;
        private bool useBundle = false;

        public ComResLoaderAsync()
            : base(ELoaderType.eLoadComResAsync, ELoaderPrior.ePrior2)
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
				AssetLoaderManager.Instance.LoadResourceAsync(resName, (string path,UnityEngine.Object asset) =>
                {
                    OnLoadFin(resName, asset);
                });
                
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