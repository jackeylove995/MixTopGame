function start()
    GOLoader.LoadFullPanel("Game", "GamePanel", onCreate)
end

function onCreate(go)
    Debug.Log("load " .. go.name .. " success")
    Debug.ShowDebugMes("hotfix at 12/3")
end