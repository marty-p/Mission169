using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    private bool levelOneLoaded;
    private string sceneCurrentlyPreviewed;

    private int selGridInt = 0;
    private string[] selStrings = new string[] { "Level 1", "Level 2", "GitHub", "About", "Share" };

    void OnGUI() {
        selGridInt = GUI.SelectionGrid(new Rect(25, 25, 150, 130), selGridInt, selStrings, 1);

        if (selGridInt == 0) {
            if (!levelOneLoaded) {
                SceneManager.LoadScene("mainscene", LoadSceneMode.Additive);
            }
            if(GUI.Button(new Rect(200, 70, 80,80), "START")) {
                SceneManager.UnloadScene("mainmenu");
              //  FindObjectOfType<GameManager>().MissionStart();
            }
            sceneCurrentlyPreviewed = "mainscene";
            levelOneLoaded = true;
        } else if (selGridInt == 1) {

        } else if (selGridInt == 2) {
            GitHub();
        } else if (selGridInt == 3) {

        } else if (selGridInt == 4) {

        }

        if (selGridInt != 0) {
            levelOneLoaded = false;
        }


    }


    private void GitHub() {
        // show text that says: "More info can be found on the GitHub page:"
        SceneManager.UnloadScene(sceneCurrentlyPreviewed);
        if (GUI.Button(new Rect(200, 70, 80,80), "Check Me Out!")) {
            Application.ExternalEval("window.open(\"http://www.unity3d.com\")");
        }
        sceneCurrentlyPreviewed = "github";

    }

}
