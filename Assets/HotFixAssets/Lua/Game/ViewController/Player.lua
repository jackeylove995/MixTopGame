--[[
    author:author
    create:2024/12/7 20:35:32
    desc: 玩家控制
]]

--- Player
local Player = Class("Player", MonoBehaviour_lua)

local speed = 1
local speedExtra = 0.1
local flys = {}
local canFly 

function Player:Move(dir)
    UnityUtil.LocalMove(self.transform, dir.x * speed * speedExtra , dir.y * speed * speedExtra)
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
        
        LoadGameObject(Fly_prefab, self.FlyContainer, function(g, l)
            table.insert(flys, l)
            UnityUtil.SetLocalPosition(g.transform, x, y, -1)

            -- 使物体的Y轴指向指定方向
            g.transform.localRotation = Quaternion.Euler(x, y, 0)

            if #flys == count then
                --canFly = true
            end
        end)
    end
end

function Player:FixedUpdate()
    if canFly then
        for i, v in ipairs(flys) do
            v.transform:RotateAround(self.FlyContainer.position, Vector3(0, 0, 1), Time.fixedDeltaTime * 10)
            -- 计算朝向指定方向的旋转
            --local targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        end
    end
end

return Player