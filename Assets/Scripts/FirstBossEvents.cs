using UnityEngine;
using System.Collections;
using System;

public class FirstBossEvents : MonoBehaviour {

    private SlugStateMachine stateMachine;
    public FollowTarget cam;

    void Start () {
        SlugState[] enemyBehaviours = new SlugState[1];
        AStateEvent state = new AStateEvent(cam, this);
        enemyBehaviours[0] = state;
        stateMachine = new SlugStateMachine(enemyBehaviours, state);
        this.enabled = false;
	}

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            this.enabled = true;
        }
    }

	void Update () {
        stateMachine.Update();	
	}
}

public class AStateEvent : SlugState {
    private float elaspedTime;
    private FollowTarget camera;
    private MonoBehaviour monoBehaviour;

    public AStateEvent(FollowTarget camera, MonoBehaviour monoBehaviour) {
        this.monoBehaviour = monoBehaviour;
        this.camera = camera;
    }

    public override void EndBehaviour() {
        camera.followActive = true;
        monoBehaviour.enabled = false;
    }

    public override void StartBehaviour() {
        elaspedTime = 0;
        camera.followActive = false;
    }

    public override bool UpdateBehaviour() {
        elaspedTime += Time.deltaTime;
        if (elaspedTime > 5) {
            return true;
        } else {
            return false;
        }
    }
}

