--- author:author
--- create:2025/1/12 20:18:09
--- desc: 

---@class Bar
local Bar = IOC.InjectClass(Bar_lua)

function Bar:Init(barModel)
    self.model = barModel
    self:SetValue(self.model.Value)
    self.Fill.color = self.model.Color
end

function Bar:SetValue(value)
    self.Hp.value = value / self.model.MaxValue
end

return Bar