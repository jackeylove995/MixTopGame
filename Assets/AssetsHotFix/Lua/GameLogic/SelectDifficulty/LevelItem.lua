--- author:author
--- create:2025/2/23 13:50:13
--- desc: 

---@class LevelItem
local LevelItem = IOC.InjectClass(LevelItem_lua)

function LevelItem:OnGet(param)
    self.transform:SetParent(param.parent)
    self.index = param.index
    self.LevelIndex.text = tostring(param.index)
    AddListener(self.Button, PackFunction(self, self.onBtnClick))
end

function LevelItem:onBtnClick()
    Push("OnGameLevelClick", {index = self.index})   
end

return LevelItem