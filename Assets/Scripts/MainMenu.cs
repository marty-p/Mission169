using UnityEngine;
using UnityEngine.UI;
using Mission169;

public class MainMenu : MonoBehaviour {
    public Button start;
    public Button museum;
    public Button thanks;
    
    void Start() {
        start.onClick.AddListener(GameManager.Instance.MissionStart);
    }    

}
