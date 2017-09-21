using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace SlugLib
{
    [System.Serializable]
    public class GamePlayRecording
    {
        public string FileNameWithPath{ get; private set; }
        public string path;

        public string FileName { get { return Path.GetFileName(FileNameWithPath); } }

        public List<Frame> frames = new List<Frame>();

        public GamePlayRecording(string fileNameWithPath)
        {
            FileNameWithPath = fileNameWithPath;
        }
    }

    [System.Serializable]
    public class Frame
    {
        public Vector2 cameraPosition;
        public float time;
        public List<SpriteRendererFrame> spriteRenderers = new List<SpriteRendererFrame>();
    }

    // TODO make that a struct
    [System.Serializable]
    public class SpriteRendererFrame
    {
        public string spriteName;
        public Vector2 pos;
        public Vector2 angleYZ;
        public int layer;
    }

    [System.Serializable]
    public class SlugSprite
    {
        public Sprite sprite;
        public string name;
    }
}
