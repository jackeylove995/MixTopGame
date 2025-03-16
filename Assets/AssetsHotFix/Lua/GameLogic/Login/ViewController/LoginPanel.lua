local LoginPanel = IOC.InjectClass(LoginPanel_lua)

function LoginPanel:Start()
    AddListener(self.BtnBase, PackFunction(self,self.OnBtnStartClick))
end

function LoginPanel:OnBtnStartClick()   
    CS.UnityEngine.GameObject.Destroy(self.gameObject)
    IOC.LoadContainorWithScope(SelectDiffContainor_lua)
end

return LoginPanel

