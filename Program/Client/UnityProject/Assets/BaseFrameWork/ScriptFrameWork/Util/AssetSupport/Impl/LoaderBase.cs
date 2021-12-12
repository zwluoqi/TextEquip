using UnityEngine;
using System.Collections;
using System.IO;

namespace XZXD
{
    //public delegate void Void_AssetRef (AssetRef assRef);
    public delegate void Void_LoaderBase (LoaderBase loader);
    public delegate void Void_Float (float num);
    //public delegate void Void_String_AssetRef(string name, AssetRef asset);
    /// <summary>
    /// 资源加载接口
    /// </summary>
    public class LoaderBase
    {
        //用户回调
        public Void_Str_Obj userCB = null;
        //系统回调，仅供系统内部使用。
        public Void_LoaderBase sysCB = null;
        // 场景资源名称
        protected string resName = "";
        // 加载进度回调
        protected Void_Float progressCB = null;
        // 加载优先级
        private ELoaderPrior prior = ELoaderPrior.eNone;
        // 加载器类型
        private ELoaderType loaderType = ELoaderType.eNone;

        public LoaderBase (ELoaderType aType, ELoaderPrior aPrior)
        {
            loaderType = aType;
            prior = aPrior;
        }

        /// <summary>
        /// 获得加载器优先级
        /// </summary>
        public int GetPrior ()
        {
            return (int)prior;
        }

        public ELoaderType GetLoaderType ()
        {
            return loaderType;
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        public virtual void StartLoad ()
        {
        }

        /// <summary>
        /// 获得加载结果对象
        /// </summary>
        /// <returns> The load result. </returns>
        public virtual object GetLoadResult ()
        {
            return null;
        }

        /// <summary>
        /// 设置欲加载资源路径
        /// </summary>
        /// <param name="resPath">资源路径.</param>
        public virtual void SetResPath (string resPath)
        {
            resName = resPath;
            if (resPath == "Effect/")
            {
                Debug.LogError("SetResPath");
            }
        }

        public virtual string GetResPath ()
        {
            return resName;
        }

        /// <summary>
        /// 设置加载进度回调
        /// </summary>
        public virtual void SetProgressCB (Void_Float cb)
        {
            progressCB = cb;
        }

    }
}
