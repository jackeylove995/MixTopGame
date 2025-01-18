--------------------------------Start 自由设置全局变量------------------------------
-- 首先导入资源地址映射
require("Lua/AddressMap/AddressMap.lua")

-- 字符串
require(StringExtension_lua)
-- table操作
require(TableExtension_lua) 

local breakSocketHandle, debugXpCall = require(LuaDebug_lua)("localhost", 7003)
--------------------------------End 自由设置全局变量------------------------------


--------------------------------禁止自由设置全局变量，使用GlobalSetter规范化设置全局变量------------------------------
local GlobalSetter = require(GlobalSetter_lua)
GlobalSetter.BanSetGlobalValueDirectly()

-- 分析器
GlobalSetter.GlobalProperty.Profiler = require(Profiler_lua)
-- 工厂模式
GlobalSetter.GlobalProperty.Factory = require(Factory_lua)

-- DI框架
GlobalSetter.GlobalProperty.IOC = require(IOC_lua)
GlobalSetter.GlobalProperty.ContainorBuilder = require(ContainorBuilder_lua)

-- 全局工具，多数为C#
GlobalSetter.GlobalPropertiesFromTable.GlobalUtil = require(GlobalUtil_lua)
-- 全局方法      
GlobalSetter.GlobalPropertiesFromTable.GlobalFunc = require(GlobalFunc_lua)
-- 全局参数             
GlobalSetter.GlobalPropertiesFromTable.GlobalParams = require(GlobalParams_lua)
-- 类     
GlobalSetter.GlobalPropertiesFromTable.Class = require(Class_lua)

GlobalSetter.LogGlobalInfos()
GlobalSetter = nil
--------------------------------禁止设置全局变量------------------------------

-- 游戏生命周期，无APP OPEN，因为APP OPEN在C#
require(AppLifeScope_lua) 

--- lua 调试，调试时需要调用此方法
local function AttachToLua()
    MonoUtil.AddUpdate("LuaDebug", breakSocketHandle)
end
AttachToLua()

Log("on lua start")
Profiler.Start("start")
--- 进入游戏
Push("OnLuaStart")
Profiler.Stop("start")
