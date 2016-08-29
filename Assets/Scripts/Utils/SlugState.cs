using System.Collections.Generic;

public abstract class SlugState {

    private List<SlugStateTransition> transitions;
    public SlugState defaultDestinationBehaviour;

    public bool resting;
    public float restingTime;
    public float requiredRestingTime;

    public abstract void StartBehaviour();
    public abstract bool UpdateBehaviour();
    public abstract void EndBehaviour();

    public SlugState () {
        transitions = new List<SlugStateTransition>();
    }

    public SlugState EvaluateTransitions() {
        for (int i=0; i<transitions.Count; i++) {
            if(transitions[i].condition()  && !transitions[i].destination.resting) {
                return transitions[i].destination;
            }
        }
        return this;
    }

    public void AddTransition(SlugStateTransition transition) {
        transitions.Add(transition);
    }

}
