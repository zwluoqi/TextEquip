using UnityEngine;
using System.Collections;

namespace XZXD
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ELoaderType
    {
        //无效类型
        eNone,
        //替换式 加载场景
        eLoadScene,
        //累加式 加载场景
        eLoadSceneAdd,
        //异步加载 audio, texture, prefab
        eLoadComResAsync,
        //阻塞式加载
        eLoadComResSync,
    }

    /// <summary>
    /// 加载优先级
    /// 不同优先级的加载器之间，是顺序执行的（从小到大）；相同优先级之间是并行执行的。
    /// </summary>
    public enum ELoaderPrior
    {
        eNone = 0,
        ePrior1 = 100,
        ePrior2 = 200,
    }
    public interface IResLoadMgr
    {
        void Start ();

        void ShutDown ();

        void AddRes(ELoaderType type, string res, Void_Str_Obj userCB, string bundleName = "");

        void StartLoad (Void_Void loadDoneCB);
    }
}
