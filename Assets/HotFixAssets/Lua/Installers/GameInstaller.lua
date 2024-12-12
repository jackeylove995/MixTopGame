--[[
    author:author
    create:2024/12/11 15:54:55
    desc: 
]]


IOC.BindMonoClass(LoginPanel_lua).FromNewPrefab(LoginPanel_prefab)
IOC.BindMonoClass(GamePanel_lua).FromNewPrefab(GamePanel_prefab)
IOC.BindMonoClass(Player_lua).FromNewPrefab(Player_prefab)
IOC.BindMonoClass(Fly_lua).FromFactory(Fly_prefab)

IOC.BindClass(GameController_lua).FromInstance()
IOC.BindClass(FlyModel_lua)

IOC.BindStartMethod(function()
    IOC.Inject(LoginPanel_lua, FullScreenPanelContainor)
end)



