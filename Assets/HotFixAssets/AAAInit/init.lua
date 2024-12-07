
--首先导入资源地址映射
require("Assets.HotFixAssets.AddressMap.AddressMap")

require(StringExtension_lua)        --字符串
require(TableExtension_lua)         --table操作
require(GlobalUtil_lua)             --全局工具，多数为C#
require(GlobalFunc_lua)             --全局方法
require(Class_lua)                  --类


--local breakSocketHandle,debugXpCall = require(LuaDebug_lua)("localhost", 7003)
--MonoUtil.AddUpdate(breakSocketHandle)

Debug.Log("lua init success")

--- 进入游戏
local function Enter()
    LoadGameObject(LoginPanel_prefab, GlobalSetting.FullScreenPanelContainor)
end

Enter()