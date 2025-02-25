--- author:author
--- create:2025/2/23 14:35:15
--- desc: 

---@class SelectDiffContainor
ContainorBuilder.NewContainorWithAddress(SelectDiffContainor_lua)

ContainorBuilder.BindMonoClass(SelectDiffPanel_lua).FromNewPrefab(SelectDiffPanel_prefab)
ContainorBuilder.BindMonoClass(LevelItem_lua).FromFactory(LevelItem_prefab).Sync()

ContainorBuilder.BindInstanceByRequire(LevelConfig_Level_lua)

ContainorBuilder.BindStartMethod(function()
    IOC.Inject(SelectDiffPanel_lua, FullScreenPanelContainor)
end)

ContainorBuilder.Build()

