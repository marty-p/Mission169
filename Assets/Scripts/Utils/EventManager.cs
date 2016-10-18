using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace SlugLib {

    public enum GlobalEvents {
        PlayerDead,
        PlayerSpawned,
        BossStart,
        BossDead,
        MissionSuccess,
        GameOver,
        SoldierDead,
        BerserkerDead,
        PointsEarned,
        KnifeUsed,
        GunUsed,
        GrenadeUsed,
        WaveEventEnd,
        PlayerInactive,
        MissionStart,
    }

    [System.Serializable]
    public class TransformEvent : UnityEvent<Transform> { }
    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    // This class initially comes from a tutorial on Unity.com but heavily adapted
    public class EventManager : Singleton<EventManager> {

        private Dictionary<GlobalEvents, UnityEvent> eventDictionary;
        private Dictionary<GlobalEvents, TransformEvent> transformEventDictionary;
        private Dictionary<GlobalEvents, FloatEvent> floatEventDictionary;

        void Awake() {
            DontDestroyOnLoad(this);
            Init();
        }

        void Init() {
            if (eventDictionary == null) {
                eventDictionary = new Dictionary<GlobalEvents, UnityEvent>();
                transformEventDictionary = new Dictionary<GlobalEvents, TransformEvent>();
                floatEventDictionary = new Dictionary<GlobalEvents, FloatEvent>();
            }
        }

        public void StartListening(GlobalEvents eventName, UnityAction listener) {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                eventDictionary.Add(eventName, thisEvent);
            }
        }

        public void StartListening(GlobalEvents eventName, UnityAction<Transform> listener) {
            TransformEvent thisEvent = null;
            if (transformEventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new TransformEvent();
                thisEvent.AddListener(listener);
                transformEventDictionary.Add(eventName, thisEvent);
            }
        }

        public void StartListening(GlobalEvents eventName, UnityAction<float> listener) {
            FloatEvent thisEvent = null;
            if (floatEventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new FloatEvent();
                thisEvent.AddListener(listener);
                floatEventDictionary.Add(eventName, thisEvent);
            }
        }

        public void StopListening(GlobalEvents eventName, UnityAction listener) {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.RemoveListener(listener);
            }
        }

        public void TriggerEvent(GlobalEvents eventName) {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.Invoke();
            }
        }

        public void TriggerEvent(GlobalEvents eventName, Transform transform) {
            TransformEvent thisEvent = null;
            if (transformEventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.Invoke(transform);
            }
        }

        public void TriggerEvent(GlobalEvents eventName, float val) {
            FloatEvent thisEvent = null;
            if (floatEventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.Invoke(val);
            }
        }
    }
}
