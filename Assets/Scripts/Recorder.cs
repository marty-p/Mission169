using UnityEngine;
using System.Collections.Generic;

namespace SlugLib
{
    [System.Serializable]
    public class RecordedSpriteRendererList
    {
        public string name = "";

        public List<RecordedSpriteRenderer> recordedSpriteRenderer = new List<RecordedSpriteRenderer>();

        public void AddSpriteRender(RecordedSpriteRenderer rsr)
        {
            recordedSpriteRenderer.Add(rsr);
        }
    }

    [System.Serializable]
    public class RecordedSpriteRenderer
    {
        public string name;
        public bool flipped;
        public int layer;
        public List<int> frame = new List<int>();
        public List<Sprite> sprite = new List<Sprite>();
        public List<Vector2> uv = new List<Vector2>();
        public List<Vector2> wh = new List<Vector2>();
        public List<Vector2> pos = new List<Vector2>();
        public List<Vector2> angle = new List<Vector2>();

        public int cpt;

        public SpriteRenderer sr;

        public bool ExistAtThisFrame(int frameIndex)
        {
            for (int i = 0; i < frame.Count; i++)
            {
                if (frame[i] == frameIndex)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
