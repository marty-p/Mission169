using UnityEngine;
using System.Collections.Generic;

namespace SlugLib
{
    [System.Serializable]
    public class RecordedSpriteRendererList
    {
        public string name = "";

        public List<RecordedSpriteRenderer> recordedSpriteRenderer = new List<RecordedSpriteRenderer>();

        public List<Vector3> cameraPosition = new List<Vector3>();

        public void Add(RecordedSpriteRenderer rsr, string name)
        {
            recordedSpriteRenderer.Add(rsr);
            rsr.name = name;
        }
    }

    [System.Serializable]
    public class RecordedSpriteRenderer
    {
        public SpriteRenderer sr;

        public List<SpriteRendererFrame> frames = new List<SpriteRendererFrame>();

        public string name;
        public bool flipped;
        public int layer;
    }




    [System.Serializable]
    public class GamePlayRecording
    {
        public List<Frame> frames = new List<Frame>();
        public List<Vector3> cameraPosition = new List<Vector3>();
    }
    [System.Serializable]
    public class Frame
    {
        public List<SpriteRendererFrame> spriteRenderers = new List<SpriteRendererFrame>();
    }

    // TODO make that a struct
    [System.Serializable]
    public class SpriteRendererFrame
    {
        public Sprite sprite;
        public Vector2 uv;
        public Vector2 wh;
        public Vector2 pos;
        public Vector2 angleYZ;
        public int layer;
    }

}
