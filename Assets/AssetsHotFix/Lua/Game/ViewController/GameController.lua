--- author:author
--- create:2024/12/8 20:06:24
--- desc: 游戏管理器
local GameController = IOC.InjectClass(GameController_lua)

local GameDataManager = IOC.Inject(GameDataManager_lua)

function GameController:FixedUpdate()
    if self.enemys and self.mainPlayer then
        for i, v in ipairs(self.enemys) do
            v:MoveToPosition(self.mainPlayer.transform.position)
        end
    end
end

function GameController:OpenGame(levelId)
    MonoUtil.AddFixedUpdate("GameController", PackFunction(self, self.FixedUpdate))
    GameDataManager:SetLevel(levelId)
    self:InitJoyStick()
    self:InitPanel()
    self:InitPlayer()
    --self:StartWaveLoop()
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
    --- 先在列表中循环到这个ball
    --- 判断ball的左右各一个是否存在且符合条件
    --- 只有符合条件的才继续

    local clickList = {}
    local rightContinue = true
    local leftContinue = true

    table.insert(clickList, ball)
    for i, v in ipairs(self.balls) do
        if v == ball then
            local rightP = i + 1
            local leftP = i - 1
            while #clickList < 3 do

                if rightContinue and rawget(self.balls, rightP) and self.balls[rightP].model.Type == ball.model.Type then
                    table.insert(clickList, self.balls[rightP])
                else
                    rightContinue = false
                end

                if leftContinue and rawget(self.balls, leftP) and self.balls[leftP].model.Type == ball.model.Type then
                    table.insert(clickList, self.balls[leftP])
                else
                    leftContinue = false
                end

                if leftContinue then
                    leftP = leftP - 1
                end

                if rightContinue then
                    rightP = rightP + 1
                end

                if not leftContinue and not rightContinue then
                    break
                end
            end
        end
    end
    
    for i, v in ipairs(clickList) do
        Factory.Take(v)
        table.RemoveByObj(self.balls, v)
    end

    self.mainPlayer:OnBallsClick(clickList)

end


--- 初始化玩家
function GameController:InitPlayer()
    local mainPlayerModel = GameDataManager:GetPlayerModel()
    IOC.Inject(Role_lua, {
        parent = Sprite3DContainor,
        model = mainPlayerModel
    }, function(mainPlayer)
        self.mainPlayer = mainPlayer

        mainPlayer:BindBars()

        FollowUtil.FollowTargetXY(TMainCamera, mainPlayer.transform)
        -- 每过几秒生成一个球
        Clock.Name("CreateBall").FixTimeCall(3, true, function(count)
            local ballModel = self.mainPlayer.model:GetRandomBall()
            IOC.Inject(Ball_lua, {
                parent = self.gamePanel.BallContent,
                model = ballModel,
                clickFunc = PackFunction(self, self.OnBallClick)
            }, function(ball)
                self.balls = self.balls or {}               
                table.insert(self.balls, ball)
            end)
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
    enemy:BindBars()

    self.enemys = self.enemys or {}
    table.insert(self.enemys, enemy)
    if #self.enemys == self.enemyCount then
        Log("all enemy created")
    end
end

return GameController
