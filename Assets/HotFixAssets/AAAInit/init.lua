
--首先导入资源地址映射
require("Assets.HotFixAssets.AddressMap.AddressMap")

require(GlobalUtil_lua)
require(GlobalFunc_lua)

--local breakSocketHandle,debugXpCall = require(LuaDebug_lua)("localhost", 19923)
--MonoUtil.AddUpdate(breakSocketHandle)

Debug.Log("lua init success")

--- 进入游戏
local function Enter()
    LoadGameObject(LoginPanel_prefab, GlobalSetting.FullScreenPanelContainor)
end

Enter()