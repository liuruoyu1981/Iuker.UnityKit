//using System.Collections.Generic;
//using Iuker.UnityKit.Run.Base.Config;
//using Iuker.UnityKit.Run.Base.Config.Develop;

//namespace Iuker.UnityKit.Run.Base
//{
//    /// <summary>
//    /// Unity应用启动设置
//    /// </summary>
//    public class BootstrapOption
//    {
//        public Project MainProject { get; private set; }

//        private readonly List<string> projectNames = new List<string>();

//        public BootstrapOption AddProject(params string[] ps)
//        {
//            projectNames.AddRange(ps);
//            return this;
//        }

//        private List<Project> projects;
//        public List<Project> Projects
//        {
//            get
//            {
//                if (projects != null) return projects;

//                projects = new List<Project>();
//                foreach (var name in projectNames)
//                {
//                    var project = RootConfig.Instance.AllProjects.Find(p => p.ProjectName == name);
//                    projects.Add(project);
//                }

//                return projects;
//            }
//        }

//        public bool AnchorAlignment { get; private set; }

//        private List<SonProject> sons;
//        public List<SonProject> Sons
//        {
//            get
//            {
//                if (sons != null) return sons;

//                sons = new List<SonProject>();

//                foreach (var project in Projects)
//                {
//                    foreach (var son in project.AllSonProjects)
//                    {
//                        sons.Add(son);
//                    }
//                }

//                return sons;
//            }
//        }


//    }
//}