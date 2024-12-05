# MixTopGame

#热更及xLua使用说明

#lua脚本编写思路及函数访问规则
1.挂在LuaBehavior脚本上的lua:
LuaBehavior脚本将Lua脚本转换到Mono的使用方式
可以在Lua脚本中调用Awake，Start，Update，OnDestroy等函数， 并且脚本中可以通过self访问自身，可调用self.gameObject,self.transform.self:GetComponent()，

local前缀为本脚本访问相当于private，不带local前缀为持有此lua对象的其他脚本也可以访问，相当于public

2.不挂在LuaBehavior上的lua:
不带local前缀为全局访问，可以在任意lua脚本中调用，但注意lua的调用先后顺序，写在前面的全局属性调用不到后面的，所以在项目启动时，初始化了所有全局属性，见init.lua

3.Lua讲究一个先来后到
如果不明白这个道理会出一些很奇怪的问题，例如
local funcA() 
    funcB() --此写法是获取不到funcB的
end

local funcB() 
    funcA()  --你过关
end

所以在C#我们的方法在一个类的想怎么写怎么写，但是在Lua里，自定义方法要写在生命周期之前了，否则Start()是获取不到的

#项目内的HotFixAssets为可热更资源
凡是在里面创建的资源都会自动打Address标记，实现以及规则见AutoMarkAddress.cs

#项目编辑器工具类介绍
AddresableBuild.cs : 打包本地资源提交到远端
ApplicationBuild.cs : 打包
AutoMarkAddress.cs : 自动标记addresable的address，规则为HotFixAssets文件夹下，第一级文件夹作为address group，并标记文件夹下所有资源，但HotFixAssets下第一级资源不做标记
CreateLuaFile.cs : 创建Lua脚本，C#都可以，咱不能没有，整上
LuaImporter.cs : 导入.lua文件时，识别为TextAsset，否则Unity无法识别lua后缀资源
PathSetting.cs : 记录工具类需要的各种路径

#游戏实现思路
C#调用init.lua脚本，由init.lua实现lua系统和游戏的初始化
可以放在lua的逻辑一律放在lua