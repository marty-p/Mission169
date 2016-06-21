namespace Slug.StateMachine {

    using UnityEngine;
    using Utils;

    public class Door {

        public RetVoidTakeVoid destination;
        public RetBoolTakeVoid openCondition;

        public float lastExitTimeStamp;
        public float lockoutDuration;

        public Door (RetVoidTakeVoid destination, RetBoolTakeVoid openCondition, float lockoutDuration=0) {
            this.destination = destination;
            this.openCondition = openCondition;
            this.lockoutDuration = lockoutDuration;
        }

        public Door () {}

        public bool Openable() {
            if (!TimeLocked()) {
                return openCondition();
            }
            return false;
        }

        public void Open() {
            destination();
        }

        private bool TimeLocked() {
            return Time.time - lastExitTimeStamp < lockoutDuration;
        }

        public void TimeLock() {
            lastExitTimeStamp = Time.time;
        }
    }

}