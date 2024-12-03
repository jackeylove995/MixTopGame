local ls = require "HotFixAssets.LevelSetting.Lua.LevelSetting"

local function start()
    LoadGameObject("GamePanel", GlobalSetting.FullbackPanelContainor ,self.onCreate)
    Debug.Log("asd" .. ls.testa)
end

local function onCreate(go)
    Debug.Log("load " .. go.name .. " success")
    Debug.ShowDebugMes("hotfix at 12/3")
end