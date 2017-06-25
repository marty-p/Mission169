using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SlugLib
{
    public class RecorderMaster : MonoBehaviour
    {
        //FIXME this is not going to work on ios/android
        public static string recordingFilePath = "C:\\Users\\Ben\\Desktop\\MSLUG\\Mission169\\Assets\\";
        public string recordingFileName = "game_play_recording";
        public float recordingFrameRate = 1f / 30f;

        private bool recording;
        private SpriteRenderer[] srBeingRecorded;
        private SpriteRenderer[] srPlayback;
        private RecordedSpriteRendererList recordedSpriteRendererList = new RecordedSpriteRendererList();

        List<SpriteRenderer> srs = new List<SpriteRenderer>();

        GamePlayRecording gameplayRecording;

        public void StartRecording()
        {
            //InitRecording();

            InitVer2();

            //StartCoroutine((RecordingCoroutine()));
            StartCoroutine((RecordingCoroutine()));
        }

        public void StopRecording()
        {
            recording = false;
        }

        public void StartPlayback(string recordingFilePath)
        {
            RecordedSpriteRendererList rsrl = ReadFromFile(recordingFilePath);

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

        public void StartPlaybackV2(string filePath)
        {
            InitPlaybackV2(filePath);
            StartCoroutine(PlaybackCoroutineV2());
        }

        public void StopPlayback() {}

        IEnumerator PlaybackCoroutine(RecordedSpriteRendererList srList)
        {
            int frameIndex = 0;

            //need a frameMax so that I quite the wile loop when lastfRame is reeached

            while (true)
            {
                for (int i = 0; i < srList.recordedSpriteRenderer.Count; i++)
                {
                    RecordedSpriteRenderer rsr = srList.recordedSpriteRenderer[i];

                    if (rsr.frames[frameIndex] == null)
                    {
                        rsr.sr.gameObject.SetActive(false);
                        continue;
                    }
                    else
                    {
                        rsr.sr.gameObject.SetActive(true);
                    }

                    rsr.sr.sprite = rsr.frames[frameIndex].sprite;
                    rsr.sr.material.mainTextureOffset = rsr.frames[frameIndex].uv;
                    rsr.sr.transform.position = rsr.frames[frameIndex].pos;
                    //rsr.sr.transform.eulerAngles = rsr.frames[frameIndex].angle;
                    rsr.sr.flipX = rsr.flipped;
                    rsr.sr.sortingOrder = rsr.layer;
                }

                Camera.main.transform.position = srList.cameraPosition[frameIndex];

                frameIndex++;

                if (frameIndex >= srList.cameraPosition.Count)
                {
                    frameIndex = 0;
                }

                yield return new WaitForSeconds(recordingFrameRate);
            }
        }


        public void InitPlaybackV2(string filePath)
        {
            gameplayRecording = ReadFromFileResourcesV2(filePath);

            //Hiding all the exisiting objects
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Camera>() == null)
                {
                    child.gameObject.SetActive(false);
                }
                else // spcial things to do for the camera and its child
                {
                    FollowTarget follow = child.GetComponent<FollowTarget>();

                    if (follow != null)
                    {
                        follow.enabled = false;
                    }

                    foreach (Transform t in child)
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }
            // Destroying all the spriteRenderers previously created for playback
            if (srPlayback != null)
            {
                foreach (SpriteRenderer sr in srPlayback)
                {
                    Destroy(sr.gameObject);
                }
            }
            // We create as many sr as the frame that needs the most
            int spriteRenderersNeeded = 0;
            for (int i = 0; i < gameplayRecording.frames.Count; i++)
            {
                if (gameplayRecording.frames[i].spriteRenderers.Count > spriteRenderersNeeded)
                {
                    spriteRenderersNeeded = gameplayRecording.frames[i].spriteRenderers.Count;
                }
            }
            srPlayback = new SpriteRenderer[spriteRenderersNeeded];
            for (int i = 0; i < spriteRenderersNeeded; i++)
            {
                // Create a game object for each sprite renderer recorded 
                GameObject go = new GameObject(i.ToString());
                go.transform.SetParent(transform);
                srPlayback[i]  = go.AddComponent<SpriteRenderer>();
                srPlayback[i].gameObject.SetActive(false);
            }
            print("created " + srPlayback.Length + " sr for playback");
        }

        IEnumerator PlaybackCoroutineV2()
        {
            print("starting playback");

            for (int i=0; i < gameplayRecording.frames.Count; i++ )
            {
                Frame currentFrame = gameplayRecording.frames[i];

                for (int j = 0; j < srPlayback.Length; j++)
                {
                    srPlayback[j].gameObject.SetActive(false);
                }

                for (int j=0; j < currentFrame.spriteRenderers.Count; j++)
                {
                    SpriteRendererFrame srf = currentFrame.spriteRenderers[j];

                    srPlayback[j].gameObject.SetActive(true);
                    srPlayback[j].sprite = srf.sprite;
                    srPlayback[j].material.mainTextureOffset = srf.uv;
                    srPlayback[j].transform.position = srf.pos;
                    srPlayback[j].transform.eulerAngles = new Vector3(0, srf.angleYZ.x, srf.angleYZ.y);
                    srPlayback[j].sortingOrder = srf.layer;
                    //rsr.sr.flipX = srf.flipped;

                    Camera.main.transform.position = gameplayRecording.cameraPosition[i];
                }
                               
                yield return new WaitForSeconds(recordingFrameRate);
                print("next frame");
            }

        }

        IEnumerator RecordingCoroutine()
        {
            Debug.Log("recording started!");
            recording = true;
            while (recording)
            {
                //CaptureFrame();
                CaptureFrameVer2();

                yield return new WaitForSeconds(recordingFrameRate);
            }
            SaveToFile();
            //SaveToFileV2();
            print("end of recording");
        }

        void InitRecording()
        {
            srBeingRecorded = GetComponentsInChildren<SpriteRenderer>(true);

            for (int i = 0; i < srBeingRecorded.Length; i++)
            {
                recordedSpriteRendererList.Add(new RecordedSpriteRenderer(), srBeingRecorded[i].gameObject.name);

                recordedSpriteRendererList.recordedSpriteRenderer[i].flipped = srBeingRecorded[i].flipX;
                recordedSpriteRendererList.recordedSpriteRenderer[i].layer = srBeingRecorded[i].sortingOrder;
            }

            recordingFilePath += gameObject.GetInstanceID() + gameObject.name;
        }

        void InitVer2()
        {
            gameplayRecording = new GamePlayRecording();
        }

        void CaptureFrameVer2()
        {
            Frame frame = new Frame();
            gameplayRecording.frames.Add( frame );
            gameplayRecording.cameraPosition.Add(Camera.main.transform.position);

            SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < sr.Length; i++)
            {
                if (sr[i].sprite == null)
                {
                    continue;
                }

                SpriteRendererFrame srFrame = new SpriteRendererFrame();
                frame.spriteRenderers.Add(srFrame);

                srFrame.sprite = sr[i].sprite;

                float textOffsetX = sr[i].sprite.uv[0].x;
                float textOffsetY = sr[i].sprite.uv[1].y - sr[i].sprite.uv[1].y;
                srFrame.uv = new Vector2(textOffsetX, textOffsetY);

                float w = sr[i].sprite.uv[1].x - sr[i].sprite.uv[0].x;
                float h = sr[i].sprite.uv[1].y - sr[i].sprite.uv[2].y;
                srFrame.wh = new Vector2(w, h);

                srFrame.pos = sr[i].transform.position;

                srFrame.angleYZ.x = sr[i].transform.eulerAngles.y;
                srFrame.angleYZ.y = sr[i].transform.eulerAngles.z;

                srFrame.layer = sr[i].sortingOrder;
            }
        }

        void CaptureFrame()
        {
            for (int i = 0; i < srBeingRecorded.Length; i++)
            {
                SpriteRenderer sr = srBeingRecorded[i];
                RecordedSpriteRenderer recordedSr = recordedSpriteRendererList.recordedSpriteRenderer[i];

                if (sr!=null && sr.gameObject != null && sr.sprite == null || !sr.gameObject.activeSelf)
                {
                    // It's actually extremely inefficient to do that (size wise) but oh well it gets compressed anyway
                    recordedSr.frames.Add(null);
                    continue;
                }

                SpriteRendererFrame srFrame = new SpriteRendererFrame();
                srFrame.sprite = sr.sprite;

                float textOffsetX = sr.sprite.uv[0].x;
                float textOffsetY = sr.sprite.uv[1].y - sr.sprite.uv[1].y;
                srFrame.uv = new Vector2(textOffsetX, textOffsetY);

                float w = sr.sprite.uv[1].x - sr.sprite.uv[0].x;
                float h = sr.sprite.uv[1].y - sr.sprite.uv[2].y;
                srFrame.wh = new Vector2(w, h);

                srFrame.pos = sr.transform.position;

                //srFrame.angle = sr.transform.eulerAngles;

                recordedSr.frames.Add(srFrame);
            }

            recordedSpriteRendererList.cameraPosition.Add(Camera.main.transform.position);
        }

        void SaveToFile()
        {
            string jsonString = JsonUtility.ToJson(gameplayRecording);

            //string to byteArray
            byte[] stringBytes = Encoding.ASCII.GetBytes(jsonString);
            //compress byteArray
            byte[] compressedStringBytes = Zip.Compress(stringBytes);
            //byteArray to file

            File.WriteAllBytes(recordingFilePath + recordingFileName, compressedStringBytes);
        }

        void SaveToFileV2()
        {
            print( JsonUtility.ToJson(gameplayRecording) );
        }

        RecordedSpriteRendererList ReadFromFile(string filePath)
        {
            byte[] compressedStringBytes = File.ReadAllBytes(filePath);
            byte[] stringBytes = Zip.Decompress(compressedStringBytes);
            string jsonString = Encoding.ASCII.GetString(stringBytes);

            return JsonUtility.FromJson<RecordedSpriteRendererList>(jsonString);
        }

        GamePlayRecording ReadFromFileResourcesV2(string fileName)
        {
            TextAsset t = Resources.Load(fileName, typeof(TextAsset))as TextAsset;

            byte[] compressedStringBytes = t.bytes;
            print("compressed sting byyte: " + compressedStringBytes.Length);
            byte[] stringBytes = Zip.Decompress(compressedStringBytes);

            print("uncompressed byyte: " + stringBytes.Length);
            string jsonString = Encoding.ASCII.GetString(stringBytes);

            print(jsonString.Length);

            return JsonUtility.FromJson<GamePlayRecording>(jsonString);
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
                //StartPlayback(recordingFilePath + recordingFileName);
                StartPlaybackV2(recordingFileName);

            }
        }
    }
}
