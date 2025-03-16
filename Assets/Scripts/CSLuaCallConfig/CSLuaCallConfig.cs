
using Cinemachine;
using System;
using System.Collections.Generic;
using XLua;
using ZLuaFramework;

namespace MTG
{
    public static class CSLuaCallConfig
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(AssetLoader),
            typeof(DOTweenUtil),
            typeof(EventUtil),
            typeof(MonoUtil),
            typeof(UnityUtil),
            typeof(JoyStick),
            typeof(Clock),
            typeof(LocalizationManager),
            typeof(LensSettings),
            typeof(Cinemachine3rdPersonFollow),
            typeof(CinemachineVirtualCamera)
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(Action<float, float>),
            typeof(Action<float>),
            typeof(Action<int>)
        };

    }
}

