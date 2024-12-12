--[[
    author:author
    create:2024/12/7 20:35:32
    desc: 玩家控制
]]

--- Player
local Player = IOC.InjectClass(Player_lua)

local speed = 1
local speedExtra = 0.1
local flys = {}
local canFly

function Player:Move(x, y)
    UnityUtil.LocalMove(self.transform, x * speed * speedExtra , y * speed * speedExtra)
end

--- 生成飞行物
---@param count 数量
function Player:GenerateFlys(count)
    local everyAddEuler = 360 / count
    local distance = 1

    for i = 1, count, 1 do
        local hudu = (i* everyAddEuler* Math.PI) / 180
        local x = Math.Sin(hudu) * distance
        local y = Math.Cos(hudu) * distance

        local fly = IOC.InjectNew(Fly_lua, self.FlyContainer)
        table.insert(flys, fly)
        UnityUtil.SetLocalPosition(fly.transform, x, y, -1)
        -- 使物体的Y轴指向指定方向
        fly.transform.localRotation = Quaternion.Euler(0, 0, - i * everyAddEuler)
        canFly = true
    end
end

function Player:FixedUpdate()
    if canFly then
        self.FlyContainer:Rotate(0, 0, Time.fixedDeltaTime * 10)
    end
end

return Player