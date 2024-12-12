/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System;
using UnityEngine;
using XLua;

namespace MTG
{
    [System.Serializable]
    public class Injection
    {
        public string name;
        public UnityEngine.Object value;
    }

    [LuaCallCSharp]
    public class LuaBehaviour : MonoBehaviour
    {
        public TextAsset luaScript;

        public Injection[] injections;

        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1; //1 second

        private Action<LuaTable> luaStart;
        private Action<LuaTable> luaUpdate;
        private Action<LuaTable> luaFixedUpdate;
        private Action<LuaTable> luaOnDestroy;

        //created new empty,set __index = __indexTable
        public LuaTable scriptTable { get; private set; } 

        //read .lua asset
        public LuaTable __indexTable { get; private set; }  

        private bool hasLuaUpdate;
        private bool hasLuaFixedUpdate;

        void Awake()
        {
            scriptTable = luaEnv.NewTable();

            __indexTable = luaEnv.DoString(luaScript.text, luaScript.name)[0] as LuaTable;
            if(__indexTable.Get<object>("isMonobehavior") == null)
            {
                Debug.LogError(luaScript.name + " binded must implement Monobehavior.lua");
            }

            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", __indexTable);
            scriptTable.SetMetaTable(meta);
            meta.Dispose();

            scriptTable.Set("self", this);
            scriptTable.Set("transform", transform);
            foreach (var injection in injections)
            {
                scriptTable.Set(injection.name, injection.value);
            }       
            scriptTable.Get("Awake", out Action<LuaTable> luaAwake);
            scriptTable.Get("Start", out luaStart);
            scriptTable.Get("Update", out luaUpdate);
            scriptTable.Get("FixedUpdate", out luaFixedUpdate);
            scriptTable.Get("OnDestroy", out luaOnDestroy);

            if (luaAwake != null)
            {
                luaAwake(scriptTable);
            }

            hasLuaUpdate = luaUpdate != null;
            hasLuaFixedUpdate = luaFixedUpdate != null;
        }

        // Use this for initialization
        void Start()
        {
            if (luaStart != null)
            {
                luaStart(scriptTable);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (hasLuaUpdate)
            {
                luaUpdate(scriptTable);
            }
            if (Time.time - LuaBehaviour.lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                LuaBehaviour.lastGCTime = Time.time;
            }
        }

        void FixedUpdate()
        {
            if (hasLuaFixedUpdate)
            {
                luaFixedUpdate(scriptTable);
            }
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy(scriptTable);
            }
            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            luaFixedUpdate = null;
            __indexTable.Dispose();
            scriptTable.Dispose();
            injections = null;
        }
    }
}
