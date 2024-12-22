--- author:author
--- create:2024/12/11 15:54:55
--- desc: 

--Controller
IOC.BindClass(GameController_lua).FromInstance()
IOC.BindClass(GameDataManager_lua).FromInstance()

--View
IOC.BindMonoClass(LoginPanel_lua).FromNewPrefab(LoginPanel_prefab)
IOC.BindMonoClass(GamePanel_lua).FromNewPrefab(GamePanel_prefab).Sync()
IOC.BindMonoClass(Player_lua).FromFactory(Player_prefab)
IOC.BindMonoClass(Fly_lua).FromFactory(Fly_prefab)
IOC.BindMonoClass(Enemy_lua).FromFactory(Enemy_prefab)

--Model
IOC.BindClass(PlayerModel_lua).FromFactory()
IOC.BindClass(LevelModel_lua).FromFactory()
IOC.BindClass(WaveModel_lua).FromFactory()
IOC.BindClass(EnemyModel_lua).FromFactory()

--Config
IOC.BindInstanceByRequire(LevelConfig_lua)
IOC.BindInstanceByRequire(LevelConfig_Wave_lua)
IOC.BindInstanceByRequire(LevelConfig_EnemyIncrease_lua)
IOC.BindInstanceByRequire(EnemyConfig_lua)
IOC.BindInstanceByRequire(RolesConfig_lua)
IOC.BindInstanceByRequire(WeaponsConfig_lua)

IOC.BindStartMethod(function()
    IOC.Inject(LoginPanel_lua, FullScreenPanelContainor)
end)



