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
        public static string recordingFilePath = "C:\\Users\\Ben\\Desktop\\MSLUG\\Mission169\\Assets\\" ;

        bool recording;

        public float recordingFrameRate = 1f / 30f;

        RecordedGameObject rgos;

        GameObject go; // the parent of everything created when playing a recording

        private SpriteRenderer[] spriteRenderers;
        private RecordedGameObject recordedGameObject = new RecordedGameObject();

        List<SpriteRenderer> srs = new List<SpriteRenderer>();

        void StartRecording()
        {
            StartCoroutine((RecordingCoroutine()));
        }

        void StopRecording()
        {
            recording = false;
        }


        void StartPlayback()
        {
            string[] files = Directory.GetFiles(recordingFilePath);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].IndexOf("recording") == -1 || files[i].IndexOf("meta") != -1)
                {
                    continue;
                }

                string jsonString = File.ReadAllText(files[i]);
                rgos = JsonUtility.FromJson<RecordedGameObject>(jsonString);
            }

            if (go != null)
            {
                Destroy(go);
            }

            go = new GameObject("masta masta");
            go.transform.SetParent(transform);

            for (int j = 0; j < rgos.recordedElements.Count; j++)
            {
                GameObject goo = new GameObject(rgos.recordedElements[j].name);
                goo.transform.SetParent(go.transform);

                SpriteRenderer s = goo.AddComponent<SpriteRenderer>();
                rgos.recordedElements[j].AddSr(s);
            }

            StartCoroutine(PlaybackCoroutine());
        }

        IEnumerator PlaybackCoroutine()
        {
            int frameIndex = 0;
            while (true)
            {

                for (int j = 0; j < rgos.recordedElements.Count; j++)
                {
                    if (!rgos.recordedElements[j].ExistAtThisFrame((frameIndex)))
                    {
                        rgos.recordedElements[j].GetSr().gameObject.SetActive(false);
                        continue;
                    }

                    SpriteRenderer s = rgos.recordedElements[j].GetSr();
                    //s.sprite = Resources.Load<Sprite>(rgos.recordedElements[j].spritePath[rgos.recordedElements[j].cpt]);

                    s.sprite = rgos.recordedElements[j].sprite[rgos.recordedElements[j].cpt];
                    s.material.mainTextureOffset = rgos.recordedElements[j].uvs[rgos.recordedElements[j].cpt];
                    s.transform.position = rgos.recordedElements[j].pos[rgos.recordedElements[j].cpt];
                    s.transform.eulerAngles = rgos.recordedElements[j].angle[rgos.recordedElements[j].cpt];
                    s.flipX = rgos.recordedElements[j].isFLiped;
                    s.sortingOrder = rgos.recordedElements[j].layer;

                    rgos.recordedElements[j].cpt++;

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
            Init();

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

        public void Init()
        {
            spriteRenderers = FindObjectsOfType<SpriteRenderer>();

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                recordedGameObject.AddSpriteRender(new RecordedElement());
                recordedGameObject.recordedElements[i].name = spriteRenderers[i].gameObject.name;
            }

            recordingFilePath += gameObject.GetInstanceID() + gameObject.name + "recording";
        }

        public void CaptureFrame(int frameIndex)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                SpriteRenderer sr = spriteRenderers[i];


                if (sr.GetComponent<Camera>() != null)
                {
                    recordedGameObject.recordedElements[i].pos.Add(sr.transform.position);
                    continue;
                }

                // sprite file path
                string assetPath = AssetDatabase.GetAssetPath(sr.sprite);
                if (string.IsNullOrEmpty(assetPath))
                {
                    continue;
                }
                //print(sr.gameObject.name + " " + assetPath);

                // remove everything up to Resources
                //assetPath = assetPath.Substring(assetPath.LastIndexOf("Resources/"));
                // remove "Resources/"
                //assetPath = assetPath.Replace("Resources/", "");
                // remove extension
                //assetPath = assetPath.Substring(0, assetPath.LastIndexOf('.'));
                //recordedGameObject.recordedElements[i].spritePath.Add(assetPath);

                recordedGameObject.recordedElements[i].sprite.Add(sr.sprite);

                // texture offset because spritesheets
                float textOffsetX = sr.sprite.uv[0].x;
                float textOffsetY = sr.sprite.uv[1].y - sr.sprite.uv[1].y;
                recordedGameObject.recordedElements[i].uvs.Add(new Vector2(textOffsetX, textOffsetY));
                // texture width/height
                float w = sr.sprite.uv[1].x - sr.sprite.uv[0].x;
                float h = sr.sprite.uv[1].y - sr.sprite.uv[2].y;
                recordedGameObject.recordedElements[i].wh.Add(new Vector2(w, h));
                // position
                recordedGameObject.recordedElements[i].pos.Add(sr.transform.position);
                // angles
                recordedGameObject.recordedElements[i].angle.Add(sr.transform.eulerAngles);
                // flipped
                recordedGameObject.recordedElements[i].isFLiped = sr.flipX;
                // layer
                recordedGameObject.recordedElements[i].layer = sr.sortingOrder;


                recordedGameObject.recordedElements[i].frameIndex.Add(frameIndex);
            }
        }

        public void SaveToFile()
        {
            recordedGameObject.name = gameObject.name;

            string jsonSting = JsonUtility.ToJson(recordedGameObject);
            File.WriteAllText(recordingFilePath, jsonSting);
        }

        public void ReadFromFile(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            JsonUtility.FromJson<RecordedElement[]>(jsonString);
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
                StartPlayback();
            }
        }

    }
}
