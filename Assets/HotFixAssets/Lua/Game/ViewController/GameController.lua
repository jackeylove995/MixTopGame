--[[
    author:author
    create:2024/12/8 20:06:24
    desc: 游戏管理器
]]

local GameController = Class("GameController")

function GameController:OpenGame()
    Receive("PlayerMove", function(dir)
        self:OnPlayMove(dir)
    end)
    LoadGameObject(GamePanel_prefab, FullScreenPanelContainor, function(g, l)
        self:OnBgCreate(g , l)
    end)
    LoadGameObject(Player_prefab, Sprite3DContainor, function(g, l)
        self:OnPlayerCreate(g , l)
    end)
end

function GameController:OnGamePanelCreate(go, lua)
    
end

function GameController:OnBgCreate(g, l)
    
end

function GameController:OnPlayerCreate(go, lua)
    self.playergo = go
    self.playerlua = lua
    self.start = true
    UnityUtil.SetZ(go.transform, PlayerZDepth)
    FollowUtil.FollowTargetXY(TMainCamera, go.transform)
    lua:GenerateFlys(3)
end

function GameController:OnPlayMove(dir)    
    if self.start then
        self.playerlua:Move(dir)
    end
end

return GameController
