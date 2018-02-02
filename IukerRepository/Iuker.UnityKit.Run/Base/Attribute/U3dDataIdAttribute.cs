namespace Iuker.UnityKit.Run.Base.Attribute
{
    public class U3dDataIdAttribute : System.Attribute
    {
        public string SonProject;

        public U3dDataIdAttribute(string sonProject)
        {
            SonProject = sonProject;
        }
    }
}