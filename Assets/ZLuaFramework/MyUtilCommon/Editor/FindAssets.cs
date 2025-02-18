using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZLuaFramework
{
    public static class FindAssets
    {
        /// <summary>
        /// ��ȡProject��ѡ�е�Objects
        /// </summary>
        /// <returns></returns>
        public static List<T> SelectObjectsInProject<T>(bool controlChildren = false) where T : UnityEngine.Object
        {
            string[] strs = Selection.assetGUIDs;
            UnityEngine.Object[] selectedObjects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
            if (strs == null || strs.Length == 0)
            {
                Debug.LogError("δѡ���κ���Դ");
                return null;
            }
            HashSet<T> children = new HashSet<T>();
            foreach (string guid in strs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                if (obj is DefaultAsset)
                {
                    ProcessFolder(AssetDatabase.GetAssetPath(obj), children);
                }
                else if (obj is T t)
                {
                    children.Add(t);
                }
            }

            /*if (controlChildren)
            {
                List<GameObject> result = new List<GameObject>();
                foreach (T obj in children)
                {
                    result.Add(obj as GameObject);
                    GetAllChild((obj as GameObject).transform, result, null);
                }
                return result;
            }
            else
            {
                return children.ToList();
            }*/
            return children.ToList();
        }

        static void ProcessFolder<T>(string folderPath, HashSet<T> children) where T : UnityEngine.Object
        {
            if (AssetDatabase.IsValidFolder(folderPath))
            {
                string[] guids = AssetDatabase.FindAssets("t:Object", new[] { folderPath });

                foreach (string guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

                    if (obj != null && obj is T t)
                    {
                        children.Add(t);
                    }
                }
            }
        }

        public static List<GameObject> AllGameObjectInProject()
        {
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

            List<GameObject> prefabsList = new List<GameObject>();

            foreach (string guid in prefabGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (prefab != null)
                {
                    prefabsList.Add(prefab);
                }
            }
            return prefabsList;
        }

        /// <summary>
        /// ����ȫ��������
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeSelf">���������Լ�</param>
        /// <param name="eachAction">����������ÿ��GOִ�еĲ���</param>
        /// <returns></returns>
        public static void ForEachChild(this GameObject gameObject, bool includeSelf = false, Action<GameObject> eachAction = null)
        {
            List<GameObject> children = new List<GameObject>();
            if (includeSelf)
            {
                children.Add(gameObject);
                eachAction?.Invoke(gameObject);
            }
            GetAllChild(gameObject.transform, children, eachAction);
            children.Clear();
            children = null;
        }

        static void GetAllChild(Transform parent, List<GameObject> children, Action<GameObject> eachAction)
        {
            // ������ǰ�ڵ�������ӽڵ�
            for (int i = 0; i < parent.childCount; i++)
            {
                // ��ȡ�� i ���ӽڵ�� Transform ���
                Transform child = parent.GetChild(i);

                children.Add(child.gameObject);
                eachAction?.Invoke(child.gameObject);

                // �ݹ�����ӽڵ���ӽڵ�
                GetAllChild(child, children, eachAction);
            }
        }

        public static bool IsNullOrEmpty(this List<GameObject> go)
        {
            return go == null || go.Count == 0;
        }
    }
}

