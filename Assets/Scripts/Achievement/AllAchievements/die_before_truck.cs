using SlugLib;
using Mission169;
using UnityEngine;

public class die_before_truck : Achievement {
    void Start() {
        EventManager.StartListening(GlobalEvents.GameOver, OnGameOver);
    }

    void OnGameOver() {
       // not so nice ...  really specific
       if( GameManager.Instance.GetPlayer().transform.GetChild(0).transform.position.x < 8){
           GrantAchievement();
       } 
    }

}