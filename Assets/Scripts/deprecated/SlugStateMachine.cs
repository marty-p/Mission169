using UnityEngine;

public class SlugStateMachine {

    private SlugState currentBehaviour;
    private SlugState nextBehaviour;
    private SlugState[] enemyBehaviours;

    public SlugStateMachine(SlugState[] states, SlugState InitialState) {
        enemyBehaviours = states;
        nextBehaviour = InitialState;
    }

    public void Update() {
       for (int i=0; i<enemyBehaviours.Length; i++) {
            SlugState e = enemyBehaviours[i];
             if(e.resting) {
                e.restingTime += Time.deltaTime;
                if (e.restingTime > e.requiredRestingTime) {
                    e.resting = false;
                    e.restingTime = 0;
                }
            }
        }

        if (nextBehaviour != null) {
            currentBehaviour = nextBehaviour;
            currentBehaviour.StartBehaviour();
            nextBehaviour = null;
        } else if (currentBehaviour.UpdateBehaviour()) {
            currentBehaviour.EndBehaviour();
            currentBehaviour.resting = true;
            if (currentBehaviour.defaultDestinationBehaviour != null) {
                nextBehaviour = currentBehaviour.defaultDestinationBehaviour;
                return;
            }
        }

        SlugState destinationBehaviour = currentBehaviour.EvaluateTransitions();
        if(destinationBehaviour != currentBehaviour) {
            SetState(destinationBehaviour);
        }
    }

    public void SetState(SlugState state) {
        if (currentBehaviour != null) {
            currentBehaviour.EndBehaviour();
            currentBehaviour.resting = true;
        } else {
            Debug.Log(enemyBehaviours.Length);
        }
        nextBehaviour = state;
    }
}

public struct SlugStateTransition {
    public SlugState destination;
    public TransitionCondition condition;
}

public delegate bool TransitionCondition();