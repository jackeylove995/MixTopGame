--- author:author
--- create:2025/1/12 20:18:09
--- desc: 

---@class Bar
local Bar = IOC.InjectClass(Bar_lua)

function Bar:OnGet(barModel)
    self.model = barModel
    self:SetValue(self.model.Value)
    self.Fill.color = self.model.Color
end

function Bar:SetValue(value)
    self.Slider.value = value
end

return Bar