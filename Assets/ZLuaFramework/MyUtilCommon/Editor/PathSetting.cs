using System.IO;
using UnityEditor;
using UnityEngine;

namespace ZLuaFramework
{
    public static class PathSetting
    {
        /// <summary>
        /// 当前平台名称，Android， iOS, Window ， etc
        /// </summary>
        public static string PlatformName { get => EditorUserBuildSettings.activeBuildTarget.ToString(); }

        /// <summary>
        /// 本地资源服务器地址
        /// </summary>
        public static string LocalServerPath { get => Path.Combine(Application.dataPath.Replace("Assets", "ServerData"), PlatformName); }

        /// <summary>
        /// 工程的父节点路径
        /// </summary>
        public static string ProjectPath { get => Directory.GetParent(Application.dataPath).Parent.FullName; }

        /// <summary>
        /// 本地存放git net asset工程路径，和工程同级, 代表git资源的本地仓库
        /// </summary>
        public static string LocalGitPath { get => Path.Combine(ProjectPath, "MixTopGameNetAssets"); }

        /// <summary>
        /// 本地网络资源存放路径，代表git资源的本地仓库
        /// </summary>
        public static string NetServerPath { get => Path.Combine(ProjectPath, "MixTopGameNetAssets", PlatformName); }

        /// <summary>
        /// 网络资源路径
        /// </summary>
        public static string NetAssetsURL = "https://github.com/jackeylove995/MixTopGameNetAssets.git";

        /// <summary>
        /// 出包资源路径，为Assets同级的BuildOutput文件夹
        /// </summary>
        public static string BuildApplicationPath { get => Application.dataPath.Replace("Assets", "BuildOutput"); }

        /// <summary>
        /// 角色帧动画路径
        /// </summary>
        public static string RoleFrameAnimationSpritesPath = "Assets/AssetsDevelop/Sprites/Role";
    }
}

