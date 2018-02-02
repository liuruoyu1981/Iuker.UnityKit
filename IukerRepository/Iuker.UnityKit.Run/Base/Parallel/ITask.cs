namespace Iuker.UnityKit.Run.Base.Parallel
{
    public interface ITask
    {
        void Start();

        TaskState State { get; }

        ITask ContinueTask { get; }
    }



}
