# MixTopGame

#热更及xLua使用说明

#lua脚本编写思路及函数访问规则
1.挂在LuaBehavior上的lua:
可视为一个Mono，可以在Lua脚本中调用Awake，Start，Update，OnDestroy等函数，实现见LuaBehavior.cs, local前缀为本脚本访问相当于private，不带local前缀为持有此lua对象的其他脚本也可以访问，相当于public

2.不挂在LuaBehavior上的lua:
不带local前缀为全局访问，可以在任意lua脚本中调用，但注意lua的调用先后顺序，写在前面的全局属性调用不到后面的，所以在项目启动时，初始化了所有全局属性，见init.lua

#项目内的HotFixAssets为可热更资源
凡是在里面创建的资源都会自动打Address标记，实现以及规则见AutoMarkAddress.cs

#项目工具类介绍
AddresableBuild.cs : 打包本地资源提交到远端
ApplicationBuild.cs : 打包
AutoMarkAddress.cs : 自动标记addresable的address，规则为HotFixAssets文件夹下，第一级文件夹作为address group，并标记文件夹下所有资源，但HotFixAssets下第一级资源不做标记
CreateLuaFile.cs : 创建Lua脚本，C#都可以，咱不能没有，整上
LuaImporter.cs : 导入.lua文件时，识别为TextAsset，否则Unity无法识别lua后缀资源
PathSetting.cs : 记录工具类需要的各种路径

#游戏实现思路
可以放在lua的逻辑一律放在lua