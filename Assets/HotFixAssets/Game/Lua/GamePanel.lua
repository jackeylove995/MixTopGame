
function start()
    Debug.ShowDebugMes("enter game")
    UIUtil.Size(bg, 200, 200)
    DOTweenUtil.SetLoop(self:GetComponent("Image"):DOFade(0,1), -1)
end