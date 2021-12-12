//using UnityEngine;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using System;
//
//namespace XZXD.UI
//{
//    public enum Orientation
//    {
//        Horizontal = 0,
//        Vertical = 1
//    }
//
//	public class UITable{
//		public Dictionary<string,object> caches = new Dictionary<string, object>();
//
//	}
//
//    /// <summary>
//    /// 无限拖拽列表返回给Lua的参数对象
//    /// </summary>
//    public class InfiniteLayoutCallBackParam
//    {
////		public UITable tblParts;
//        public int startIndex;
//        public int endIndex;
//    }
//
//    public class XZXDInfiniteItemLayout : MonoBehaviour
//    {
//        public ScrollRect scrollRect;
//        public Orientation orientation = Orientation.Vertical;
//
//        private RectTransform transScrollRect;
//        //一个元素的宽
//        public float cellSizeWidth = 50f;
//        //一个元素的高
//        public float cellSizeHeight = 50f;
//        //元素间高度的间隔
//        public float spacingHeight = 10;
//        //元素间宽度的间隔
//        public float spacingWidth = 10;
//        // 整体的偏移
//        public float paddingX = 0;
//        public float paddingY = 0;
//
//        //显示的行数或者列数
//        //当是 Horizontal时候 代表的是行
//        //当是 Vertical的时候代表的是列
//        public int nRawOrColumnNum = 1;
//        /// <summary>
//        /// 是否根据 大小控制个数
//        /// </summary>
//        public bool flexible = false;
//
//        //最多显示多少行 或列.当是Horizontal的时候代表的列
//        //当是vertical的时候代表的行
//        private int maxLine;
//
//        //布局中 要显示多少个元素
//        private int totalNum ;
//
//
//
//        //TransContent 的引用
//        private RectTransform transContent;
//
//        //scrollRect的高
//        private float scrollRectHeight;
//        //scrollRect的宽
//        private float scrollRectWidth;
//
//        //该ScrollRect 最多可以盛下多少行 或者多少列
//        private int showMaxRawOrColumnNum;
//
//        //正在使用的item
//        public List<RectTransform> items = new List<RectTransform>();
//
//        //为优化准备的参数
//        private int oldStartIndex = -1;
//        private int oldEndIndex = -1;
//
//        private Vector3 vector3Param = Vector3.zero;
//
//        //lua传过来的参数
//        private  GameObject objCopy;
//		private Action<InfiniteLayoutCallBackParam> funcCB;
//        private RectTransform rectCopy;
//        //优化用
//
//        //缓存一下传给Lua数据
////        private Dictionary<int, LuaPart> mCacheParts = new Dictionary<int, LuaPart>();
//
//
//        //Content 初始的位置
//        private Vector3 contentOriPos;
//
////        [Header("开头留余")]
////        public int leftAffluence = 0;
//
//        [Header("结尾留余N行、列")]
//        public int rightAffluence = 1;
//
//
//        //优化用数据
//        private int lastStartIndex = 0;
//        private int lastEndIndex = 0;
//        private Dictionary<int, RectTransform> hisTarget = new Dictionary<int, RectTransform>();
//
//        //返回给Lua的参数
//        private InfiniteLayoutCallBackParam callbackParam;
//
//        private void Awake()
//        {
//            if (scrollRect == null)
//            {
//                scrollRect = gameObject.GetComponent<ScrollRect>();
//            }
//            if (scrollRect == null)
//            {
//#if DEBUG && !PROFILER
//                Debug.LogError("cant find scrollRect");
//#endif
//
//            }
//            if (scrollRect != null && scrollRect.content != null)
//            {
//                transContent = scrollRect.content;
//                contentOriPos = transContent.anchoredPosition;
//            }
//
//            if(callbackParam == null)
//            {
//                callbackParam = new InfiniteLayoutCallBackParam();
//            }
//        }
//
//        /// <summary>
//        /// 设置 总共需要显示多少个 元素
//        /// </summary>
//        /// <param name="totalNum"></param>
//        void SetTotalNum(int totalNum)
//        {
//            this.totalNum = totalNum;
//            
//            Init();
//        }
//
//		public void SetParam(GameObject objCopy, Action<InfiniteLayoutCallBackParam> funcCallBack, int count)
//        {
//            this.objCopy = objCopy;
//            RectTransform transItem = objCopy.GetComponent<RectTransform>();
//            this.rectCopy = transItem;
//            this.funcCB = funcCallBack;
//            SetTotalNum(count);
//        }
//
//        /// <summary>
//        /// 刷新当前位置上显示的元素
//        /// </summary>
//        public void Refresh()
//        {
//            __RefreshHisIndex();
//            int currentIndex = getCurrentRowOrColumnIndex();
//            SetCurentStartLine(currentIndex, true);
//        }
//
//
//        /// <summary>
//        /// Init
//        /// </summary>
//        private void Init()
//        {
//            scrollRect.horizontal = orientation == Orientation.Horizontal;
//            scrollRect.vertical = orientation == Orientation.Vertical;
//
//            
//
//
//            transScrollRect = scrollRect.transform as RectTransform;
//            scrollRectHeight = transScrollRect.rect.height;
//            scrollRectWidth = transScrollRect.rect.width;
//
//            // 算出 ScrollRect 上最多显示多少行 或者多少列
//            //竖直方向滑动 
//            if (orientation == Orientation.Vertical)
//            {
//                showMaxRawOrColumnNum = Mathf.CeilToInt(Mathf.Abs(scrollRectHeight) / (cellSizeHeight + spacingHeight));
//            }
//            //水平方向上滑动
//            else if (orientation == Orientation.Horizontal)
//            {
//                showMaxRawOrColumnNum = Mathf.CeilToInt(Mathf.Abs(scrollRectWidth) / (cellSizeWidth + spacingWidth));
//            }
//
//            if(flexible)
//            {
//                float width = transContent.rect.width;
//                float height = transContent.rect.height; 
//                nRawOrColumnNum = orientation == Orientation.Vertical ? Mathf.FloorToInt(Mathf.Abs(width) / (cellSizeWidth + spacingWidth)) : Mathf.FloorToInt(Mathf.Abs(height) / (cellSizeHeight + spacingHeight));
//            }
//
//            this.maxLine = Mathf.CeilToInt(totalNum / (float)nRawOrColumnNum);
//
//            scrollRect.onValueChanged.RemoveAllListeners();
//            scrollRect.onValueChanged.AddListener(OnScrollRectValueChange);
//
//            SetContenSize();
//            //设置一下初始位置
//            Refresh();
//        }
//        /// <summary>
//        /// 更新显示的数据
//        /// </summary>
//        /// <param name="startLineIndex">开始的行数</param>
//        /// <param name="forceRefresh">是否强制刷新 </param>
//        private void SetCurentStartLine(int startLineIndex, bool forceRefresh)
//        {
//            if (startLineIndex < 0)
//            {
//#if DEBUG && !PROFILER
//                Debug.LogError("line index error " + startLineIndex.ToString());
//#endif
//
//                return;
//            }
//
//            /////////先求 出来 index的范围 作为以后显示的依据
//            int endLineIndex = startLineIndex + showMaxRawOrColumnNum + rightAffluence;
//            int startIndex = startLineIndex * nRawOrColumnNum ;
//            startIndex = startIndex < 0 ? 0 : startIndex;
//            int endIndex = endLineIndex * nRawOrColumnNum;
//            //在这做一下限制 不能超过 最多显示的个数
//            endIndex = endIndex > totalNum ? totalNum : endIndex;
//
//            //检测一下是否跟上次一样
//            if (startIndex == oldStartIndex && endIndex == oldEndIndex)
//            {
//                if (!forceRefresh)
//                {
//                    return;
//                }
//            }
//            else
//            {
//                MoveItems(startIndex > oldStartIndex);
//                oldStartIndex = startIndex;
//                oldEndIndex = endIndex;
//            }
//
//            // 设置Item 数据
//            int index = -1;
//            for (int i = startIndex; i < endIndex; i++)
//            {
//                index = i - startIndex;
//                if (index == items.Count)
//                {
//                    AddNewItem(index);
//                }
//                RectTransform rectTarget = items[index];
//                GameObject objItem = rectTarget.gameObject;
//                if (!objItem.activeSelf)
//                {
//                    objItem.SetActive(true);
//                }
//                SetPositionByIndex(rectTarget, i);
//  
//                if (__IsIndexNeedRefresh(i, rectTarget))
//                {
//                    hisTarget[i] = rectTarget;
//					#if UNITY_EDITOR
//					rectTarget.name = i.ToString ();
//					#endif
//					PartsLuaCall(i, i);
//                }
//            }
//
////            PartsLuaCall(startIndex, endIndex);
//
//            __SetHisIndex(startIndex, endIndex);
//            // 删除无用的Item
//            for (int i = items.Count - 1; i > index; i--)
//            {
//                DelItem(i);
//            }
//
//            
//        }
//
//        /// <summary>
//        /// 移动item是仅有首尾端产生移动
//        /// </summary>
//        /// <param name="direction">true forward false back</param>
//        private void MoveItems(bool direction)
//        {
//            if (items.Count >= nRawOrColumnNum)
//            {
//                RectTransform swapItem;
//                for (int i = 0; i < nRawOrColumnNum; i++)
//                {
//                    if (direction)
//                    {
//                        swapItem = items[i];
//                        for (int j = i; j < items.Count; j += nRawOrColumnNum)
//                        {
//                            var tmp = j + nRawOrColumnNum >= items.Count ? swapItem : items[j + nRawOrColumnNum];
//                            items[j] = tmp;
//                        }
//                    }
//                    else
//                    {
//                        swapItem = items[items.Count - nRawOrColumnNum + i];
//                        for (int j = items.Count - nRawOrColumnNum + i; j >= i; j -= nRawOrColumnNum)
//                        {
//                            var tmp = j - nRawOrColumnNum < i ? swapItem : items[j - nRawOrColumnNum];
//                            items[j] = tmp;
//                        }
//                    }
//                }
//            }
//        }
//
//        /// <summary>
//        /// 在可用的列表中增加一个元素
//        /// </summary>
//        private void AddNewItem(int index)
//        {
//            RectTransform transItem = null;
//            transItem = CreateNewItem(index);
//            items.Add(transItem);
//
//        }
//
//        private RectTransform CreateNewItem(int index)
//        {
//            GameObject objItem = GameObject.Instantiate(objCopy, transContent) as GameObject;
//            objItem.SetActive(true);
//            RectTransform transItem = objItem.GetComponent<RectTransform>();
//            if(!transItem)
//            {
//#if DEBUG && !PROFILER
//                Debug.LogError("cant find rectTransform");
//#endif
//
//            }
//            
//            //transItem.sizeDelta = new Vector2(cellSizeWidth, cellSizeHeight);
//            transItem.localScale = Vector3.one;
//
//            transItem.localScale = this.rectCopy.localScale;
//            transItem.anchorMin = this.rectCopy.anchorMin;
//            transItem.anchorMax = this.rectCopy.anchorMax;
//            transItem.pivot = this.rectCopy.pivot;
//            transItem.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSizeWidth);
//            transItem.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSizeHeight);
//            return transItem;
//        }
//
//        /// <summary>
//        /// 把一个元素从可用列表中删除
//        /// </summary>
//        /// <param name="index"></param>
//        private void DelItem(int index)
//        {
//            RectTransform transItem = items[index];
//            transItem.gameObject.SetActive(false);
//        }
//
//
//        private void PartsLuaCall(int startIndex, int endIndex)
//        {
//            callbackParam.startIndex = startIndex;
//            callbackParam.endIndex = endIndex;
//            this.funcCB(callbackParam);
//        }
//
//        /// <summary>
//        /// 根据  index 给transform的位置 赋值
//        /// </summary>
//        /// <param name="transItem"></param>
//        /// <param name="index"></param>
//        private void SetPositionByIndex(RectTransform transItem, int index)
//        {
//            float x, y, z = 0;
//            float xPivot = cellSizeWidth * transItem.pivot.x;
//            float yPivot = cellSizeHeight * (1 - transItem.pivot.y);
//            if (orientation == Orientation.Horizontal)
//            {
//                x = (index / nRawOrColumnNum) * (cellSizeWidth + spacingWidth);
//                y = -(index % nRawOrColumnNum) * (cellSizeHeight + spacingHeight);
//            }
//            else
//            {
//                x = (index % nRawOrColumnNum) * (cellSizeWidth + spacingWidth);
//                y = -(index / nRawOrColumnNum) * (cellSizeHeight + spacingHeight);
//            }
//            vector3Param.Set(x + paddingX + xPivot, y - paddingY - yPivot, z);
//
//            transItem.localPosition = vector3Param;
//            
//        }
//
//
//
//
//
//        /// <summary>
//        /// 当scrollView 滑动的回调
//        /// </summary>
//        /// <param name="normalizedPos"></param>
//        private void OnScrollRectValueChange(Vector2 normalizedPos)
//        {
//            int currentIndex = getCurrentRowOrColumnIndex();
//            SetCurentStartLine(currentIndex, false);
//
//        }
//        /// <summary>
//        /// 得到当前 开始的行或者列
//        /// </summary>
//        /// <returns>int 当前开始的行或者列</returns>
//        public int getCurrentRowOrColumnIndex()
//        {
//            if(this.maxLine == 0)
//            {
//                return 0;
//            }
//
//            if (orientation == Orientation.Horizontal)
//            {
//                //这样写是因为 当为负数时 有可能计算出来1 导致少显示的问题
//                //其实这样写 并不全面 但是现在由于 无限拖拽列表 只支持 左上。所以现在这么写并没有什么问题，之后支持了别的接续优化
//                float delta = transContent.anchoredPosition.x - contentOriPos.x;
//
//                float transcontentX = -delta < 0 ? 0 : transContent.anchoredPosition.x;
//                int index = Mathf.FloorToInt(Mathf.Abs(transcontentX) / (cellSizeWidth + spacingWidth));
//                // 如果是弹性的模式 有可能超过 最大列数 界限                
//                index = Mathf.Clamp(index, 0, this.maxLine - 1);
//                return index;
//            }
//            else if (orientation == Orientation.Vertical)
//            {
//                float delta = transContent.anchoredPosition.y - contentOriPos.y;
//                float transcontentY = delta < 0 ? 0 : transContent.anchoredPosition.y;
//                int index = Mathf.FloorToInt(Mathf.Abs(transcontentY) / (cellSizeHeight + spacingHeight));
//                index = Mathf.Clamp(index, 0, this.maxLine - 1);
//                return index;
//            }
//            return 0;
//        }
//
//
//        /// <summary>
//        /// 设置 conten的大小
//        /// </summary>
//        private void SetContenSize()
//        {
//            float x, y = 0;
//            if (orientation == Orientation.Vertical)
//            {
//                x = (nRawOrColumnNum * cellSizeWidth) + (nRawOrColumnNum - 1) * spacingWidth;
//                int rowNum = Mathf.CeilToInt((float)totalNum / nRawOrColumnNum);
//                y = (rowNum * cellSizeHeight) + (rowNum - 1) * spacingHeight;
//                if(!flexible)
//                {
//                    transContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
//                }
//                transContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
//            }
//            else
//            {
//                y = (nRawOrColumnNum * cellSizeHeight) + (nRawOrColumnNum - 1) * spacingHeight;
//                int columnNum = Mathf.CeilToInt((float)totalNum / nRawOrColumnNum);
//                x = (columnNum * cellSizeWidth) + (columnNum - 1) * spacingWidth;
//                if(!flexible)
//                {
//                    transContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
//                }
//                transContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
//
//            }
//
////            transContent.ForceUpdateRectTransforms();
//            scrollRect.Rebuild(CanvasUpdate.PostLayout);
//
//
//        }
//
//        private void __RefreshHisIndex()
//        {
//            lastStartIndex = -1;
//            lastEndIndex = -1;
//        }
//
//        private void __SetHisIndex(int startIndex, int endIndex)
//        {
//            lastStartIndex = startIndex;
//            lastEndIndex = endIndex;
//        }
//
//        private bool __IsIndexInHis(int index)
//        {
//
//
//            return index >= lastStartIndex && index < lastEndIndex;
//        }
//
//        private bool __IsIndexNeedRefresh(int index, RectTransform target)
//        {
//            if(!__IsIndexInHis(index))
//            {
//                return true;
//            }
//            RectTransform oldTarget = null;
//            if(hisTarget.TryGetValue(index, out oldTarget))
//            {
//                if(oldTarget == target)
//                {
//                    return false;
//                }
//                else
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//
//        void OnDestroy()
//        {
////            mCacheParts.Clear();
//        }
//    }
//}
