local LoginPanel = Class("LoginPanel", MonoBehaviour_lua)

function LoginPanel:Start()
    self:LoginSuccess()
end


function LoginPanel:LoginSuccess()
    newClass(GameController_lua):OpenGame()
end

return LoginPanel


