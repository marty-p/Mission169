using UnityEngine;
using System.Collections.Generic;

namespace SlugLib
{

    [System.Serializable]
    public class RecordedGameObject
    {
        public string name = "";

        public List<RecordedElement> recordedElements = new List<RecordedElement>();

        public void AddSpriteRender(RecordedElement re)
        {
            recordedElements.Add(re);
        }
    }

    [System.Serializable]
    public class RecordedElement
    {
        public string name;
        public bool isFLiped;
        public int layer;
        public List<int> frameIndex = new List<int>();
        //public List<string> spritePath = new List<string>();

        public List<Sprite> sprite = new List<Sprite>();
        public List<Vector2> uvs = new List<Vector2>();
        public List<Vector2> wh = new List<Vector2>();
        public List<Vector2> pos = new List<Vector2>();
        public List<Vector2> angle = new List<Vector2>();

        public int cpt;

        private SpriteRenderer sr;

        public void AddSr(SpriteRenderer sr)
        {
            this.sr = sr;
        }

        public SpriteRenderer GetSr()
        {
            return this.sr;
        }

        public bool ExistAtThisFrame(int frameIndex)
        {
            for (int i = 0; i < this.frameIndex.Count; i++)
            {
                if (this.frameIndex[i] == frameIndex)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
