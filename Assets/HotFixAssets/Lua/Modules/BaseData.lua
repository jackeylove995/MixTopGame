--[[
    author:author
    create:2024/12/7 12:17:48
    desc: 
]]

local BaseData = Class("BaseData")

BaseData.level = "from base"

function BaseData:BaseMethod()
    --Debug.Log(self.level)
end

return BaseData