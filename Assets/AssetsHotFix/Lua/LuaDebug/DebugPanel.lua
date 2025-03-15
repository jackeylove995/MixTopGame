--- author:author
--- create:2025/3/15 18:35:46
--- desc: 

---@class DebugPanel
local DebugPanel = Class(DebugPanel_lua, MonoBehaviour_lua)
local PlayerPrefs = CS.UnityEngine.PlayerPrefs
local IsLuaDebugKey = "IsLuaDebug"
local breakSocketHandle, debugXpCall = require(LuaDebug_lua)("localhost", 7003)


function DebugPanel:Start()
    AddListener(self.BtnLuaDebug, PackFunction(self, self.onBtnDebugClick))    
    self.isLuaDebug = PlayerPrefs.GetInt(IsLuaDebugKey, 0) == 1
    self:updateLuaDebug()
end

function DebugPanel:Update()
    if self.isLuaDebug then
        breakSocketHandle()
    end
end

function DebugPanel:onBtnDebugClick()
    self.isLuaDebug = not self.isLuaDebug
    PlayerPrefs.SetInt(IsLuaDebugKey, self.isLuaDebug and 1 or 0)
    self:updateLuaDebug()
end

function DebugPanel:updateLuaDebug()
    self.TxtLuaDebug.text = self.isLuaDebug and "Lua调试中" or "开启Lua调试"
    self.ImgBtnDebug.color = self.isLuaDebug and Color.green or Color.white
end

return DebugPanel