using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace SlugLib {

    public enum GlobalEvents {
        PlayerDead,
        PlayerSpawned,
        PlayerStabbed,
        PlayerInactive,

        BossStart,
        BossDead,
        SoldierDead,
        BerserkerDead,

        PointsEarned,
        KnifeUsed,
        GunUsed,
        GrenadeUsed,
        WaveEventEnd,

        MissionStartRequest,
        MissionStart,
        MissionEnd, // the end regardless of the success/failure
        MissionSuccess,

        GameOver,
        ItemPickedUp,
        Home
    }

    [System.Serializable]
    public class TransformEvent : UnityEvent<Transform> { }
    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    // This class initially comes from a tutorial on Unity.com but heavily adapted
    public class EventManager {

        private static Dictionary<GlobalEvents, UnityEvent> eventDictionary = new Dictionary<GlobalEvents, UnityEvent>();
        private static Dictionary<GlobalEvents, TransformEvent> transformEventDictionary = new Dictionary<GlobalEvents, TransformEvent>();
        private static Dictionary<GlobalEvents, FloatEvent> floatEventDictionary = new Dictionary<GlobalEvents, FloatEvent>();

        public static void StartListening(GlobalEvents eventName, UnityAction listener) {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StartListening(GlobalEvents eventName, UnityAction<Transform> listener) {
            TransformEvent thisEvent = null;
            if (transformEventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new TransformEvent();
                thisEvent.AddListener(listener);
                transformEventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StartListening(GlobalEvents eventName, UnityAction<float> listener) {
            FloatEvent thisEvent = null;
            if (floatEventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new FloatEvent();
                thisEvent.AddListener(listener);
                floatEventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(GlobalEvents eventName, UnityAction listener) {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(GlobalEvents eventName) {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.Invoke();
            }
        }

        public static void TriggerEvent(GlobalEvents eventName, Transform transform) {
            TransformEvent thisEvent = null;
            if (transformEventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.Invoke(transform);
            }
        }

        public static void TriggerEvent(GlobalEvents eventName, float val) {
            FloatEvent thisEvent = null;
            if (floatEventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.Invoke(val);
            }
        }
    }
}
