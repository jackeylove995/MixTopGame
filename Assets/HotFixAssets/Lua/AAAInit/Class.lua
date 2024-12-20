--- author:author
--- create:2024/12/7 16:27:14
--- desc: 类方法

---@class Class
local classCache = {}
local maxClassCacheCount = 5

--- 声明一个Class
function Class(name, ...)
    local c = {}
    c.ClassName = name
    c.Dispose = function()
        c = nil
    end

    local isMono
    local implements = {...}
    if implements ~= nil then
        c.Implement = c.Implement or ""
        for i, implement in ipairs(implements) do
            if type(implement) == "string" then
                --表明是类地址
                local class = require(implement)
                c.Implement = c.Implement .. class.ClassName .. " " .. (class.Implement or "")  .. " "              
                setmetatable(c, {
                    __index = class
                })
            else
                --表明是table 
                c.Implement = c.Implement .. implement.ClassName .. " " .. (implement.Implement or "") .. " "               
                setmetatable(c, {
                    __index = implement
                })
            end
        end   
    end
    isMono = string.find(c.Implement, "MonoBehaviour") ~= nil
    -- Mono has no constructor
    if not isMono and rawget(c, "Constructor") == nil then
        c.Constructor = function()
        end
    end
    return c
end

--- 创建一个类对象，不要创建继承了MonoBehaviour的
function new(classAddress, ...)
    if #classCache > maxClassCacheCount then
        for k, v in pairs(classCache) do
            v = nil
        end
        classCache = nil
        classCache = {}
    end

    if rawget(classCache, classAddress) == nil then
        classCache.classAddress = require(classAddress)
    end
    local newItem = DeepCopy(classCache.classAddress)
    newItem:Constructor(...)
    return newItem
end

function IsMono(classAddress)
    local classEntity = require(classAddress)
    if classEntity.Implement ~= nil then
        return string.find(classEntity.Implement, "MonoBehaviour") ~= nil
    end
    return false
end

function GetClassNameByAddress(address)
    return table.Last(string.split(address, "/"))
end
