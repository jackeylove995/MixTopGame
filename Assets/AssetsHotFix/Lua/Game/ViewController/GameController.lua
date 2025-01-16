--- author:author
--- create:2024/12/8 20:06:24
--- desc: 游戏管理器
local GameController = IOC.InjectClass(GameController_lua)

local GameDataManager = IOC.Inject(GameDataManager_lua)

function GameController:FixedUpdate()
    if self.enemys and self.mainPlayer then
        for i, v in ipairs(self.enemys) do
            v:MoveToPosition(self.mainPlayer.position)
        end
    end
end

function GameController:OpenGame()
    MonoUtil.AddFixedUpdate("GameController", PackFunction(self, self.FixedUpdate))
    self:InitJoyStick()
    self:InitPanel()
    self:InitPlayer()
    self:StartWaveLoop()
end

--- 初始化摇杆
function GameController:InitJoyStick()
    CS.JoyStick.OnMove("+", function(x, y)
        if self.mainPlayer then
            self.mainPlayer:Direction(x, y)
        end
    end)

    CS.JoyStick.OnEndMove("+", function()
        if self.mainPlayer then
            self.mainPlayer:Direction(0, 0)
        end
    end)
end

--- 初始化游戏Panel
function GameController:InitPanel()
    self.gamePanel = IOC.Inject(GamePanel_lua, FullScreenPanelContainor)
end

function GameController:OnBallClick(ball)
    Factory.Take(ball)
    self.mainPlayer:CreateFly()
end


--- 初始化玩家
function GameController:InitPlayer()
    local mainPlayerModel = GameDataManager:GetPlayerModel()
    IOC.Inject(Role_lua, {
        parent = Sprite3DContainor,
        model = mainPlayerModel
    }, function(mainPlayer)
        self.mainPlayer = mainPlayer
        FollowUtil.FollowTargetXY(TMainCamera, mainPlayer.transform)
        -- 每过几秒生成一个球
        Clock.Name("CreateBall").FixTimeCall(3, true, function(count)
            local ballModel = self.mainPlayer.model:GetRandomBall()
            IOC.Inject(Ball_lua, {
                parent = self.gamePanel.BallContent,
                model = ballModel,
                clickFunc = PackFunction(self, self.OnBallClick)
            })
        end)
    end)
end

--- 开始关卡波次轮替
function GameController:StartWaveLoop()
    if not GameDataManager:ToNextWave() then
        print("Game Over")
        return
    end

    -- 生成enemy
    local enemyModels = GameDataManager:GetCurrentWaveEnemyModels()
    self.enemyCount = #enemyModels
    for i, v in ipairs(enemyModels) do
        IOC.Inject(Role_lua, {
            parent = Sprite3DContainor,
            model = v
        }, PackFunction(self, self.OnEnemyCreate))
    end

    local timeToNext = GameDataManager:GetTimeToNext()
    Clock.StartTimer(timeToNext, 0, -1, function(t)
        self.time = t
        self.gamePanel:SetTime("距离下一波次还有"..tostring(t).."秒")
        if self.time and self.time == 0 then
            self:StartWaveLoop()
        end
    end)
end

function GameController:CheckWaveOver()

end

function GameController:OnEnemyCreate(enemy)
    self.enemys = self.enemys or {}
    table.insert(self.enemys, enemy)
    if #self.enemys == self.enemyCount then
        Log("all enemy created")
    end
end

return GameController
