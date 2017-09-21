using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlugLib;
using Mission169;


public class RecordingUI : MonoBehaviour {

    [SerializeField] GameObject recordingFileNamePrefab;
    TextMeshProUGUI[] fileNames;
    [SerializeField] GameObject mainPanel;

    public void ToggleVisible()
    {
        mainPanel.SetActive(!mainPanel.activeSelf);

        if (mainPanel.activeSelf)
        {
            RecorderMaster.Instance.StopRecording();
            PopupulateRecordingList();
        }
        else
        {
            DestroyRecordingFileNames();
        }
    }

    void PopupulateRecordingList()
    {
        GamePlayRecording[] gameplayRecordings  = RecorderMaster.FindAllRecordings();

        fileNames = new TextMeshProUGUI[gameplayRecordings.Length];

        for (int i = 0; i < gameplayRecordings.Length; i++)
        {
            fileNames[i] = Instantiate(recordingFileNamePrefab).GetComponentInChildren<TextMeshProUGUI>();
            fileNames[i].transform.SetParent(recordingFileNamePrefab.transform.parent, false);
            // it is positionned via layout
            fileNames[i].text = gameplayRecordings[i].FileName;

            int cpi = i;
            fileNames[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                RecorderMaster.Instance.StartPlayback(gameplayRecordings[cpi]);
                //TODO
                GameManager.Instance.PauseGame(false);
                UIManager.Instance.Dialog.OnResumePressed();
            });
        }
        recordingFileNamePrefab.gameObject.SetActive(false);
    }

    void DestroyRecordingFileNames()
    {
        for (int i=0; i< fileNames.Length; i++)
        {
            Destroy(fileNames[i].gameObject);
        }
    }

    public void StartRecording()
    {
        RecorderMaster.Instance.StartRecording();
        GameManager.Instance.PauseGame(false);
        ToggleVisible();
    }

}
