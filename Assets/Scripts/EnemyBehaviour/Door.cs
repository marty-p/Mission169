namespace Slug.StateMachine {

    using UnityEngine;
    using Utils;

    public class Door {

        public RetVoidTakeVoid destination;
        public RetBoolTakeVoid openCondition;

        public float lastExitTimeStamp;
        public float lockoutDuration;

        public Door (RetVoidTakeVoid destination, RetBoolTakeVoid openCondition, float lockoutDuration=0.1f) {
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

            //Debug.Log("time locked???? with " + Time.time + " " + lastExitTimeStamp +  " " + (Time.time - lastExitTimeStamp).ToString() + " " + lockoutDuration.ToString());
            return Time.time - lastExitTimeStamp < lockoutDuration;
        }

        public void TimeLock() {
            //Debug.Log("time lock with " + Time.time);
            lastExitTimeStamp = Time.time;
        }
    }

}