using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Process = System.Diagnostics.Process;

namespace MTG
{
    public class AddressableBuild
    {    
        [MenuItem("MTG/Assets/OpenLocalServer")]
        public static void OpenLocalServer()
        {
            if (Directory.Exists(PathSetting.LocalServerPath))
            {
                Process.Start(PathSetting.LocalServerPath);
            }
        }

        [MenuItem("MTG/Assets/BuildLocalBundlesToNet")]
        public static void BuildLocalBundlesToNet()
        {
            //先删除本地存在的bundles
            if (Directory.Exists(PathSetting.LocalServerPath))
            {
                Directory.Delete(PathSetting.LocalServerPath, true);
            }
            AddressableAssetSettings.CleanPlayerContent();
            //打包新bundles
            AddressableAssetSettings.BuildPlayerContent();
            //拷贝本地bundles到远端
            CopyLocalBundlesToNet();
        }

        public static void CopyLocalBundlesToNet()
        {
            //string gitHttpConfig = "git config --global --unset http.proxy";

            if (!Directory.Exists(PathSetting.LocalGitPath))
            {
                Debug.LogError("git clone and retry");
                return;
                /*Directory.CreateDirectory(PathSetting.LocalGitPath);
                //本地远端仓库没有，那么先拉取创建
                string gitClone = string.Format($"git clone -b main {NetAssetsURL} {PathSetting.LocalGitPath}");
                RunCMD(new string[] { "git init", gitClone });*/
            }

            //如果本地有这个文件，那么直接提交 
            if (Directory.Exists(PathSetting.NetServerPath))
            {
                Directory.Delete(PathSetting.NetServerPath, true);
            }

            FileUtil.CopyFileOrDirectory(PathSetting.LocalServerPath, PathSetting.NetServerPath);

            string[] gitCommands = new string[]
            {
            "git add -A",
            "git commit -m \"0\"",
                //gitHttpConfig,
                //"git push"
            };
            RunCMD(gitCommands);
        }

        [MenuItem("MTG/Assets/Push")]
        public static void Push()
        {
            RunCMD(new string[] { "git push" });
        }

        public static void RunCMD(string[] inPut)
        {
            string outPut = "";
            string error = "";
            Process process = new Process();
            try
            {
                process.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
                process.StartInfo.CreateNoWindow = true;//是否在新窗口启动该进程的值(不显示窗口)
                process.StartInfo.RedirectStandardInput = true;//是否接收来自调用程序的输入信息
                process.StartInfo.RedirectStandardOutput = true;//是否可以由调用程序获取输出信息
                process.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.WorkingDirectory = PathSetting.LocalGitPath;
                process.Start();//启动

                StreamWriter stream = process.StandardInput;
                stream.AutoFlush = true;

                // streamWriter.WriteLine("rundll32.exe git-cmd.exe");  //启动git
                for (int i = 0; i < inPut.Length; i++)
                {
                    //stream.WriteAsync(inPut[i]).Wait();
                    stream.WriteLine(inPut[i]);
                    Debug.Log(inPut[i]);
                }

                //不管前一个命令是否执行成功都要执行exit，要不后面执行的ReadEnd()方法会假死
                stream.WriteLine("exit");

                stream.Close();//关闭

                outPut = process.StandardOutput.ReadToEnd();//获取所有的流输出
                error = process.StandardOutput.ReadToEnd();//获取所有的错误输出

                process.WaitForExit();//等待命令执行完之后退出
                process.Close();
            }
            catch
            {

            }

            /*if (!string.IsNullOrEmpty(outPut))
            {
                Debug.Log("outPut:" + outPut);
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.Log("error" + error);
            }*/
        }
    }

}
