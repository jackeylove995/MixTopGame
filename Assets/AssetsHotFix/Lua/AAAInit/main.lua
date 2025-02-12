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
local GlobalSetManager = require(GlobalSetManager_lua)
GlobalSetManager.StartRecordGlobalInfos()

-- 分析器
require(Profiler_lua)
-- 工厂模式
require(Factory_lua)

-- DI框架
require(IOC_lua)
require(ContainorBuilder_lua)

-- 全局工具，多数为C#
require(GlobalUtil_lua)
-- 全局方法      
require(GlobalFunc_lua)
-- 全局参数             
require(GlobalParams_lua)
require(GlobalEnum_lua)
-- 类     
require(Class_lua)


GlobalSetManager.PrintGlobalInfos()
GlobalSetManager.BanSetGlobalValueDirectly()
GlobalSetManager = nil
--------------------------------禁止设置全局变量------------------------------

-- 游戏生命周期，无APP OPEN，因为APP OPEN在C#
require(AppLifeScope_lua) 

--- lua 调试，调试时需要调用此方法
local function AttachToLua()
    MonoUtil.AddUpdate("LuaDebug", breakSocketHandle)
end
AttachToLua()

Log("on lua start")

--- 进入游戏
Push("OnLuaStart")

