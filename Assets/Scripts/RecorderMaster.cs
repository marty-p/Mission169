using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

namespace SlugLib
{
    public class RecorderMaster : MonoBehaviour
    {
        //FIXME this is not going to work on ios/android
        public static string recordingFilePath = "C:\\Users\\Ben\\Desktop\\MSLUG\\Mission169\\Assets\\";
        public string recordingFileName = "game_play_recording";
        public float recordingFrameRate = 1f / 30f;

        private bool recording;
        private SpriteRenderer[] spriteRenderers;
        private RecordedSpriteRendererList recordedGameObject = new RecordedSpriteRendererList();

        List<SpriteRenderer> srs = new List<SpriteRenderer>();

        public void StartRecording()
        {
            StartCoroutine((RecordingCoroutine()));
        }

        public void StopRecording()
        {
            recording = false;
        }

        public void StartPlayback(string recordingFilePath)
        {
            string jsonString = File.ReadAllText(recordingFilePath);
            RecordedSpriteRendererList rsrl = JsonUtility.FromJson<RecordedSpriteRendererList>(jsonString);

            foreach (Transform child in transform)
            {
                Destroy(child);
            }

            for (int i = 0; i < rsrl.recordedSpriteRenderer.Count; i++)
            {
                // Create a game object for each sprite renderer recorded 
                GameObject goo = new GameObject(rsrl.recordedSpriteRenderer[i].name);
                goo.transform.SetParent(transform);
                // Add the sprite renderer component to our list of recorded data
                SpriteRenderer s = goo.AddComponent<SpriteRenderer>();
                rsrl.recordedSpriteRenderer[i].sr = s;
            }

            StartCoroutine(PlaybackCoroutine(rsrl));
        }

        public void StopPlayback() {}

        IEnumerator PlaybackCoroutine(RecordedSpriteRendererList rsrl)
        {
            int frameIndex = 0;
            while (true)
            {
                for (int i = 0; i < rsrl.recordedSpriteRenderer.Count; i++)
                {
                    if (!rsrl.recordedSpriteRenderer[i].ExistAtThisFrame((frameIndex)))
                    {
                        rsrl.recordedSpriteRenderer[i].sr.gameObject.SetActive(false);
                        continue;
                    }

                    RecordedSpriteRenderer rsr = rsrl.recordedSpriteRenderer[i];
                    rsr.cpt++;

                    rsr.sr.sprite = rsr.sprite[rsr.cpt];
                    rsr.sr.material.mainTextureOffset = rsr.uv[rsr.cpt];
                    rsr.sr.transform.position = rsr.pos[rsr.cpt];
                    rsr.sr.transform.eulerAngles = rsr.angle[rsr.cpt];
                    rsr.sr.flipX = rsr.flipped;
                    rsr.sr.sortingOrder = rsr.layer;

                    //	if (rgos[i].name == "Camera")
                    //	{
                    //		Camera.main.transform.position = rgos[i].recordedElements[0].pos[frameIndex];
                    //		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -1.55f);
                    //		Camera.main.GetComponent<FollowTarget>().enabled = false;
                    //		}
                }

                frameIndex++;
                yield return new WaitForSeconds(recordingFrameRate);
            }
        }

        IEnumerator RecordingCoroutine()
        {
            InitRecording();

            Debug.Log("recording started!");
            recording = true;
            int frameIndex = 0;
            while (recording)
            {
                //TODO SUrely this is wrong!!!!
                if (gameObject.activeSelf)
                {
                    CaptureFrame((frameIndex));
                }

                frameIndex++;
                yield return new WaitForSeconds(recordingFrameRate);
            }

            SaveToFile();
            print("end of recording");
        }

        void InitRecording()
        {
            spriteRenderers = FindObjectsOfType<SpriteRenderer>();

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                recordedGameObject.AddSpriteRender(new RecordedSpriteRenderer());
                recordedGameObject.recordedSpriteRenderer[i].name = spriteRenderers[i].gameObject.name;
            }

            recordingFilePath += gameObject.GetInstanceID() + gameObject.name + "recording";
        }

        void CaptureFrame(int frameIndex)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                SpriteRenderer sr = spriteRenderers[i];

                if (sr.GetComponent<Camera>() != null)
                {
                    recordedGameObject.recordedSpriteRenderer[i].pos.Add(sr.transform.position);
                    continue;
                }

                // sprite file path
                string assetPath = AssetDatabase.GetAssetPath(sr.sprite);
                if (string.IsNullOrEmpty(assetPath))
                {
                    continue;
                }

                recordedGameObject.recordedSpriteRenderer[i].sprite.Add(sr.sprite);

                // texture offset because spritesheets
                float textOffsetX = sr.sprite.uv[0].x;
                float textOffsetY = sr.sprite.uv[1].y - sr.sprite.uv[1].y;
                recordedGameObject.recordedSpriteRenderer[i].uv.Add(new Vector2(textOffsetX, textOffsetY));
                // texture width/height
                float w = sr.sprite.uv[1].x - sr.sprite.uv[0].x;
                float h = sr.sprite.uv[1].y - sr.sprite.uv[2].y;
                recordedGameObject.recordedSpriteRenderer[i].wh.Add(new Vector2(w, h));
                // position
                recordedGameObject.recordedSpriteRenderer[i].pos.Add(sr.transform.position);
                // angles
                recordedGameObject.recordedSpriteRenderer[i].angle.Add(sr.transform.eulerAngles);
                // flipped
                recordedGameObject.recordedSpriteRenderer[i].flipped = sr.flipX;
                // layer
                recordedGameObject.recordedSpriteRenderer[i].layer = sr.sortingOrder;

                recordedGameObject.recordedSpriteRenderer[i].frame.Add(frameIndex);
            }
        }

        void SaveToFile()
        {
            recordedGameObject.name = gameObject.name;

            string jsonSting = JsonUtility.ToJson(recordedGameObject);
            File.WriteAllText(recordingFilePath + recordingFileName, jsonSting);
        }

        void ReadFromFile(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            JsonUtility.FromJson<RecordedSpriteRenderer[]>(jsonString);
        }

        void Update()
        {
            if (Input.GetKeyDown("a"))
            {
                StartRecording();
            }
            else if (Input.GetKeyDown(("s")))
            {
                StopRecording();
            }
            else if (Input.GetKeyDown("d"))
            {
                StartPlayback(recordingFilePath + recordingFileName);
            }
        }
    }
}
