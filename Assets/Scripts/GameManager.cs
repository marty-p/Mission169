using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public int playerLifeCount = 2;
    private int score;
    public PlayerDeathManager player; // TODO the gameManager should instantiate the player
    // Keep it like it for now in order to always have a Player in the hierarchy even when not running
    private TimeUtils timeUtils;
    private float waitTimeBeforeSpawn = 1.5f;


	void Start () {
        timeUtils = GetComponent<TimeUtils>();
        EventManager.StartListening("player_death",
                    ()=> timeUtils.TimeDelay(waitTimeBeforeSpawn, OnplayerDeath));
	}
	
    private void OnplayerDeath() {
        playerLifeCount--;
        if (playerLifeCount >= 0) {
            player.SpawnPlayer();
            EventManager.TriggerEvent("player_back_alive");
        } else {
            // Continue ?
        }
    }


}
