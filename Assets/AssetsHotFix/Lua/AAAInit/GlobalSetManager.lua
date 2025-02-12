--- author:author
--- create:2025/1/18 15:32:07
--- desc: 
---@class GlobalSetManager
local GlobalSetManager = {}

GlobalSetManager.GlobalPropertyInfos = ""
GlobalSetManager.GlobalTablePropertyInfos = ""

function GlobalSetManager.BanSetGlobalValueDirectly()
    -- 设置全局环境表的元表
    setmetatable(_G, {
        __newindex = function(tbl, key, value)
            CS.UnityEngine.Debug.LogError("全局变量" .. key .. "需要在main的GlobalSetManager设定")
        end
    })
end

function GlobalSetManager.StartRecordGlobalInfos()
    setmetatable(_G, {
        __newindex = function(tbl, key, value)
            local type = type(value)
            if type == "table" then
                GlobalSetManager.GlobalTablePropertyInfos = GlobalSetManager.GlobalTablePropertyInfos .. "[" .. key .. "] type: ".. type .. "\n" 
            else
                GlobalSetManager.GlobalPropertyInfos = GlobalSetManager.GlobalPropertyInfos .. "[" .. key .. "] type: ".. type .. "\n"   
            end
            rawset(_G, key, value)
        end
    })
end

function GlobalSetManager.PrintGlobalInfos()
    -- 输出全局环境表的信息
    local logInfo = "[GlobalSetManager]全局属性信息\n\nglobal table as below:\n%s\nothers as below:\n%s"
    CS.UnityEngine.Debug.Log(string.format(logInfo, GlobalSetManager.GlobalTablePropertyInfos, GlobalSetManager.GlobalPropertyInfos))
end

return GlobalSetManager
