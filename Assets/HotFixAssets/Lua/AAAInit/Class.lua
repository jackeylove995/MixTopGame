--[[
    author:author
    create:2024/12/7 16:27:14
    desc: 类方法
]]

local classCache = {}
local maxClassCacheCount = 5

--- 声明一个Class
function Class(name, implement)
    local c = {}
    c.ClassName = name
    c.Dispose = function()
        c = nil
    end

    if implement ~= nil then
        c.Implement = table.last(string.split(implement, "."))
        setmetatable(c, { __index = require(implement) })   
    end
  
    return c
end

--- 创建一个类对象
function newClass(className)
    if #classCache > maxClassCacheCount then
        for k, v in pairs(classCache) do
            v = nil
        end
        classCache = nil
        classCache = {}
    end

    if classCache.className == nil then
        classCache.className = require(className)
    end

    return deepCopy(classCache.className)
end

-- 函数用于深拷贝一个 table
function deepCopy(original)
    local copy
    if type(original) == 'table' then
        copy = {}
        for k, v in next, original, nil do
            copy[deepCopy(k)] = deepCopy(v)
        end
        setmetatable(copy, deepCopy(getmetatable(original)))
    else
        copy = original
    end
    return copy
end
