--- author:author
--- create:2024/12/7 19:37:32
--- desc: 游戏生命周期

--- Lua启动时，也是整个游戏正式开始时
local function OnLuaStart()
    IOC.LoadContainorWithScope(GameContainor_lua)
end


Receive("OnLuaStart", OnLuaStart)

