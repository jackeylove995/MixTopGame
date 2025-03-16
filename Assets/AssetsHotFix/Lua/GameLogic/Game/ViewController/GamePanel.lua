--- author:author
--- create:2024/12/5 21:11:38
--- desc: 游戏主页面

local GamePanel = IOC.InjectClass(GamePanel_lua)

function GamePanel:Start()
    Log("enter game")
end

function GamePanel:SetTime(timeStr)
    self.CountDown.text = timeStr
end

return GamePanel
