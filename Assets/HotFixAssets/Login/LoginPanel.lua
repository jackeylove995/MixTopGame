require(GameData_lua)


function Start()
    LoadGameObject(GamePanel_prefab, GlobalSetting.FullScreenPanelContainor)

    for k, v in pairs(package.preload) do
        Debug.Log(k .. " "..v)
    end
end

function onCreate(go, gamepanel)
    Debug.Log("load " .. go.name .. " success")
    Debug.ShowDebugMes("hotfix at 12/3")
end


