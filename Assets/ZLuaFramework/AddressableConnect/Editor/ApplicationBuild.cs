using Process = System.Diagnostics.Process;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;

namespace ZLuaFramework
{
    /// <summary>
    /// build apk or ios project to <see cref="PathSetting.BuildApplicationPath"/> folder
    /// </summary>
    public class ApplicationBuild
    {
        public static string tag = "[ab] ";
        public static void Log(string message)
        {
            Debug.Log(tag + message);
        }

        public static void LogError(string message)
        {
            Debug.LogError(tag + message);
        }

        [MenuItem("ZLuaFramework/BuildApplication")]
        public static void BuildApplication()
        {
            BuildInternal(EditorUserBuildSettings.activeBuildTarget);
        }

        public static void BuildInternal(BuildTarget target)
        {
            try
            {
                var options = new BuildPlayerOptions
                {
                    locationPathName = GetLocalBuildPath(target),
                    scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(),
                    target = target,
                };
                var report = BuildPipeline.BuildPlayer(options);

                //Process.Start(PathSetting.BuildApplicationPath);
                Log("build completed location: " + GetLocalBuildPath(target));
                string adbCMD = "adb install " + GetLocalBuildPath(target);
                GUIUtility.systemCopyBuffer = adbCMD;
                Log("Text copied to clipboard: " + adbCMD);
            }catch(Exception e)
            {
                LogError(e.ToString());
            }       
        }

        public static string GetLocalBuildPath(BuildTarget target)
        {
            var targetPath = PathSetting.BuildApplicationPath;
            switch (target)
            {
                case BuildTarget.Android:
                    if (!targetPath.EndsWith(".apk"))
                        targetPath = Path.Combine(targetPath, "output.apk");
                    break;
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneOSX:
                    targetPath = Path.Combine(targetPath, PlayerSettings.productName);
                    break;
            }

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    targetPath += ".exe";
                    break;
            }

            return targetPath;
        }
    }

}
