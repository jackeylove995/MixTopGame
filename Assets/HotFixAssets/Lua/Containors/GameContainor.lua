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
ContainorBuilder.BindMonoClass(Player_lua).FromFactory(Player_prefab)
ContainorBuilder.BindMonoClass(Fly_lua).FromFactory(Fly_prefab)
ContainorBuilder.BindMonoClass(Enemy_lua).FromFactory(Enemy_prefab)
ContainorBuilder.BindMonoClass(Ball_lua).FromFactory(Ball_prefab)

--Model
ContainorBuilder.BindClass(PlayerModel_lua).FromFactory()
ContainorBuilder.BindClass(LevelModel_lua).FromFactory()
ContainorBuilder.BindClass(WaveModel_lua).FromFactory()
ContainorBuilder.BindClass(EnemyModel_lua).FromFactory()
ContainorBuilder.BindClass(BallModel_lua).FromFactory()

--Config
ContainorBuilder.BindInstanceByRequire(LevelConfig_Level_lua)
ContainorBuilder.BindInstanceByRequire(LevelConfig_Wave_lua)
ContainorBuilder.BindInstanceByRequire(LevelConfig_EnemyIncrease_lua)
ContainorBuilder.BindInstanceByRequire(EnemyConfig_lua)
ContainorBuilder.BindInstanceByRequire(RolesConfig_lua)
ContainorBuilder.BindInstanceByRequire(WeaponsConfig_lua)
ContainorBuilder.BindInstanceByRequire(BallsConfig_lua)

ContainorBuilder.BindStartMethod(function()
    IOC.Inject(LoginPanel_lua, FullScreenPanelContainor)
end)

ContainorBuilder.BindItemsFromOtherContainorContent(BarContainor_lua)

ContainorBuilder.Build()



