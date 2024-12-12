local LoginPanel = IOC.InjectClass(LoginPanel_lua)

local GameController = IOC.Inject(GameController_lua)

function LoginPanel:Start()
    GameController:OpenGame()
end

return LoginPanel


