local LoginPanel = IOC.InjectClass(LoginPanel_lua)

function LoginPanel:Start()
    self.Button.onClick:AddListener(PackFunction(self,self.OnBtnStartClick))
    --self:OnBtnStartClick()
end

function LoginPanel:OnBtnStartClick()   
    CS.UnityEngine.GameObject.Destroy(self.gameObject)
    IOC.LoadContainorWithScope(SelectDiffContainor_lua)
end

return LoginPanel

