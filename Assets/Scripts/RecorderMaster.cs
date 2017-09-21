using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace SlugLib
{
    public class RecorderMaster : Singleton<RecorderMaster>
    {
        public static string recordingFilePath;
        private const string recordingFileNamePrefix = "Mslug_rec-";
        public const float recordingFrameRate = 1f / 60f;

        private bool recording;
        private SpriteRenderer[] srBeingRecorded;
        private SpriteRenderer[] srPlayback;

        GamePlayRecording gameplayRecording;

        void Awake()
        {
            recordingFilePath = Application.persistentDataPath + "/";
            print(recordingFilePath);
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
                StartPlayback(recordingFileNamePrefix);

            }
            else if (Input.GetKeyDown("f"))
            {
                print(FindAllRecordings());
            }
        }

        public void StartRecording()
        {
            gameplayRecording = new GamePlayRecording( recordingFilePath + CreateFileName() );
            Debug.Log("recording started!");

            recording = true;
            StartCoroutine((RecordingCoroutine()));
        }

        public void StopRecording()
        {
            if (!recording)
            {
                return;
            }
            StopAllCoroutines();
            recording = false;
            SaveToFile2();
            SpriteDatabase.Save();
        }

        public void StartPlayback(string filePath, bool looping = true)
        {
            InitPlayback(filePath);
            StartCoroutine(PlaybackCoroutine(looping));
        }

        public void StartPlayback(GamePlayRecording recording, bool looping = true)
        {
            StartPlayback(recording.path, looping);
        }

        public void StopPlayback() {}

        public void InitPlayback(string filePath)
        {
            gameplayRecording = ParseRecordingV2(filePath);

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
        }

        IEnumerator PlaybackCoroutine(bool looping)
        {
        START:
            float prevTime = 0;

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

                    srPlayback[j].sprite = SpriteDatabase.FindSpriteByName(srf.spriteName);
                    srPlayback[j].transform.position = srf.pos;
                    srPlayback[j].transform.eulerAngles = new Vector3(0, srf.angleYZ.x, srf.angleYZ.y);
                    srPlayback[j].sortingOrder = srf.layer;

                    Camera.main.transform.position =  currentFrame.cameraPosition;
                }

                float timeToWait = prevTime == 0 ? 0.1f : currentFrame.time - prevTime;
                yield return new WaitForSeconds(timeToWait);

                prevTime = currentFrame.time;
            }

            if (looping)
            {
                goto START; // ZOMGBBQ goto
            }
        }

        IEnumerator RecordingCoroutine()
        {
            while (recording)
            {
                CaptureFrame();
                yield return new WaitForSecondsRealtime(recordingFrameRate);
            }
        }

        void CaptureFrame()
        {
            Frame frame = new Frame();
            gameplayRecording.frames.Add( frame );

            frame.cameraPosition = Camera.main.transform.position;
            frame.time = Time.time;

            SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < sr.Length; i++)
            {
                if (sr[i].sprite == null || !sr[i].isVisible)
                {
                    continue;
                }

                SpriteRendererFrame srFrame = new SpriteRendererFrame();

                frame.spriteRenderers.Add(srFrame);

                srFrame.spriteName = sr[i].sprite.name;

                srFrame.pos = sr[i].transform.position;

                srFrame.angleYZ.x = sr[i].transform.eulerAngles.y;
                srFrame.angleYZ.y = sr[i].transform.eulerAngles.z;

                srFrame.layer = sr[i].sortingOrder;

                //populating database
                if (SpriteDatabase.FindSpriteByName(srFrame.spriteName) == null)
                {
                    SpriteDatabase.AddSprite(sr[i].sprite, sr[i].sprite.name);
                }
            }
        }

        void SaveToFile(bool compress = true)
        {
            // to json string
            string jsonString = JsonUtility.ToJson(gameplayRecording);
            // string to byteArray
            byte[] stringBytes = Encoding.ASCII.GetBytes(jsonString);
            // to compressed byteArray
            if (compress)
            {
                stringBytes = Zip.Compress(stringBytes);
            }
            // byteArray to file
            File.WriteAllBytes(gameplayRecording.FileNameWithPath, stringBytes);

            Debug.Log("Recording saved to: " + gameplayRecording.FileNameWithPath);
        }

        void SaveToFile2()
        {
            List<Frame> frames = gameplayRecording.frames;
            StreamWriter stream = new StreamWriter(gameplayRecording.FileNameWithPath , true);
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < frames.Count; i++)
            {
                string oneFrame = "";

                oneFrame = frames[i].cameraPosition.x.ToString("0.000");
                oneFrame += "," + frames[i].cameraPosition.y.ToString("0.000");
                oneFrame += "," + frames[i].time.ToString("0.0000");

                // add time elapsed since last frame and other frame specific info
                for (int j = 0; j<frames[i].spriteRenderers.Count; j++)
                {
                    oneFrame += "," + frames[i].spriteRenderers[j].spriteName;
                    oneFrame += "," + ToStringMemoryEfficient(frames[i].spriteRenderers[j].pos.x);
                    oneFrame += "," + ToStringMemoryEfficient(frames[i].spriteRenderers[j].pos.y);
                    oneFrame += "," + ToStringMemoryEfficient(frames[i].spriteRenderers[j].angleYZ.x);
                    oneFrame += "," + ToStringMemoryEfficient(frames[i].spriteRenderers[j].angleYZ.y);
                    oneFrame += "," +  frames[i].spriteRenderers[j].layer;

                    oneFrame += " ";
                }

                //bytes.Add(Encoding.ASCII.GetBytes(oneFrame).);

                stream.WriteLine(oneFrame);
            }

            stream.Dispose();
        }

        GamePlayRecording ReadFromFileResources(string fileName)
        {
            // removing file extension
            int index = fileName.LastIndexOf('.');
            if (index >= 0)
            {
                fileName = fileName.Substring(0, index);
            }

            TextAsset t = Resources.Load(fileName, typeof(TextAsset))as TextAsset;

            byte[] compressedStringBytes = t.bytes;
            byte[] stringBytes = Zip.Decompress(compressedStringBytes);
            string jsonString = Encoding.ASCII.GetString(stringBytes);

            return JsonUtility.FromJson<GamePlayRecording>(jsonString);
        }

        public static GamePlayRecording[] FindAllRecordings()
        {
            string[] files = Directory.GetFiles(recordingFilePath);
            GamePlayRecording[] recordings = new GamePlayRecording[files.Length];

            //TODO here should be able to tell if it is compressed or not and then do magic accordingly
            for (int i=0; i < files.Length; i++)
            {
                recordings[i] = ParseRecordingV2(files[i]);
            }
            return recordings;
        }

        private static GamePlayRecording ParseRecordingV2(string fileNameWithPath)
        {
            GamePlayRecording recording = new GamePlayRecording(fileNameWithPath);
            recording.path = fileNameWithPath;


            string[] lines = null;
            try
            {
                // StreamReader stream = new StreamReader(fileNameWithPath);
                lines = File.ReadAllLines("a.txt");
            }
            catch(FileNotFoundException e)
            {
                print("looking for " + fileNameWithPath);
                TextAsset t = Resources.Load(fileNameWithPath, typeof(TextAsset)) as TextAsset;

                if (t == null)
                    print("NOT FOUND");

                lines = Regex.Split(t.text, "\n|\r|\r\n");
            }

            string line;
            //while((line = stream.ReadLine()) != null)
            for (int lineNumber=0; lineNumber < lines.Length; lineNumber++)
            {
                line = lines[lineNumber];

                string[] values = line.Split(',');

                Frame frame = new Frame();

                try
                {
                    frame.cameraPosition.x = float.Parse(values[0]);
                    frame.cameraPosition.y = float.Parse(values[1]);
                    frame.time = float.Parse(values[2]);

                    for (int i = 3; i < values.Length; i = i + 6)
                    {
                        SpriteRendererFrame srFrame = new SpriteRendererFrame();

                        srFrame.spriteName = values[i];
                        srFrame.pos.x = float.Parse(values[i + 1]);
                        srFrame.pos.y = float.Parse(values[i + 2]);
                        srFrame.angleYZ.x = float.Parse(values[i + 3]);
                        srFrame.angleYZ.y = float.Parse(values[i + 4]);
                        srFrame.layer = int.Parse(values[i + 5]);

                        frame.spriteRenderers.Add(srFrame);
                    }
                    recording.frames.Add(frame);
                }
                catch(Exception e)
                {
                    print(e);
                }
            }
            return recording;
        }

        private static GamePlayRecording ParseRecording(string filepath)
        {
            // removing file extension
            //int index = filepath.LastIndexOf('.');
            //if (index >= 0)
            //{
            //    filepath = filepath.Substring(0, index);
            //}

            byte[] compressedStringBytes = File.ReadAllBytes(filepath);
            byte[] stringBytes = Zip.Decompress(compressedStringBytes);
            string jsonString = Encoding.ASCII.GetString(stringBytes);

            return JsonUtility.FromJson<GamePlayRecording>(jsonString);
        }

        //TODO use string format
        string CreateFileName()
        {
            System.DateTime now = System.DateTime.Now;
            return recordingFileNamePrefix + now.Day + now.Month + now.Year + "-" + now.Hour + now.Minute + now.Second;
        }

        string ToStringMemoryEfficient(float val, string precision = "0.000")
        {
            if (val == 0f)
            {
                return "0";
            }
            else
            {
                return val.ToString(precision);
            }
        }

        // for SLugLIB:
        string RemoveFileExtension(string filepath)
        {
            // just doing that should work too
            //assetPath = assetPath.Substring(0, assetPath.LastIndexOf('.'));

            // removing file extension
            int index = filepath.LastIndexOf('.');
            if (index >= 0)
            {
                  return filepath.Substring(0, index);
            }
            else
            {
                return "";
            }
        }

    }
}
