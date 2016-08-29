using UnityEngine;
using System.Collections.Generic;

public class EnemyBehaviorManager : MonoBehaviour {

    private List <EBehaviour> behaviors;
    private List <EBehaviour> behaviorsKeenToStart;

	void Start () {
	    behaviors = new List<EBehaviour>();
        behaviorsKeenToStart = new List<EBehaviour>();
        behaviors.AddRange(GetComponents<EBehaviour>());
	}
	
	void Update () {
        if (behaviors.Count < 1) {
            return;
        }
/*
        if (behaviorInProgress != null) {
            if (behaviorInProgress.InProgress) {
                behaviorInProgress.UpdateBehavior();
                if (!behaviorInProgress.interruptible) {
                    //print(" Can't be interrupted "+ behaviorInProgress);
                    return;
                }
            } else {
                behaviorInProgress = null;
            }
        }
*/
        PopulateBehaviorsKeenToStart();
        EBehaviour highestKeen = FindHighestPriorityBehaviour();
        if (highestKeen != null) {
            if (ExclusiveBehaviourRunning()) {
                return;
            }

            if (highestKeen.exclusive) {
                StopAllBehaviours();
            }

            highestKeen.enabled = true;
        }
	}

    private void PopulateBehaviorsKeenToStart() {
        behaviorsKeenToStart.Clear();
        for (int i = 0; i < behaviors.Count; i++) {
            // No point trying to start a behavior that's already running
            if (behaviors[i].enabled) {
                //print("Same behavior already runnig, skipping");
                continue;
            }
            if (behaviors[i].WantToStart()) {
                //print(" want to start: " + behaviors[i]);
                behaviorsKeenToStart.Add(behaviors[i]);
            }
        }
    }

    private EBehaviour FindHighestPriorityBehaviour() {
        float highestPriority = 0;
        EBehaviour behaviorOfHighestPriority = null;
        for (int i = 0; i < behaviorsKeenToStart.Count; i++) {
            if (behaviorsKeenToStart[i].priority > highestPriority) {
                behaviorOfHighestPriority = behaviorsKeenToStart[i];
                highestPriority = behaviorsKeenToStart[i].priority;
            }
        }
        return behaviorOfHighestPriority;
    }

    private bool ExclusiveBehaviourRunning() {
        for (int i = 0; i < behaviors.Count; i++) {
            if (behaviors[i].enabled && behaviors[i].exclusive) {
                return true;
            }
        }
        return false;
    }

    private void StopAllBehaviours() {
         for (int i = 0; i < behaviors.Count; i++) {
            behaviors[i].enabled = false;
         }

    }

}
