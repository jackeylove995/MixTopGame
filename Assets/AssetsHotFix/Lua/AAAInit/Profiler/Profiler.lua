--- author:author
--- create:2025/1/16 14:56:00
--- desc:
---@class Profiler
local Profiler = {}

function Profiler.Start(name)
    Profiler[name] = {
        startTime = CS.UnityEngine.Time.realtimeSinceStartup,
        parts = {},
        stopTime = 0
    }
end

function Profiler.Part(name, partName, logRightNow)
    local profiler = Profiler[name]
    local part = {
        name = partName,
        time = CS.UnityEngine.Time.realtimeSinceStartup
    }

    if logRightNow then
        local timeLength = -1
        local recardName
        if #profiler.parts > 0 then
            timeLength = part.time - profiler.parts[#profiler.parts].time
            recardName = profiler.parts[#profiler.parts].name .. " to " .. partName
        else
            timeLength = part.time - profiler.startTime
            recardName = "start to " .. partName
        end
        Profiler.log("[" .. name .. "] [" .. recardName .. "] cost time:" .. Profiler.timeDotStr(timeLength) .. "s")
    end

    table.insert(profiler.parts, part)
end

function Profiler.Stop(name)
    local profiler = Profiler[name]
    local stopTime = CS.UnityEngine.Time.realtimeSinceStartup
    local totalTime = stopTime - profiler.startTime
    local debugInfo = "[" .. name .. "] cost time:" .. Profiler.timeDotStr(totalTime) .. "s"

    if #profiler.parts > 0 then
        debugInfo = debugInfo .. "\n" .. "its parts infos below:"

        for index, part in ipairs(profiler.parts) do
            local lastPartName = index == 1 and "start" or profiler.parts[index-1].name
            local costTime = index == 1 and part.time - profiler.startTime or part.time - profiler.parts[index-1].time
            local costPercentStr = string.format("%.2f%%", costTime / totalTime * 100)
            debugInfo = debugInfo .. "\n" .."[" .. lastPartName .. " to " .. part.name .. "] : " .. Profiler.timeDotStr(costTime) .. " [" .. costPercentStr .. "]"
        end

        local costTime = stopTime - profiler.parts[#profiler.parts].time
        local costPercentStr = string.format("%.2f%%", costTime / totalTime * 100)
        debugInfo = debugInfo .. "\n" .. "[" .. profiler.parts[#profiler.parts].name .. " to end] : " .. Profiler.timeDotStr(costTime) .. " [" .. costPercentStr .. "]"
    end
    Profiler[name] = nil
    Profiler.log(debugInfo)
end

function Profiler.timeDotStr(timeNum)
    return string.format("%.5f", timeNum)
end

function Profiler.log(mes)
    print("[Profiler]" .. mes)
end

return Profiler
