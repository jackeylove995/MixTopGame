--Auto Generate
--生成全局资源映射，不用手写地址
--映射规则：
--key : 统一为 文件名_后缀，整个项目不要使用同名资源
--value : lua文件为路径用.分割，用以require，预制体路径用/分割，用来Addressables.LoadAsset
Resources = "Resources"
EditorSceneList = "EditorSceneList"
GamePanel_prefab = "Game/GamePanel.prefab"
Game_Bg_jpg = "Game/Art/Game_Bg.jpg"
Player_prefab = "Game/Player.prefab"
Circle_green_png = "Game/Art/Circle_green.png"
Circle_blue_full_png = "Game/Art/Circle_blue_full.png"
JoyStick_prefab = "Game/JoyStick.prefab"
Fly_prefab = "Game/Fly.prefab"
image_jpg = "Login/image.jpg"
login_bg_jpg = "Login/login_bg.jpg"
LoginPanel_prefab = "Login/LoginPanel.prefab"
bg3_jpg = "Login/bg3.jpg"
BasePanel_prefab = "AABase/BasePanel.prefab"
StringExtension_lua = "Lua/AAAInit/StringExtension.lua"
AppLifeScope_lua = "Lua/AAAInit/AppLifeScope.lua"
GlobalUtil_lua = "Lua/AAAInit/GlobalUtil.lua"
Class_lua = "Lua/AAAInit/Class.lua"
GlobalFunc_lua = "Lua/AAAInit/GlobalFunc.lua"
TableExtension_lua = "Lua/AAAInit/TableExtension.lua"
LuaDebug_lua = "Lua/AAAInit/LuaDebug.lua"
main_lua = "Lua/AAAInit/main.lua"
AddressMap_lua = "Lua/AddressMap/AddressMap.lua"
FlyModel_lua = "Lua/Game/Model/FlyModel.lua"
GamePanel_lua = "Lua/Game/ViewController/GamePanel.lua"
Player_lua = "Lua/Game/ViewController/Player.lua"
Fly_lua = "Lua/Game/ViewController/Fly.lua"
LoginPanel_lua = "Lua/Login/ViewController/LoginPanel.lua"
GameController_lua = "Lua/Game/ViewController/GameController.lua"
MonoBehaviour_lua = "Lua/AAAInit/MonoBehaviour.lua"
GlobalParams_lua = "Lua/AAAInit/GlobalParams.lua"
IOC_lua = "Lua/AAAInit/IOC.lua"
GameInstaller_lua = "Lua/Installers/GameInstaller.lua"
Factory_lua = "Lua/AAAInit/Factory.lua"
PlayerData_lua = "Lua/Game/Model/PlayerData.lua"
GameDataManager_lua = "Lua/Game/Model/GameDataManager.lua"
IFactory_lua = "Lua/AAAInit/IFactory.lua"
