using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace SlugLib
{
    public static class SpriteDatabase
    {
        const string databaseName = "spritesDB";

        private static List<SlugSprite> sprites = new List<SlugSprite>();
        public static List<SlugSprite> Sprites { get { return sprites; } }

        public static Sprite FindSpriteByName(string name)
        {
            for (int i=0; i<sprites.Count; i++)
            {
                if (name == sprites[i].name)
                {
                    return sprites[i].sprite;
                }
            }
            return null;
        }

        public static void AddSprite(Sprite sprite, string spriteName)
        {
            SlugSprite slugSprite = new SlugSprite();
            slugSprite.sprite = sprite;
            slugSprite.name = spriteName;

            sprites.Add(slugSprite);
        }

        static SpriteDatabase()
        {
            Debug.Log("test!");
            Object o = Resources.Load(databaseName);
            if (o != null)
            {
                GameObject go = GameObject.Instantiate(o) as GameObject;
                sprites = go.GetComponent<SlugSpriteContainer>().sprites;
#if UNITY_EDITOR
               // GameObject.DestroyImmediate(go);
#else
               // GameObject.Destroy( go );
#endif
            }
        }

        public static void Save()
        {
#if UNITY_EDITOR
            Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Resources/" + databaseName +".prefab");
            GameObject go = new GameObject("ItemData", typeof(SlugSpriteContainer));
            go.GetComponent<SlugSpriteContainer>().sprites = sprites;
            PrefabUtility.ReplacePrefab(go, prefab);
            GameObject.DestroyImmediate(go);
#endif
        }
    }
}