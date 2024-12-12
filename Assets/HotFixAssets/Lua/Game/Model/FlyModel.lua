--[[
    author:author
    create:2024/12/9 20:43:30
    desc: 飞行物数据类
]]

local FlyModel = IOC.InjectClass("FlyModel")

function FlyModel:Constructor(flyType)
    self.flyType = flyType
end

return FlyModel