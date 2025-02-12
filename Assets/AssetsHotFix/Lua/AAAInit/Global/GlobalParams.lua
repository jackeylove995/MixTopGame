--- author:author
--- create:2024/12/10 22:10:08
--- desc: 全局参数 
    
null = nil
local GameObject = CS.UnityEngine.GameObject
local Camera = CS.UnityEngine.Camera
UIRoot = UIRoot or GameObject.Find("UIRoot").transform;
FullScreenPanelContainor = FullScreenPanelContainor or GameObject.Find("FullScreenPanelContainor").transform;
PopupPanelContainor = PopupPanelContainor or GameObject.Find("PopupPanelContainor").transform;
Sprite3DContainor = Sprite3DContainor or GameObject.Find("Sprite3DContainor").transform;
TMainCamera = TMainCamera or Camera.main.transform;

PlayerZDepth = -2
FlyZDepth = -2


