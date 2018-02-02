namespace Iuker.Common.Base.Enums
{
    /// <summary>
    /// App�¼�����
    /// 1. App��������ѭ���¼���FixedUpdate Update LateUpdate OnGUI��
    /// 2. App�������ڻ����¼���AppStart,AppQuit,AppPause��
    /// </summary>
#if DEBUG
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 11:10:47")]
    [EmumPurposeDesc("App�¼����ͣ�ָ����ѭ��ģʽ��App���������¼���", "")]
#endif
    public enum AppEventType
    {
        /// <summary>
        /// �̶�ʱ��ѭ��
        /// </summary>
        FixedUpdate,

        /// <summary>
        /// ֡ѭ��
        /// </summary>
        Update,

        /// <summary>
        /// �ӳ�ѭ��
        /// </summary>
        LateUpdate,

        /// <summary>
        /// UI����ѭ��
        /// </summary>
        OnGUI,

        /// <summary>
        /// Ӧ�ÿ�ʼ
        /// </summary>
        AppStart,

        /// <summary>
        /// Ӧ���˳�
        /// </summary>
        AppQuit,

        /// <summary>
        /// Ӧ����ͣ
        /// </summary>
        AppPause,

        /// <summary>
        /// ʧȥ���ý���
        /// </summary>
        AppFocus,
    }
}
