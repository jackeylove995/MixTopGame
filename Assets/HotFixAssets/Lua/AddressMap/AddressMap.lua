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
Sword1_Material_Bronze_mat = "Game/Art/Sword1/Sword1_Material_Bronze.mat"
Sword1_Emission_png = "Game/Art/Sword1/Textures/Sword1_Emission.png"
Sword1_Normal_png = "Game/Art/Sword1/Textures/Sword1_Normal.png"
Sword1_Albedo_Silver_png = "Game/Art/Sword1/Textures/Sword1_Albedo_Silver.png"
Sword1_Albedo_Bronze_png = "Game/Art/Sword1/Textures/Sword1_Albedo_Bronze.png"
Enemy_prefab = "Game/Enemy.prefab"
Sword1_Material_Gold_mat = "Game/Art/Sword1/Sword1_Material_Gold.mat"
Sword1_Material_Silver_mat = "Game/Art/Sword1/Sword1_Material_Silver.mat"
Sword1_Albedo_Gold_png = "Game/Art/Sword1/Textures/Sword1_Albedo_Gold.png"
Sword1_FBX_fbx = "Game/Art/Sword1/Sword1_FBX.fbx"
Sword1_Metallic_png = "Game/Art/Sword1/Textures/Sword1_Metallic.png"
Knight_idle_02_png = "Game/Art/PlayerAnim/Knight_idle_02.png"
Knight_idle_01_png = "Game/Art/PlayerAnim/Knight_idle_01.png"
Knight_idle_05_png = "Game/Art/PlayerAnim/Knight_idle_05.png"
Knight_idle_06_png = "Game/Art/PlayerAnim/Knight_idle_06.png"
Knight_idle_03_png = "Game/Art/PlayerAnim/Knight_idle_03.png"
Knight_idle_04_png = "Game/Art/PlayerAnim/Knight_idle_04.png"
Knight_walk_05_png = "Game/Art/PlayerAnim/Knight_walk_05.png"
Knight_walk_04_png = "Game/Art/PlayerAnim/Knight_walk_04.png"
Knight_walk_01_png = "Game/Art/PlayerAnim/Knight_walk_01.png"
Knight_walk_02_png = "Game/Art/PlayerAnim/Knight_walk_02.png"
Knight_walk_06_png = "Game/Art/PlayerAnim/Knight_walk_06.png"
Knight_walk_03_png = "Game/Art/PlayerAnim/Knight_walk_03.png"
player_idle_2_png = "Game/Art/EnemyAnim/player-idle-2.png"
player_idle_4_png = "Game/Art/EnemyAnim/player-idle-4.png"
player_idle_1_png = "Game/Art/EnemyAnim/player-idle-1.png"
player_idle_3_png = "Game/Art/EnemyAnim/player-idle-3.png"
player_run_1_png = "Game/Art/EnemyAnim/player-run-1.png"
player_run_6_png = "Game/Art/EnemyAnim/player-run-6.png"
player_run_5_png = "Game/Art/EnemyAnim/player-run-5.png"
player_run_2_png = "Game/Art/EnemyAnim/player-run-2.png"
player_run_4_png = "Game/Art/EnemyAnim/player-run-4.png"
player_run_3_png = "Game/Art/EnemyAnim/player-run-3.png"
player_hurt_2_png = "Game/Art/EnemyAnim/player-hurt-2.png"
player_hurt_1_png = "Game/Art/EnemyAnim/player-hurt-1.png"
Ball_prefab = "Game/Ball.prefab"
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
PlayerModel_lua = "Lua/Game/Model/PlayerModel.lua"
GameDataManager_lua = "Lua/Game/Model/GameDataManager.lua"
IFactory_lua = "Lua/AAAInit/IFactory.lua"
RolesConfig_lua = "Lua/Excels/RolesConfig.lua"
LevelConfig_Wave_lua = "Lua/Excels/LevelConfig_Wave.lua"
WeaponsConfig_lua = "Lua/Excels/WeaponsConfig.lua"
EnemyConfig_lua = "Lua/Excels/EnemyConfig.lua"
WaveModel_lua = "Lua/Game/Model/WaveModel.lua"
LevelModel_lua = "Lua/Game/Model/LevelModel.lua"
LevelConfig_EnemyIncrease_lua = "Lua/Excels/LevelConfig_EnemyIncrease.lua"
EnemyModel_lua = "Lua/Game/Model/EnemyModel.lua"
Enemy_lua = "Lua/Game/ViewController/Enemy.lua"
LevelConfig_Level_lua = "Lua/Excels/LevelConfig_Level.lua"
Ball_lua = "Lua/Game/ViewController/Ball.lua"
FramesAnimation_lua = "FramesAnimation/FramesAnimation.lua"
