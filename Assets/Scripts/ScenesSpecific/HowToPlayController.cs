using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class HowToPlayController : MonoBehaviour {

    public MovementManager marcoMove;
    public MovementManager marcoJump;
    public MarcoAttackManager marcoAttack;

    private RetVoidTakeVoid marcoMoveAction;
    private RetVoidTakeVoid marcoAttackAction;

    private TimeUtils timeUtils;

    private RetVoidTakeVoid[] marcoMoveActions;
    private RetVoidTakeVoid[] marcoAttackActions;

    public GameObject allTheMarcos;
    public GameObject keyboardInstructions;
    public GameObject gamePadProTip;

    void Awake() {
        timeUtils = GetComponent<TimeUtils>();
    }

    void Start() {
        int moveActionCount = 2;
        marcoMoveActions = new RetVoidTakeVoid[moveActionCount];
        marcoMoveActions[0] = () => { marcoMove.HorizontalMovement(Vector2.right); };
        marcoMoveActions[1] = () => { marcoMove.HorizontalMovement(Vector2.left); };
        //marcoMoveActions[2] = marcoMove.LookUp;
        //marcoMoveActions[3] = marcoMove.DownMovement;
        marcoMoveAction = marcoMoveActions[0];

        marcoAttackActions = new RetVoidTakeVoid[2];
        marcoAttackActions[0] = ()=> { marcoAttack.PrimaryAttack(); };
        marcoAttackActions[1] = ()=> { marcoAttack.SecondaryAttack(); };
        marcoAttackAction = marcoAttackActions[0];

        int actionIndex = 0;
        timeUtils.RepeatEvery(2, () => {

            marcoMoveAction = marcoMoveActions[actionIndex];
            marcoAttackAction = marcoAttackActions[actionIndex];
            marcoJump.Jump();
            marcoAttackAction();

            actionIndex = (actionIndex+1) % marcoMoveActions.Length;
            return true;
        });
    }

    void Update() {
        marcoMoveAction();

        if (Input.anyKey) {
            allTheMarcos.SetActive(false);
            keyboardInstructions.SetActive(false);
            gamePadProTip.SetActive(true);
            timeUtils.TimeDelay(1.5f, () => {SceneManager.LoadScene("mainscene");});
        }

    }
}
