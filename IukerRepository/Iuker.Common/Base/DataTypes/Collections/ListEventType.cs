namespace Iuker.Common.DataTypes.Collections
{
    /// <summary>
    /// 列表事件类型
    /// </summary>
    public enum ListEventType
    {
        OnAdd,

        OnRemove,

        OnInsert,

        /// <summary>
        /// 列表被清空
        /// </summary>
        OnClear,

        /// <summary>
        /// 列表整体被更新
        /// </summary>
        OnUpdate,
    }
}