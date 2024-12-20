# MixTopGame

## 热更及xLua使用说明

### lua脚本编写思路及函数访问规则

#### 1.挂在LuaBehavior脚本上的lua:

LuaBehavior脚本将Lua脚本转换到Mono的使用方式
可以在Lua脚本中调用Awake，Start，Update，OnDestroy等函数

#### 2.不挂在LuaBehavior上的lua:

不带local前缀为全局访问，可以在任意lua脚本中调用，但注意lua的调用先后顺序，写在前面的全局属性调用不到后面的，所以在项目启动时，初始化了所有全局属性，见init.lua

#### 3.Lua讲究一个先来后到

如果不明白这个道理会出一些很奇怪的问题，例如

```
local funcA()
    funcB() --此写法是获取不到funcB的
end
function funcB() end

function funcA() end
local funcB()
    funcA()  --你过关
end
```

如果两个方法都在一个table里则不存在先后问题

#### 4.unity与lua实现对比

为了使Lua编写更符合Mono的习惯，添加了Class.lua，实现类的声明，继承，创建

Class("name", ...)
函数第一个为本身，第二个为继承谁, 此处可变参数可实现多种继承，传入lua地址或require后的table都可以
如果是挂在LuaBehaviour上的最终要继承Mono
如果是不挂在LuaBehaviour上的根据需求继承

##### 4.1 声明一个Mono类

```C#中为
public class Player : MonoBehaviour
{
    void Awake(){}
    void Start(){}
    void Update()
    {
        MyFunc();
    }
    void FixedUpdate(){}
    void OnDestroy(){}
    void MyFunc(){}
}
```

lua中为

```
local Player = Class("Player", MonoBehaviour.lua)
function Player:Awake() end
function Player:Start() end

function Player:Update()
    self:MyFunc()
end

function Player:FixedUpdate() end
function Player:OnDestroy() end
function Player:MyFunc() end

return Player
```


##### 4.2声明一个不继承Mono的类，例如Model层

C#为

```
public class PlayerModel
{
    public string name;
    public void Play()
    {
        Debug.Log(name);
    }
}
```

lua为

```
local PlayerModel = Class("PlayerModel")
PlayerModel.name = "name"

function PlayerModel:Play()
     Debug.Log(self.name)
end

return PlayerModel
```


#### 5.了解以上后，查看LuaBehaviour.cs脚本来看以上具体实现方式会快很多

#### 6.采用IOC(依赖注入容器)和Factory(工厂)联合搭建游戏框架

流程：编写IOC Install，通过BindClass绑定各种类，以及通过FromInstance，FromNewPrefab，FromFactory实现Inject时的方式
因为全部采用InjectClass注入Class，所以没有BindClass的类将无法调用，强行实现模块化

### Lua通过Addressable加载热更资源

#### 1.Lua加载GameObject或者图片

通过LuaCallCSharp直接访问AddressableAPI

#### 2.Lua加载其他Lua代码

通过lua提供的require方法加载
但是在编辑器中，require可以访问到工程文件，所以编辑器可用，但是到了真机上会发现加载不出来lua，这是因为手机里没有电脑的环境，lua使用require无法加载到Addressable中的资源，解决办法是添加Lua的CustomLoader，通过XLuaAPI的AddLoader方法，添加新的加载方法，在这个CustomLoader中传入AddressableKey，加载好Lua资源后返回byte[]即可，也就是require添加了Addressable的加载方式，最终还是addressable加载，详见LuaInitTask.cs

### 项目内的HotFixAssets为可热更资源

凡是在里面创建的资源都会自动打Address标记，实现以及规则见AutoMarkAddress.cs

### 项目编辑器工具类介绍

**AddresableBuild.cs**: 打包本地资源提交到远端
**ApplicationBuild.cs** : 打包
**AutoMarkAddress.cs** : 自动标记addresable的address，规则为HotFixAssets文件夹下，第一级文件夹作为address group，并标记文件夹下所有资源，但HotFixAssets下第一级资源不做标记
**CreateLuaFile.cs** : 创建Lua脚本，C#都可以，咱不能没有，整上
**LuaImporter.cs** : 导入.lua文件时，识别为TextAsset，否则Unity无法识别lua后缀资源
**PathSetting.cs** : 记录工具类需要的各种路径
**LuaInjectInspector** : Lua挂载选择Component功能

### 游戏实现思路

C#调用main.lua脚本，由main.lua实现lua系统和游戏的初始化
可以放在lua的逻辑一律放在lua

