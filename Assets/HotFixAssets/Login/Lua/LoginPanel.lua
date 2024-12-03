local ls = require(LevelSetting_lua)
function start()
    LoadGameObject(GamePanel_prefab, GlobalSetting.FullbackPanelContainor ,self.onCreate)
    Debug.Log("asd" .. ls.testa)
end

function onCreate(go)
    Debug.Log("load " .. go.name .. " success")
    Debug.ShowDebugMes("hotfix at 12/3")
end