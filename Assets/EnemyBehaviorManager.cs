using UnityEngine;
using System.Collections.Generic;

public class EnemyBehaviorManager : MonoBehaviour {

    private List <IEnemyBehavior> enemyBehaviors;
    private List<IEnemyBehavior> enemyBehaviorsKeenToStart;
    private IEnemyBehavior behaviorInProgress;

	void Start () {
	    enemyBehaviors = new List<IEnemyBehavior>();
        enemyBehaviorsKeenToStart = new List<IEnemyBehavior>();
        enemyBehaviors.AddRange(GetComponents<IEnemyBehavior>());
	}
	
	void Update () 
    {
        if (enemyBehaviors.Count < 1) {
            return;
        }

        if (behaviorInProgress != null) {
            if (behaviorInProgress.InProgress()) {
                behaviorInProgress.UpdateBehavior();
                if (!behaviorInProgress.CanBeInterrupted()) {
                    //print(" Can't be interrupted "+ behaviorInProgress);
                    return;
                }
            } else {
                behaviorInProgress = null;
            }
        }

        for (int i = 0; i < enemyBehaviors.Count; i++) 
        {
            // No point trying to start a behavior that's already running
            if (enemyBehaviors[i] == behaviorInProgress) {
                //print("Same behavior already runnig, skipping");
                continue;
            }
            //print(" want to start: " + enemyBehaviors[i]);
            if (enemyBehaviors[i].WantToStart()) {
                enemyBehaviorsKeenToStart.Add(enemyBehaviors[i]);
            }
        }

        behaviorInProgress = FindHighestPriorityBehavior();
        // TODO what if enemuBehavoirKeenToStart EMPTY??

        enemyBehaviorsKeenToStart.Clear();

        if (behaviorInProgress != null) {
            behaviorInProgress.StartBehavior();
        }
	}


    private IEnemyBehavior FindHighestPriorityBehavior() {
        float highestPriority = 0;
        IEnemyBehavior behaviorOfHighestPriority = null;
        for (int i = 0; i < enemyBehaviorsKeenToStart.Count; i++) 
        {
            if (enemyBehaviorsKeenToStart[i].GetPriority() > highestPriority) {
                behaviorOfHighestPriority = enemyBehaviorsKeenToStart[i];
                highestPriority = enemyBehaviorsKeenToStart[i].GetPriority();
            }
        }
        return behaviorOfHighestPriority;
    }

}
