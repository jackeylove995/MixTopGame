--[[
    author:author
    create:2024/12/5 21:11:38
    desc: 全局工具
]] --
-- 全局属性 
local globalUtil = {}
--[[LuaCallCSharp]] --
--- 日志
globalUtil.Debug = CS.UnityEngine.Debug
--- Dotween工具
globalUtil.DOTweenUtil = CS.MTG.DOTweenUtil
--- 资源加载器
globalUtil.AssetLoader = CS.MTG.AssetLoader
--- Unity基础方法工具
globalUtil.UnityUtil = CS.MTG.UnityUtil
--- Mono工具
globalUtil.MonoUtil = CS.MTG.MonoUtil
globalUtil.MonoUtil.Init()
--- 事件工具
globalUtil.EventUtil = CS.MTG.EventUtil
--- 跟随工具
globalUtil.FollowUtil = CS.MTG.FollowUtil
--- Vector3
globalUtil.Vector3 = CS.UnityEngine.Vector3
--- Time
globalUtil.Time = CS.UnityEngine.Time
--- 旋转
globalUtil.Quaternion = CS.UnityEngine.Quaternion
---钟表
globalUtil.Clock = CS.MTG.Clock
globalUtil.Clock.Init(globalUtil.Clock)

return globalUtil