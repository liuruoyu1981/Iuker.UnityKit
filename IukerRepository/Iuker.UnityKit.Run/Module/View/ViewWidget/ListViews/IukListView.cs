using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    public class IukListView : MonoBehaviour, IListView, IViewActionRequester<IListView>
    {
        #region 字段

        /// <summary>
        /// 当列表子项发生变更时用于绘制列表子项的委托。
        /// </summary>
        private Action<GameObject, int> mUpdateItem;

        /// <summary>
        /// 当Content的本地位置低于指定值时进行下拉刷新。
        /// </summary>
        [Tooltip("当Content的本地位置低于指定值时进行下拉刷新")]
        [SerializeField]
        private float pollDownUpdate = 100;

        /// <summary>
        /// 下拉更新委托。
        /// </summary>
        private Action m_PoollDownAction;

        /// <summary>
        /// 列表数据，可以是任意类型。
        /// </summary>
        public List<object> Datas { get; private set; }

        /// <summary>
        /// 列表组件所依附的视图
        /// </summary>
        public IView AttachView { get; private set; }

        /// <summary>
        /// 列表组件所依赖的游戏对象
        /// </summary>
        public GameObject DependentGo { get { return gameObject; } }

        /// <summary>
        /// 列表组件所在视图的游戏对象
        /// </summary>
        public GameObject ViewRoot { get { return AttachView.RectRoot.gameObject; } }

        /// <summary>
        /// 列表组件自己自身的全局唯一组件标识符
        /// </summary>
        public string WidgetToken { get { return ViewRoot.name + "_" + gameObject.name; } }

        /// <summary>
        /// 按钮组件所在视图的视图Id
        /// </summary>
        public string ViewId { get { return ViewRoot.name; } }

        /// <summary>
        /// 源组件（即列表组件自身）
        /// </summary>
        public IListView Origin { get { return this; } }

        /// <summary>
        /// 框架引用，列表组件初始化时由外部注入
        /// </summary>
        private IU3dFrame mFrame;

        /// <summary>
        /// 列表子项的宽
        /// </summary>
        private float ItemWidth = 200f;

        /// <summary>
        /// 列表子项的高
        /// </summary>
        private float ItemHeight = 200f;

        /// <summary>
        /// 列表的布局和排列方式，水平、垂直。
        /// </summary>
        [SerializeField]
        private Arrangement Arrangement = Arrangement.Vertical;

        /// <summary>
        /// 单位空间内（一行或者一列）最多允许容纳的子项数量
        /// </summary>
        [SerializeField]
        private int MaxPerLine = 2;

        /// <summary>
        /// 列表子项之间横向的间隔距离。
        /// </summary>
        [SerializeField]
        private float ItemWidthSpace = 10f;

        /// <summary>
        /// 列表子项直接垂直的间隔距离。
        /// </summary>
        [SerializeField]
        private float ItemHeightSpace = 10f;

        /// <summary>
        /// 需要创建的最少的列表组件数量（构成循环列表的最少数量）。
        /// </summary>
        [SerializeField]
        private int ViewCount = 5;

        /// <summary>
        /// 列表持有的滚动物体组件
        /// </summary>
        private ScrollRect mScrollRect;

        /// <summary>
        /// 列表子项所在的容器（展示内容的上级物体）物体。
        /// </summary>
        private RectTransform mContent;

        /// <summary>
        /// 列表子项的模板（预制件实例）。
        /// </summary>
        private GameObject mCellPrefab;

        /// <summary>
        /// 列表所使用的数据长度。
        /// </summary>
        private int mDataCount;

        /// <summary>
        /// 
        /// </summary>
        private int curScrollPerLineIndex = -1;

        /// <summary>
        /// 列表子项实例列表
        /// </summary>
        private readonly List<IukListViewItem> mListItems = new List<IukListViewItem>();

        /// <summary>
        /// 列表子项实例队列
        /// </summary>
        private readonly Queue<IukListViewItem> mUnUseItems = new Queue<IukListViewItem>();

        /// <summary>
        /// 列表子项的默认命名，该命名由外部传入。
        /// </summary>
        private string mItemName;

        #endregion

        #region Public Method

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="u3DFrame">框架实例</param>
        /// <param name="view">列表将要被置入其下的视图实例</param>
        /// <param name="fragment">列表将要被置入其下的视图碎片实例</param>
        /// <returns></returns>
        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            mFrame = u3DFrame;
            AttachView = view;

            return this;
        }

        /// <summary>
        /// mono组件初始化委托。
        /// </summary>
        private Action<GameObject> m_MonoInit;

        /// <summary>
        /// 设置滚动列表子项模板
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="updateItem"></param>
        /// <param name="itemName">可选的指定模板名</param>
        /// <param name="monoInitAction">mono组件初始化委托。</param>
        /// <param name="pollDownAction">下拉更新委托</param>
        public void SetItemTemplate(List<object> datas, Action<GameObject, int> updateItem = null,
            string itemName = "liveviewitem", Action<GameObject> monoInitAction = null, Action pollDownAction = null)
        {
            InitContext();
            mContent.gameObject.DeleteAllChild();

            if (datas == null || datas.Count == 0) return;

            m_MonoInit = monoInitAction;
            m_PoollDownAction = pollDownAction;
            Datas = datas;
            mUpdateItem = updateItem;
            mItemName = itemName;
            SetDataCount(datas.Count);
            mScrollRect.onValueChanged.RemoveAllListeners();
            mScrollRect.onValueChanged.AddListener(OnValueChanged);

            mUnUseItems.Clear();
            mListItems.Clear();

            UpdateActivedItem(0);
            InitMonos();
        }

        /// <summary>
        /// 初始化列表子项上可能存在的mono组件。
        /// </summary>
        private void InitMonos()
        {
            if (m_MonoInit == null) return;

            for (var i = 0; i < mContent.childCount; i++)
            {
                var child = mContent.GetChild(i);
                m_MonoInit(child.gameObject);
            }
        }

        private void OnValueChanged(Vector2 vt2)
        {
            CheckPollDownUpdate();

            switch (Arrangement)
            {
                case Arrangement.Vertical:
                    var y = vt2.y;
                    if (y >= 1.0f || y <= 0.0f)
                    {
                        return;
                    }
                    break;
                case Arrangement.Horizontal:
                    var x = vt2.x;
                    if (x <= 0.0f || x >= 1.0f)
                    {
                        return;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var _curScrollPerLineIndex = GetCurrentIndex();
            if (_curScrollPerLineIndex == curScrollPerLineIndex)
            {
                return;
            }
            UpdateActivedItem(_curScrollPerLineIndex);
        }

        private void CheckPollDownUpdate()
        {
            if (Arrangement != Arrangement.Vertical) return;
            if (!(mContent.localPosition.y < pollDownUpdate)) return;

            Debug.Log("下拉刷新！");
            if (m_PoollDownAction != null)
            {
                m_PoollDownAction();
            }
        }

        private bool mIsInited;

        private void InitContext()
        {
            if (mIsInited) return;

            var scrollbarVertical = transform.Find("Scroll View/Scrollbar Vertical");
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
                scrollbarVertical.gameObject.SetActive(false);
            }

            mCellPrefab = transform.Find("ListViewItemTemplate").gameObject;
            mCellPrefab.SetActive(false);
            var itemSize = mCellPrefab.GetComponent<RectTransform>().sizeDelta;
            ItemWidth = itemSize.x;
            ItemHeight = itemSize.y;
            mScrollRect = transform.Find("Scroll View").GetComponent<ScrollRect>();
            mContent = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

            mCellPrefab.SetActive(false);
            mIsInited = true;
        }

        /// <summary>
        /// 添加当前数据索引数据
        /// </summary>
        /// <param name="dataIndex"></param>
        public void AddItem(int dataIndex)
        {
            if (dataIndex < 0 || dataIndex > mDataCount)
            {
                return;
            }
            //检测是否需添加gameObject
            var isNeedAdd = false;
            for (var i = mListItems.Count - 1; i >= 0; i--)
            {
                var item = mListItems[i];
                if (item.DataIndex < mDataCount - 1) continue;
                isNeedAdd = true;
                break;
            }
            SetDataCount(mDataCount + 1);

            if (isNeedAdd)
            {
                foreach (var item in mListItems)
                {
                    var oldIndex = item.DataIndex;
                    if (oldIndex >= dataIndex)
                    {
                        UpdateItemPosition(oldIndex + 1, item);
                    }
                }
                UpdateActivedItem(GetCurrentIndex());
            }
            else
            {
                //重新刷新数据
                foreach (var item in mListItems)
                {
                    var oldIndex = item.DataIndex;
                    if (oldIndex >= dataIndex)
                    {
                        UpdateItemPosition(oldIndex, item);
                    }
                }
            }
        }

        /// <summary>
        /// 删除当前数据索引下的数据
        /// </summary>
        /// <param name="dataIndex"></param>
        public void DeleteItem(int dataIndex)
        {
            if (dataIndex < 0 || dataIndex >= mDataCount) return;

            //删除item逻辑三种情况
            //1.只更新数据，不销毁gameObject,也不移除gameobject
            //2.更新数据，且移除gameObject,不销毁gameObject
            //3.更新数据，销毁gameObject

            var isNeedDestroyGameObject = mListItems.Count >= mDataCount;
            SetDataCount(mDataCount - 1);

            for (var i = mListItems.Count - 1; i >= 0; i--)
            {
                var item = mListItems[i];
                var oldIndex = item.DataIndex;
                if (oldIndex == dataIndex)
                {
                    mListItems.Remove(item);
                    if (isNeedDestroyGameObject)
                    {
                        Destroy(item.gameObject);
                    }
                    else
                    {
                        UpdateItemPosition(-1, item);
                        mUnUseItems.Enqueue(item);
                    }
                }
                if (oldIndex > dataIndex)
                {
                    UpdateItemPosition(oldIndex - 1, item);
                }
            }
            UpdateActivedItem(GetCurrentIndex());
        }

        #endregion

        #region Private Method

        private void UpdateItemPosition(int index, IukListViewItem item)
        {
            item.DataIndex = index;
            item.Transform.localPosition = GetItemPosition(item.DataIndex);
            var itemIndexStr = item.DataIndex < 10 ? "0" + item.DataIndex : "" + item.DataIndex;
            item.name = mItemName + "_" + itemIndexStr + "_cell";
            if (mUpdateItem != null && item.DataIndex >= 0)
            {
                mUpdateItem(item.gameObject, item.DataIndex);
            }
        }

        /// <summary>
        /// 获得滚动列表子项的当前位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector3 GetItemPosition(int index)
        {
            float x, y;
            const float z = 0f;
            switch (Arrangement)
            {
                case Arrangement.Horizontal: //水平方向
                    x = (index / MaxPerLine) * (ItemWidth + ItemWidthSpace);
                    y = -(index % MaxPerLine) * (ItemHeight + ItemHeightSpace);
                    break;
                case Arrangement.Vertical://垂着方向
                    x = (index % MaxPerLine) * (ItemWidth + ItemWidthSpace);
                    y = -(index / MaxPerLine) * (ItemHeight + ItemHeightSpace);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new Vector3(x, y, z);
        }

        private void SetDataCount(int count)
        {
            if (mDataCount == count)
            {
                return;
            }
            mDataCount = count;
            SetUpdateContentSize();
        }


        private void SetUpdateContentSize()
        {
            var lineCount = Mathf.CeilToInt((float)mDataCount / MaxPerLine);
            switch (Arrangement)
            {
                case Arrangement.Horizontal:
                    mContent.sizeDelta = new Vector2(ItemWidth * lineCount + ItemWidthSpace * (lineCount - 1), mContent.sizeDelta.y);
                    break;
                case Arrangement.Vertical:
                    mContent.sizeDelta = new Vector2(mContent.sizeDelta.x, ItemHeight * lineCount + ItemHeightSpace * (lineCount - 1));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 更新当前激活的子项
        /// </summary>
        /// <param name="scrollPerLineIndex"></param>
        private void UpdateActivedItem(int scrollPerLineIndex)
        {
            if (scrollPerLineIndex < 0) return;

            curScrollPerLineIndex = scrollPerLineIndex;
            var startDataIndex = curScrollPerLineIndex * MaxPerLine;
            var endDataIndex = (curScrollPerLineIndex + ViewCount) * MaxPerLine;
            //移除
            for (var i = mListItems.Count - 1; i >= 0; i--)
            {
                var item = mListItems[i];
                var index = item.DataIndex;
                if (index >= startDataIndex && index < endDataIndex) continue;
                UpdateItemPosition(-1, item);
                mListItems.Remove(item);
                mUnUseItems.Enqueue(item);
            }
            //显示
            for (var dataIndex = startDataIndex; dataIndex < endDataIndex; dataIndex++)
            {
                if (dataIndex >= mDataCount || IsExist(dataIndex)) continue;
                CreateItem(dataIndex);
            }
        }

        private void CreateItem(int dataIndex)
        {
            var item = mUnUseItems.Count > 0 ? mUnUseItems.Dequeue() : mContent.AddPrefab(mCellPrefab).AddComponent<IukListViewItem>();
            item.SetListView(this);
            UpdateItemPosition(dataIndex, item);
            mListItems.Add(item);
        }

        /// <summary>
        /// 给定索引的列表子项是否存在于子项列表中
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        private bool IsExist(int dataIndex)
        {
            return mListItems.Any(t => t.DataIndex == dataIndex);
        }

        /// <summary>
        /// 根据Content偏移,计算当前开始显示所在数据列表中的行或列
        /// </summary>
        /// <returns></returns>
        private int GetCurrentIndex()
        {
            switch (Arrangement)
            {
                case Arrangement.Horizontal: //水平方向
                    return Mathf.FloorToInt(Mathf.Abs(mContent.anchoredPosition.x) / (ItemWidth + ItemWidthSpace));
                case Arrangement.Vertical://垂着方向
                    return Mathf.FloorToInt(Mathf.Abs(mContent.anchoredPosition.y) / (ItemHeight + ItemHeightSpace));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Issue(IViewActionRequest<IListView> request)
        {
            mFrame.ViewModule.ViewActionispatcher.DispatchRequest(request);
        }


        #endregion




    }
}



