/*
UniGif
Copyright (c) 2015 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlugReplay : MonoBehaviour {
    [SerializeField]
    private UniGifImage uniGifImage;
    private bool gifSaved;
    private RawImage image;
    public Image LoadingImage;

    private bool mutex;

    void Awake() {
        image = GetComponent<RawImage>();
    }

    void OnEnable() {
        gifSaved = false;
        if (Record.m_Recorder != null) {
            Record.m_Recorder.Save();
            Record.m_Recorder.OnFileSaved += OnGifSaved;
        }
        uniGifImage.Stop();
        image.enabled = false;
        LoadingImage.gameObject.SetActive(true);
    }

    public void OnDisable() {
        StopAllCoroutines();
        if (Record.m_Recorder != null) {
            Record.m_Recorder.Record();
        }
    }

    void Update() {
        if (gifSaved) {
            if (mutex == false) {
                gifSaved = false;
                mutex = true;
                StartCoroutine(ViewGifCoroutine());
            }
        }
    }

    void OnGifSaved(int a, string fileName) {
        gifSaved = true;
    }

    private IEnumerator ViewGifCoroutine() {
        string path = "gif_record.gif";
        yield return StartCoroutine(uniGifImage.SetGifFromUrlCoroutine(path));
        image.enabled = true;
        LoadingImage.gameObject.SetActive(false);
        mutex = false;
    }
}