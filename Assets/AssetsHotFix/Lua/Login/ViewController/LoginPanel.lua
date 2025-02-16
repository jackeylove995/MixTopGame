local LoginPanel = IOC.InjectClass(LoginPanel_lua)

local GameController = IOC.Inject(GameController_lua)

function LoginPanel:Start()
    self.Button.onClick:AddListener(PackFunction(self,self.OnBtnStartClick))
    self:OnBtnStartClick()
end

function LoginPanel:OnBtnStartClick()
    GameController:OpenGame(1)
    CS.UnityEngine.GameObject.Destroy(self.gameObject)
end

return LoginPanel

