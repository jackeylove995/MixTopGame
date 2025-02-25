--[[
    author:author
    create:2024/12/5 21:11:38
    desc: 全局工具
]] --
-- 全局属性 

--[[LuaCallCSharp]] --
--- 日志
Debug = CS.UnityEngine.Debug
--- Dotween工具
DOTweenUtil = CS.MTG.DOTweenUtil
--- 资源加载器
AssetLoader = CS.MTG.AssetLoader
--- Unity基础方法工具
UnityUtil = CS.MTG.UnityUtil
--- Mono工具
MonoUtil = CS.MTG.MonoUtil
MonoUtil.Init()
--- 事件工具
EventUtil = CS.MTG.EventUtil
--- 跟随工具
FollowUtil = CS.MTG.FollowUtil
--- Vector3
Vector3 = CS.UnityEngine.Vector3
--- Time
Time = CS.UnityEngine.Time
--- 旋转
Quaternion = CS.UnityEngine.Quaternion
---钟表
Clock = CS.MTG.Clock
Clock.Init(Clock)
--颜色
Color = CS.UnityEngine.Color
--多语言
LocalizationManager = CS.ZLuaFramework.LocalizationManager
