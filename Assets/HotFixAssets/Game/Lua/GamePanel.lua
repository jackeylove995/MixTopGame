
tests = 8

function Start()
    Debug.ShowDebugMes("enter game")
    UIUtil.Size(bg, 200, 200)
    DOTweenUtil.SetLoop(self:GetComponent("Image"):DOFade(0,1), -1)
end

function asd()
    return 3
end