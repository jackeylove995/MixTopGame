--- author:author
--- create:2024/12/11 15:54:55
--- desc: 

ContainorBuilder.NewContainorWithAddress(LoginContainor_lua)

--View
ContainorBuilder.BindMonoClass(LoginPanel_lua).FromNewPrefab(LoginPanel_prefab)

ContainorBuilder.BindStartMethod(function()
    IOC.Inject(LoginPanel_lua, FullScreenPanelContainor)
end)

ContainorBuilder.Build()



