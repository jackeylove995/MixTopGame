
--全局属性
Debug =  CS.MTG.DebugUtil
DOTweenUtil = CS.MTG.DOTweenUtil
GOLoader = CS.MTG.GOLoader
UIUtil = CS.MTG.UIUtil

Debug.Log("lua init success")

local function InitGame()
    GOLoader.LoadFullPanel("Login","LoginPanel")
end

InitGame()