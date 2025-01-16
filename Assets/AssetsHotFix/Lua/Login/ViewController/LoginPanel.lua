local LoginPanel = IOC.InjectClass(LoginPanel_lua)

local GameController = IOC.Inject(GameController_lua)

function LoginPanel:Start()
    self.Button.onClick:AddListener(function()
        GameController:OpenGame()
        CS.UnityEngine.GameObject.Destroy(self.gameObject)
    end)
    GameController:OpenGame()
    CS.UnityEngine.GameObject.Destroy(self.gameObject)
end

return LoginPanel

