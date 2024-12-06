


function Start()
    LoadGameObject(GamePanel_prefab, GlobalSetting.FullScreenPanelContainor)
end

function onCreate(go, gamepanel)
    Debug.Log("load " .. go.name .. " success")
    Debug.ShowDebugMes("hotfix at 12/3")
end


