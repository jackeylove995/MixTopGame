--- author:author
--- create:2024/12/11 15:54:55
--- desc: 

ContainorBuilder.NewContainorWithAddress(GameContainor_lua)

--Controller
ContainorBuilder.BindClass(GameController_lua).FromInstance()
ContainorBuilder.BindClass(GameDataManager_lua).FromInstance()

--View
ContainorBuilder.BindMonoClass(LoginPanel_lua).FromNewPrefab(LoginPanel_prefab)
ContainorBuilder.BindMonoClass(GamePanel_lua).FromNewPrefab(GamePanel_prefab).Sync()
ContainorBuilder.BindMonoClass(Role_lua).FromFactory(Role_prefab)
ContainorBuilder.BindMonoClass(Fly_lua).FromFactory(Fly_prefab)
ContainorBuilder.BindMonoClass(Ball_lua).FromFactory(Ball_prefab)

--Model
ContainorBuilder.BindClass(RoleModel_lua).FromFactory()
ContainorBuilder.BindClass(LevelModel_lua).FromFactory()
ContainorBuilder.BindClass(WaveModel_lua).FromFactory()
ContainorBuilder.BindClass(BallModel_lua).FromFactory()

--Config
ContainorBuilder.BindReadOnlyConfig(LevelConfig_Level_lua)
ContainorBuilder.BindReadOnlyConfig(LevelConfig_Wave_lua)
ContainorBuilder.BindReadOnlyConfig(RolesConfig_lua)
ContainorBuilder.BindReadOnlyConfig(WeaponsConfig_lua)
ContainorBuilder.BindReadOnlyConfig(BallsConfig_lua)
ContainorBuilder.BindReadOnlyConfig(IncreaseConfig_lua)

ContainorBuilder.BindStartMethod(function()
    IOC.Inject(LoginPanel_lua, FullScreenPanelContainor)
end)

ContainorBuilder.BindItemsFromOtherContainorContent(BarContainor_lua)

ContainorBuilder.Build()



