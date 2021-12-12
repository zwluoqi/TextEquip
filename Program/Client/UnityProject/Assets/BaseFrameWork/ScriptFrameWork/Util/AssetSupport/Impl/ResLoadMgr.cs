using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace XZXD
{
    /// <summary>
    /// 资源加载管理
    /// 本类会产生消息：加载开始 LoadStart，加载进度 LoadProgress， 加载结束 LoadEnd
    /// 同样也会触发相应的回调，方便用户使用。
    /// 发送 Event_LoadProgress 事件，通知加载进度
    /// 场景加载要先于其它场景内资源的加载，因为unity在切换场景时会自动将创建的Go删除（如果没有设置为NotDestroyOnLoad）
    /// 因此，eLoadScene 类型的场景加载 要比其它任何加载类型 先做。
    /// 更灵活的做法是，为每种loader分配优先级编号，相同优先级的loader之间是可以并行加载的；不同优先级之间必须是按顺序执行。
    /// </summary>
    public class ResLoadMgr : IResLoadMgr
    {
        // <优先级，加载器列表>
        private SortedList<int,List<LoaderBase>> loaderPrior = new SortedList<int, List<LoaderBase>> ();
        //全部加载完毕的回调
        private Void_Void loadAllDoneCB = null;
        //任务总数
        private int totalTaskSum = 0;
        //任务总数的倒数（为了避免除法）
        private float reciTaskSum = 0;

        public void Start ()
        {
        }

        public void ShutDown ()
        {
            loaderPrior.Clear ();
        }

        /// <summary>
        /// 添加加载资源.
        /// 注意，用户提供的回调函数是 Void_Object 类型，之所以参数不是 AssetRef，目的是为了
        /// 加载器能够尽量在 Loading 状态时就进行 Instantiate ，因为频繁的 Instantiate 也会
        /// 造成卡帧的现象。
        /// AddRes() 是一个工厂方法，会根据给定 ELoaderType 类型，自行决定使用哪个加载器类，将加载器隐藏在接口之下。
        /// </summary>
        /// <param name="type">加载器类型.</param>
        /// <param name="res">加载的资源路径名称.</param>
        /// <param name="userCB">资源加载完毕后的回调。</param>
        public void AddRes(ELoaderType type, string res, Void_Str_Obj userCB, string bundleName)
        {
            if (string.IsNullOrEmpty (res)) {
                XZXDDebug.LogWarning ("error! invalid param in PreloadingMgr.AddRes(res=" + res);
                return;
            }
            switch (type) {
            // 替换式 加载场景
            case ELoaderType.eLoadScene:
                AddRes (new SceneLoader (), res, userCB);
                break;
            // 累加式 加载场景
            case ELoaderType.eLoadSceneAdd:
                AddRes (new SceneAddLoader (), res, userCB);
                break;
            //// 异步往场景内创建其它类型（非场景）的资源
            case ELoaderType.eLoadComResAsync:
                AddRes(new ComResLoaderAsync(), res, userCB);
                break;
            case ELoaderType.eLoadComResSync:
                AddRes(new ComResLoaderSync(), res, userCB);
                break;
            default:
                Debug.LogError ( "error! invalid param in PrelaodingMgr.AddRes(type=" + type);
                break;
            }
        }

        /// <summary>
        /// 开始加载资源
        /// </summary>
        /// <param name="loadDoneCB">加载完毕的回调函数</param>
        /// <param name="progressCB">加载进程的回调函数</param>
        public void StartLoad (Void_Void aLoadDoneCB)
        {
            loadAllDoneCB = aLoadDoneCB;

            // 计算任务总数
            this.totalTaskSum = GetTaskSum ();
            this.reciTaskSum = (0 < totalTaskSum ? (1.0f / totalTaskSum) : 0);

            // 从最低等级开始加载
            int nextPrior = GetNextPrior ((int)ELoaderPrior.eNone);
            if (nextPrior > (int)ELoaderPrior.eNone)
                StartLoadPrior(nextPrior);
            else
                loadAllDoneCB();
        }

        private void AddRes (LoaderBase loader, string res, Void_Str_Obj userCB)
        {
            //设置用户回调
            loader.userCB = userCB;
            //设置系统回调
            loader.sysCB = OnLoaderFin;
            //设置资源加载路径
            loader.SetResPath (res);
            //添加加载器到队列中
            int prior = loader.GetPrior ();
            if (!loaderPrior.ContainsKey (prior))
                loaderPrior.Add (prior, new List<LoaderBase> ());
            loaderPrior [prior].Add (loader);
        }

        /// <summary>
        /// 根据给定的优先级，返回下一个紧邻的优先级，如果没有了，返回 ELoaderPrior.eNone;
        /// </summary>
        /// <returns>The next prior.</returns>
        /// <param name="aCurPrior">A current prior.</param>
        private int GetNextPrior (int aCurPrior)
        {
            int resu = (int)ELoaderPrior.eNone;
            bool found = false;
            foreach (var p in loaderPrior) {
                if ((int)ELoaderPrior.eNone == aCurPrior) {
                    resu = p.Key;
                    break;
                }
                if (found) {
                    resu = p.Key;
                    break;
                }
                if (aCurPrior == p.Key)
                    found = true;
            }
            return resu;
        }

        /// <summary>
        /// 启动加载指定优先级下的所有加载器。
        /// </summary>
        /// <param name="aPrior">优先级.</param>
        private void StartLoadPrior (int aPrior)
        {
            if (!loaderPrior.ContainsKey (aPrior)) {
                Debug.LogError ( "error! invalid param in StartLoadPrior( prior=" + aPrior);
                return;
            }
            List<LoaderBase> tempLoaders = new List<LoaderBase> ();
            for (int i = 0, count = loaderPrior [aPrior].Count; i < count; ++i) {
                tempLoaders.Add (loaderPrior [aPrior][i]);
            }
            for (int i = 0, max = tempLoaders.Count; i < max; ++i) {
                tempLoaders [i].StartLoad ();
            }
        }

        /// <summary>
        /// 某一个loader加载完毕的回调
        /// </summary>
        /// <param name="loader">Loader.</param>
        private void OnLoaderFin (LoaderBase loader)
        {
            //删除已经加载完毕的加载器
            RemoveLoader (loader);
            //触发加载进度事件
            //IEventFactory.GetInstance ().SendEvent (new Event_LoadProgress (CalcLoadPerc ()));
            //触发用户回调
            if (null != loader.userCB)
                loader.userCB (loader.GetResPath (), loader.GetLoadResult ());
            // 如果相同优先级的loader都加载完毕，则需要触发 该优先级别的整体回调
            int prior = loader.GetPrior ();
            if (IsLoaderPriorFin (prior))
                OnLoaderPriorFin (loader);
        }

        /// <summary>
        /// 当相同优先级的所有加载器完毕时
        /// </summary>
        /// <param name="lastLoader">相同优先级的加载器中的最后一个.</param>
        private void OnLoaderPriorFin (LoaderBase lastLoader)
        {
            // 如果所有优先级的加载器都加载完毕，则触发alldone，否则继续下一个优先级。
            if (IsAllLoaderFin ()) {
                RemoveAllPrior ();
                OnLoadAllDone ();
            } else {
                int nextPrior = GetNextPrior (lastLoader.GetPrior ());
                //移除已经全部加载完的优先级
                RemovePrior (lastLoader.GetPrior ());
                //继续下一个优先级
                StartLoadPrior (nextPrior);
            }
        }

        /// <summary>
        /// 判断是否所有的加载器都加载完毕了。
        /// </summary>
        private bool IsAllLoaderFin ()
        {
            bool resu = true;
            foreach (var loaders in loaderPrior) {
                if (!IsLoaderPriorFin (loaders.Key)) {
                    resu = false;
                    break;
                }
            }
            return resu;
        }

        /// <summary>
        /// 判断是否给定优先级的所有加载器都加载完成了
        /// </summary>
        private bool IsLoaderPriorFin (int aPrior)
        {
            if (!loaderPrior.ContainsKey (aPrior)) {
                Debug.LogError ( "error! invalid param in IsLoaderPriorFin(aPrior=" + aPrior);
                return false;
            }
            var loaders = loaderPrior [aPrior];
            return 0 >= loaders.Count;
        }

        /// <summary>
        /// 移除给定优先级
        /// </summary>
        /// <param name="key">Key.</param>
        private void RemovePrior (int prior)
        {
            if (!IsLoaderPriorFin (prior))
				XZXDDebug.LogWarning ("Warning! not all loader finished when RemovePrior(prior=" + prior);
            loaderPrior.Remove (prior);
        }

        private void RemoveAllPrior ()
        {
            loaderPrior.Clear ();
        }

        /// <summary>
        /// 删除loader
        /// </summary>
        /// <param name="aLoader">A loader.</param>
        private void RemoveLoader (LoaderBase aLoader)
        {
            if (null == aLoader)
                return;
            int prior = aLoader.GetPrior ();
            if (!loaderPrior.ContainsKey (prior)) {
                Debug.LogError ( "error! invalid param in RemoveLoader(aLoader=" + aLoader.GetResPath ());
                return;
            }

            var loaders = loaderPrior [prior];
            for (int i = 0, max = loaders.Count; i < max; ++i) {
                if (loaders [i] == aLoader) {
                    loaders.RemoveAt (i);
                    break;
                }
            }
        }
        //当所有资源加载完毕
        private void OnLoadAllDone ()
        {
            // 触发 all done 回调
            if (null != loadAllDoneCB)
                loadAllDoneCB ();
        }

        /// <summary>
        /// 计算加载进度百分比(0~1)
        /// </summary>
        public float CalcLoadPerc ()
        {
            float resu = 0.0f;
            // 已完成任务的数量 与 任务总数 的比
            int finTaskSum = totalTaskSum - GetTaskSum ();
            if (finTaskSum == totalTaskSum)
                resu = 1.0f;
            else
                resu = finTaskSum * reciTaskSum;
            return resu;
        }

        /// <summary>
        /// 获得当前剩余加载任务总数
        /// </summary>
        private int GetTaskSum ()
        {
            int resu = 0;
            foreach (var loaders in loaderPrior)
                resu += loaders.Value.Count;
            return resu;
        }
    }
}