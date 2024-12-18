--- author:author
--- create:2024/12/8 20:06:24
--- desc: 游戏管理器

local GameController = IOC.InjectClass(GameController_lua)

local GameDataManager = IOC.Inject(GameDataManager_lua)

function GameController:OpenGame()
    
    CS.JoyStick.OnJoyStickMove("+", function(x, y)
        if self.mainPlayer then
            self.mainPlayer:Move(x, y)
        end
    end)

    IOC.Inject(GamePanel_lua, FullScreenPanelContainor)

    local mainPlayerData = GameDataManager.GetPlayerData()
    IOC.Inject(Player_lua, {parent = Sprite3DContainor, data = mainPlayerData}, function(mainPlayer)
        self.mainPlayer = mainPlayer
        FollowUtil.FollowTargetXY(TMainCamera, mainPlayer.transform)
    end)

    local botsData = GameDataManager.GetBotsData()
    for i, v in ipairs(botsData) do
        IOC.Inject(Player_lua,{ parent = Sprite3DContainor, data = v })
    end
end

return GameController
