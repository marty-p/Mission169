using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Mission169 {
    /// <summary>
    /// Really just a place holder for all the UI elements used in the game
    /// so that anybody can show/hide them or set value of text
    /// </summary>
    public class UIManager : Singleton<UIManager> {

        public HUDManager HUD { get { return hudManager;} }
        public MainMenu MainMenuT { get { return mainMenu; } }
        public DialogManager Dialog { get { return dialogManager; } }
        private HUDManager hudManager;
        private MainMenu mainMenu;
        private DialogManager dialogManager;
        public TransitionOverlay blackOverlay { get; set; }

        public GameObject HUDPrefab;
        public GameObject maineMenuPrefab;
        public GameObject dialogManagerPrefab;
        public GameObject blackOverlayTransition;

        void Awake() {
            DontDestroyOnLoad(this);

            EventSystem eventSystem = gameObject.AddComponent(typeof(EventSystem)) as EventSystem;
            StandaloneInputModule inputModule = gameObject.AddComponent(typeof(StandaloneInputModule)) as StandaloneInputModule;

            GameObject hudManagerGO = Instantiate(Resources.Load<GameObject>("UI/HUD"));
            hudManager = hudManagerGO.GetComponent<HUDManager>();
            hudManager.transform.SetParent(transform);
            hudManager.SetVisible(false);

            GameObject mainMenuGO = Instantiate(Resources.Load<GameObject>("UI/MainMenu"));
            mainMenu = mainMenuGO.GetComponent<MainMenu>();
            mainMenu.transform.SetParent(transform);
            mainMenu.SetVisible(false);

            GameObject dialogManagerGO = Instantiate(Resources.Load<GameObject>("UI/DialogManager"));
            dialogManager = dialogManagerGO.GetComponent<DialogManager>();
            dialogManager.transform.SetParent(transform);
            dialogManager.SetVisible(false);

            GameObject blackOverlayGO = Instantiate(Resources.Load<GameObject>("UI/BlackOverlay"));
            blackOverlay = blackOverlayGO.GetComponent<TransitionOverlay>();
            blackOverlay.transform.SetParent(transform);
            blackOverlay.SetVisible(false);
        }
    }
}
