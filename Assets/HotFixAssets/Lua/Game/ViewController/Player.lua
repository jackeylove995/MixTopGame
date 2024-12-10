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

function Player:GenerateFlys(count)
    for i = 1, count, 1 do
        LoadGameObject(Fly_prefab, self.FlyContainer, function(g, l)
            table.insert(flys, l)
            g.transform.localPosition = Vector3(1, 0 ,0)
            if #flys == count then
                canFly = true
            end
        end)
    end
end

function Player:FixedUpdate()
    if canFly then
        for i, v in ipairs(flys) do
            v.transform:LookAt(self.FlyContainer)
            v.transform.eulerAngles = Vector3(0, self.FlyContainer.eulerAngles.y, 0);
            v.transform:RotateAround(self.FlyContainer.position, Vector3(0, 0, 1), Time.fixedDeltaTime * 10)
        end
    end
end

return Player