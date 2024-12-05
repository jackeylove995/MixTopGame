
local function onCreate(go, gamepanel)
    Debug.Log(gamepanel.tests)
    Debug.Log("load " .. go.name .. " success")
    Debug.ShowDebugMes("hotfix at 12/3")
end

function Start()
    Receive("game", dsadsa)
    Receive("close login", function()
        AssetLoader.DestroyGameObject(self)
    end)
    LoadGameObject(GamePanel_prefab, GlobalSetting.FullScreenPanelContainor, onCreate)
end

local function dsadsa(t)
    Debug.Log(t.a)
    Debug.Log(t.b)
end


