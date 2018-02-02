/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2017/11/16 下午 08:45:43 
/*  Email:          35490136@qq.com
/*  QQCode:         35490136
/*	Machine:		DESKTOP-M1OBR70
/*  CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
/* 	1. 修改日期： 修改人： 修改内容：
/* 	2. 修改日期： 修改人： 修改内容：
/* 	3. 修改日期： 修改人： 修改内容：
/* 	4. 修改日期： 修改人： 修改内容：
/* 	5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/


using System.IO;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;

namespace Iuker.UnityKit.Run.Base.Config
{
    /// <summary>
    /// Unity项目设置备份器
    /// </summary>
    public static class ProjectSettingsBacker
    {
        public static void Back(Project to)
        {
            if (RootConfig.Instance.AllProjects.Count == 1
                || to.ProjectName == RootConfig.GetCurrentProject().ProjectName)
            {
                return;
            }

            Back();
            if (Directory.Exists(to.ProjectSettingsBakDir))
            {
                FileUtility.DeleteDirectory(U3dConstants.ProjectSettingsDir);
                FileUtility.CopyDirectory(to.ProjectSettingsBakDir, U3dConstants.ProjectSettingsDir);
                Debug.Log("Unity项目设置已切换！");
            }
            else
            {
                FileUtility.CopyDirectory(U3dConstants.ProjectSettingsDir, to.ProjectSettingsBakDir);
                Debug.Log("Unity项目设置已切换！");
            }
        }

        public static void Back()
        {
            FileUtility.CopyDirectory(U3dConstants.ProjectSettingsDir,
                RootConfig.GetCurrentProject().ProjectSettingsBakDir, true);
            Debug.Log("Unity项目设置已备份！");
        }


















    }
}
