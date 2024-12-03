using XLua;
using DG.Tweening;

namespace MTG
{
    [LuaCallCSharp]
    public static class DOTweenUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="loops"></param>
        /// <param name="loopType"><see cref="DG.Tweening.LoopType"></param>
        public static void SetLoop<T>(T t, int loops, int loopType = 1) where T : Tween
        {
            t.SetLoops(loops, (LoopType)loopType);
        }
    }
}

