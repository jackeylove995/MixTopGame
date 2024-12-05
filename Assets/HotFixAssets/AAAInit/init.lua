
require("Assets.HotFixAssets.AddressMap.AddressMap")

--全局属性 LuaCallCSharp
Debug =  CS.MTG.DebugUtil
DOTweenUtil = CS.MTG.DOTweenUtil
AssetLoader = CS.MTG.AssetLoader
UIUtil = CS.MTG.UIUtil
GlobalSetting = CS.MTG.GlobalSetting
MonoUtil = CS.MTG.MonoUtil

--test

--

local breakSocketHandle,debugXpCall = require(LuaDebug_lua)("localhost", 19923)
MonoUtil.AddUpdate(breakSocketHandle)

Debug.Log("lua init success")

--- 创建GameObject
---@param address 资源在HotFixAssets文件夹下地址
---@param parent 父物体
---@param onCreate 创建成功携带GameObject的回调
function LoadGameObject(address, parent, onCreate)
    AssetLoader.LoadGameObject(address, parent, onCreate)
end

--- 初始化游戏环境
local function InitEnv()
    --全局设置
    GlobalSetting.InitSetting()
end

local function InitGame()
    LoadGameObject(LoginPanel_prefab, GlobalSetting.FullScreenPanelContainor)
end

InitEnv()
InitGame()