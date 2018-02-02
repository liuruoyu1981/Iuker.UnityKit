using System;
using System.Collections.Generic;
using Iuker.Common.Base;
using Iuker.Common.DataTypes.ReactiveDatas;
using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.ReactiveDataModel.BaseModel
{
    /// <summary>
    /// 会话视图数据模型
    /// </summary>
#if DEBUG
    [ClassPurposeDesc("", "会话视图数据模型。")]
#endif
    public class DialogModel : IReactiveDataModel
    {
        #region 响应式数据字段

        /// <summary>
        /// 会话内容
        /// </summary>
#if DEBUG
        [FieldPurposeDesc("", "会话内容。")]
#endif
        public readonly ReactiveString Content = new ReactiveString();

        /// <summary>
        /// 是否显示取消按钮
        /// </summary>
#if DEBUG
        [FieldPurposeDesc("", "是否显示取消按钮。")]
#endif
        public readonly ReactiveBool IsShowCancel = new ReactiveBool();

        #endregion

        /// <summary>
        /// 确定按钮回调函数
        /// </summary>
        public Action EnsureAction { get; private set; }

        public List<int> ConcernedProtoIdList { get; private set; }
        public void Init(IU3dFrame frame)
        {
        }

        public void InitConcernedProtoIdList()
        {
        }

        public void OnNetResponse(object message)
        {
        }
    }
}