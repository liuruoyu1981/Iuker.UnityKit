/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/23 22:07:18
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System.Collections.Generic;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.Assist
{
    /// <summary>
    /// 视图辅助器
    /// 1. 可视化视图配置
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ViewAssister : MonoBehaviour
    {
        /// <summary>
        /// 将自身的视图配置属性更新到子项目视图配置文件中
        /// </summary>
        public void UpdateToViewConfig(SonProject son)
        {
            var newView = new Base.Config.View
            {
                ViewType = ViewType.ToString(),
                ViewId = ViewId,
                AssetName = AssetName,
                IsMain = IsMain,
                IsBlankClose = IsBlankClose,
                IsCloseTop = IsCloseTop,
                IsHideOther = IsHideOther,
                ParasiticList = ParasiticList
            };

            Views.Update(son, newView);
        }

        /// <summary>
        /// 设置视图的相关配置项
        /// </summary>
        public void SetViewConfig(string viewId, string viewAssetName, ViewType viewType, bool isClosetop, bool isHideOther, bool isBlankClose)
        {
            mViewId = viewId;
            mAssetName = viewAssetName;
            mViewType = viewType;
            mIsCloseTop = isClosetop;
            mIsBlankClose = isBlankClose;
            mIsHideOther = isHideOther;
        }

        [SerializeField]
        private string mViewId;

        [SerializeField]
        private List<string> mParasiticList;

        /// <summary>
        /// 视图的寄生视图列表
        /// </summary>
        public List<string> ParasiticList
        {
            get
            {
                return mParasiticList;
            }
        }

        /// <summary>
        /// 视图身份Id
        /// </summary>
        public string ViewId
        {
            get
            {
                return mViewId;
            }
        }

        [SerializeField] private bool mIsMain = false;

        /// <summary>
        /// 是否为主视图
        /// </summary>
        public bool IsMain
        {
            get
            {
                return mIsMain;
            }
        }

        [SerializeField]
        private string mAssetName;

        public string AssetName
        {
            get
            {
                return mAssetName;
            }
        }

        [SerializeField] private ViewType mViewType = ViewType.Normal;

        /// <summary>
        /// 视图类型
        /// 决定视图将会挂载在哪个视图根节点上
        /// </summary>
        public ViewType ViewType
        {
            get
            {
                return mViewType;
            }
        }

        [SerializeField]
        private bool mIsBlankClose;

        /// <summary>
        /// 视图是否需要支持点击视图空白处关闭自身
        /// </summary>
        public bool IsBlankClose
        {
            get
            {
                return mIsBlankClose;
            }
        }

        [SerializeField]
        private bool mIsHideOther;

        /// <summary>
        /// 视图打开时是否需要关闭自身视图类型视图栈中的其他视图
        /// </summary>
        public bool IsHideOther
        {
            get
            {
                return mIsHideOther;
            }
        }

        [SerializeField]
        private bool mIsCloseTop;

        /// <summary>
        /// 视图打开时是否需要关闭自身视图类型视图栈中居顶的视图
        /// </summary>
        public bool IsCloseTop
        {
            get
            {
                return mIsCloseTop;
            }
        }






    }
}