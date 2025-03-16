#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class CinemachineCinemachine3rdPersonFollowWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Cinemachine.Cinemachine3rdPersonFollow);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 12, 10);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMaxDampTime", _m_GetMaxDampTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MutateCameraState", _m_MutateCameraState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTargetObjectWarped", _m_OnTargetObjectWarped);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRigPositions", _m_GetRigPositions);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsValid", _g_get_IsValid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Stage", _g_get_Stage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Damping", _g_get_Damping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ShoulderOffset", _g_get_ShoulderOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "VerticalArmLength", _g_get_VerticalArmLength);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CameraSide", _g_get_CameraSide);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CameraDistance", _g_get_CameraDistance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CameraCollisionFilter", _g_get_CameraCollisionFilter);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IgnoreTag", _g_get_IgnoreTag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CameraRadius", _g_get_CameraRadius);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DampingIntoCollision", _g_get_DampingIntoCollision);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DampingFromCollision", _g_get_DampingFromCollision);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Damping", _s_set_Damping);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ShoulderOffset", _s_set_ShoulderOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "VerticalArmLength", _s_set_VerticalArmLength);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CameraSide", _s_set_CameraSide);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CameraDistance", _s_set_CameraDistance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CameraCollisionFilter", _s_set_CameraCollisionFilter);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IgnoreTag", _s_set_IgnoreTag);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CameraRadius", _s_set_CameraRadius);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DampingIntoCollision", _s_set_DampingIntoCollision);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DampingFromCollision", _s_set_DampingFromCollision);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Cinemachine.Cinemachine3rdPersonFollow();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Cinemachine.Cinemachine3rdPersonFollow constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMaxDampTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMaxDampTime(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MutateCameraState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Cinemachine.CameraState _curState;translator.Get(L, 2, out _curState);
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.MutateCameraState( ref _curState, _deltaTime );
                    translator.Push(L, _curState);
                        translator.Update(L, 2, _curState);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnTargetObjectWarped(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _positionDelta;translator.Get(L, 3, out _positionDelta);
                    
                    gen_to_be_invoked.OnTargetObjectWarped( _target, _positionDelta );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRigPositions(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _root;
                    UnityEngine.Vector3 _shoulder;
                    UnityEngine.Vector3 _hand;
                    
                    gen_to_be_invoked.GetRigPositions( out _root, out _shoulder, out _hand );
                    translator.PushUnityEngineVector3(L, _root);
                        
                    translator.PushUnityEngineVector3(L, _shoulder);
                        
                    translator.PushUnityEngineVector3(L, _hand);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsValid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsValid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Stage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Stage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Damping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.Damping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ShoulderOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.ShoulderOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VerticalArmLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.VerticalArmLength);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CameraSide(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CameraSide);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CameraDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CameraDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CameraCollisionFilter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CameraCollisionFilter);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IgnoreTag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.IgnoreTag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CameraRadius(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CameraRadius);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DampingIntoCollision(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DampingIntoCollision);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DampingFromCollision(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DampingFromCollision);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Damping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Damping = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ShoulderOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.ShoulderOffset = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_VerticalArmLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.VerticalArmLength = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CameraSide(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CameraSide = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CameraDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CameraDistance = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CameraCollisionFilter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                UnityEngine.LayerMask gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.CameraCollisionFilter = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IgnoreTag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IgnoreTag = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CameraRadius(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CameraRadius = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DampingIntoCollision(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DampingIntoCollision = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DampingFromCollision(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Cinemachine.Cinemachine3rdPersonFollow gen_to_be_invoked = (Cinemachine.Cinemachine3rdPersonFollow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DampingFromCollision = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
