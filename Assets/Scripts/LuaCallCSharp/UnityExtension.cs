
using UnityEngine;
using UnityEngine.U2D;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class UnityExtension
    {
        #region SpriteAtlas
        public static Sprite[] GetAllSprites(this SpriteAtlas spriteAtlas)
        {
            Sprite[] result = new Sprite[spriteAtlas.spriteCount];
            spriteAtlas.GetSprites(result);
            return result;
        }
        #endregion

        public static void Standard(this Transform transform)
        {
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }

        #region transform position
        public static void LocalPosition(this Transform transform, float x, float y, float z)
        {
            transform.localPosition = new Vector3(x, y, z);
        }

        public static void LocalPosition(this Transform transform, Vector3 position)
        {
            transform.localPosition = position;
        }

        public static void Position(this Transform transform, float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

        public static void Position(this Transform transform, Vector3 position)
        {
            transform.position = position;
        }

        public static void WorldMove(this Transform transform, Vector3 pos)
        {
            transform.position += pos;
        }

        public static void WorldMove(this Transform transform, float x, float y, float z)
        {
            transform.position += new Vector3(x, y, z);
        }

        public static void LocalMove(this Transform transform, Vector3 pos)
        {
            transform.localPosition += pos;
        }

        public static void LocalMove(this Transform transform, float x, float y, float z)
        {
            transform.localPosition += new Vector3(x, y, z);
        }
        #endregion

        #region transform rotation
        public static void Euler(this Transform transform, float x, float y, float z)
        {
            transform.localEulerAngles = new Vector3(x, y, z);
        }
        #endregion

        #region transform scale
        public static void Scale(this Transform transform, float x, float y, float z)
        {
            transform.localScale = new Vector3(x, y, z);
        }
        #endregion
    }
}

