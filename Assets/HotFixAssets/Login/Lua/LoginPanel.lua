local LoginPanel = {}

function Start()
    LoadGameObject(GamePanel_prefab, GlobalSetting.FullbackPanelContainor, onCreate)
end

function onCreate(go, gamepanel)
    Debug.Log(gamepanel.tests)
    Debug.Log("load " .. go.name .. " success")
    Debug.ShowDebugMes("hotfix at 12/3")
end
