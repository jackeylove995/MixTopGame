--- author:author
--- create:2025/2/23 13:02:04
--- desc: 

---@class SelectDiffPanel
local SelectDiffPanel = IOC.InjectClass(SelectDiffPanel_lua)

local Config = IOC.Inject(LevelConfig_Level_lua)

function SelectDiffPanel:Start()
    Receive(self, "OnGameLevelClick", function(param)
        AssetLoader.DestroyGameObject(self.gameObject)
        IOC.LoadContainorWithScope(GameContainor_lua, param.index)
    end)

    for i, v in ipairs(Config) do
        IOC.Inject(LevelItem_lua, {
            parent = self.Content,
            index = i,
            config = v,
            onClick = PackFunction(self, self.onItemClick)
        })
    end
end

function SelectDiffPanel:onItemClick(index)
    AssetLoader.DestroyGameObject(self.gameObject)
    IOC.LoadContainorWithScope(GameContainor_lua, self.index)
end

return SelectDiffPanel