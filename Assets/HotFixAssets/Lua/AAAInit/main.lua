--首先导入资源地址映射
require("Lua/AddressMap/AddressMap.lua")

require(StringExtension_lua)        --字符串
require(TableExtension_lua)         --table操作
require(GlobalUtil_lua)             --全局工具，多数为C#
require(GlobalFunc_lua)             --全局方法
require(GlobalParams_lua)
require(Class_lua)                  --类
require(AppLifeScope_lua)           --游戏生命周期，无APP OPEN，因为还在C#

--- lua 调试，调试时需要调用此方法
local function AttachToLua()
    local breakSocketHandle,debugXpCall = require(LuaDebug_lua)("localhost", 7003)
    MonoUtil.AddUpdate(breakSocketHandle)
end
--AttachToLua()

Debug.Log("lua init success")

--- 进入游戏
Push("OnLuaStart")