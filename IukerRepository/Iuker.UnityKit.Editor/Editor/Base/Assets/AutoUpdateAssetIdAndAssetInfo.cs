namespace Iuker.UnityKit.Editor.Assets
{
    public static class AutoUpdateAssetIdAndAssetInfo
    {
        private static bool sAutoBackTask;
        private static bool sAutoAssetId;

        public static void TryUpdate(string[] pathList)
        {
            //sAutoBackTask = IukerEditorPrefs.GetBool(IukerIl8nAndKey.AutoBackTaskKey);
            //sAutoAssetId = IukerEditorPrefs.GetBool(IukerIl8nAndKey.AutoAssetIdKey);

            //if (sAutoBackTask && sAutoAssetId)
            //{
            //    var sonProject = RootConfig.GetCurrentSonProject();
            //    var exists = pathList.ToList().Exists(s =>
            //    {
            //        var tempPath = s.Router("Assets/", "");
            //        var startMark = $"_{sonProject.ParentName}/{sonProject.CompexName}";
            //        if (tempPath.StartsWith(startMark))
            //        {
            //            if (tempPath.Contains("Ab"))
            //            {
            //                return true;
            //            }
            //        }
            //        return false;
            //    });

            //    if (exists)
            //    {
            //        AssetDatabase.Refresh();
            //        // 创建新的AssetId脚本
            //        //AssetBundleUtility.UpdateAssetInfoAndAssetIdScript();
            //    }
            //}
        }
    }
}