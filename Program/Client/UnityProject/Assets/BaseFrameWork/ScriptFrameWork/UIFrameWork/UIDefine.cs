using UnityEngine;
using System.Collections;

namespace XZXD.UI{
    public enum UILayerEnum
    {
		SCENE ,  // 场景层
        MAIN,       // 主层级
        GUIDER,     // 新手引导层
        ATTENTION,  // 提示层
        NETWORK,    // 网络层
        PLATFORM,   // 平台层(公告类似的使用到本地化的东西，都需要放入次层)

        COUNT
    }

    public class UIDefine
    {
        // 层级的名字
        public static readonly string[] UI_LAYER_NAME = new string[] { "SceneRoot", "MainRoot", "GuiderRoot", "AttentionRoot", "NetworkRoot","Platform" }; 

        // 基础depth
        public static readonly int[] UI_LAYER_DEPTH_BASE = new int[] { 100000, 200000, 300000, 400000, 500000,600000};

        // 每个大UIPanel的限制depth,每个UIPanel的层级控制在[0,1000)
        public static readonly int DEPTH_UI_MAX = 1000;

        // 内部动态的Panel增加值(要求每个大Panel内部，不应该超过10个内部Panel)
        public static readonly int INNER_PANEL_DEPTH = 100;

        // 当前每层的RootPanel达到某个值时，需要讲队列重新整理排序
        // 考虑sort order 以20增长，所以此处最大值为49，切勿超过此值
        public static readonly int CHECK_MAX_ROOT_PANEL_INDEX = 40;

        // 基础Z
		public static readonly int[] UI_LAYER_Z_BASE = new int[] { 0, 0, 0, 0, 0,0};

        //////// sort order//////////
        // 基础sort order
        public static readonly int[] UI_LAYER_ORDER_BASE = new int[]{1000,2000,3000,4000,5000,6000};

        // 每个大UIPanel的sort order的间隔
        // 每个UIPanel的sort order的范围[-9,9] 
        public static readonly int ORDER_UI_MAX = 20;
    }

    // 另外需要说明：
    // SceneRoot层的Postion的Z为5000 注意摄像机的照射距离需要调到[-20,20]
    // MainRoot层涉及到模型的，一般模型放入Z为500，背景Z为1000。MainRoot的Z控制在-1000~5000 [理论上可以达到5个模型层]

    // BoxCollider阻塞事件的问题:
    // 发现如果depth达到一定数值后，高层级阻塞不了低层级的BoxCollider
    // 大概测试了一下数值，depth达到2100000以上就会有问题，具体是多少以及为何原因，未知。
}

