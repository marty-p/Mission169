namespace Slug.StateMachine {

    using UnityEngine;
    using System.Collections.Generic;

    public abstract class Room : StateMachineBehaviour {

        protected List<Door> exitDoors = new List<Door>();
        protected AnimDrivenBrain brain;
        private Door lastDoorUsed;
        protected Animator animator;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (brain == null) {
                brain = animator.GetComponent<AnimDrivenBrain>();
                Init();
                this.animator = animator;
            }
            if (lastDoorUsed != null) {
                lastDoorUsed.TimeLock();
            }
            Enter();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            // Can I leave this room ?
            for (int i=0; i<exitDoors.Count; i++) {
                if (exitDoors[i].Openable()) {
                    exitDoors[i].Open();
                    lastDoorUsed = exitDoors[i];
                }
            }
            Update();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Exit();
        }

        public void AddExitDoor(Door door) {
            exitDoors.Add(door);
        }

        // don't want to force to override and ...
        // don't want to forget to add base.OnStateEnter so doing that
        virtual public void Init() { }
        virtual public void Enter() { }
        virtual public void Update() { }
        virtual public void Exit() { }

        public void print(string str) {
            Debug.Log(str);
        }
    }

}
