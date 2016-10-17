using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

[System.Serializable]
public class TransformEvent : UnityEvent<Transform>{}
[System.Serializable]
public class FloatEvent : UnityEvent<float>{}

// This class initially comes from a tutorial on Unity.com, not many lines from
// the original version are still there though.
public class EventManager : Singleton<EventManager>{

    private Dictionary<string, UnityEvent> eventDictionary;
    private Dictionary<string, TransformEvent> transformEventDictionary;
    private Dictionary<string, FloatEvent> floatEventDictionary;

    void Awake() {
        DontDestroyOnLoad(this);
        Init();
    }

    void Init() {
        if (eventDictionary == null) {
            eventDictionary = new Dictionary<string, UnityEvent>();
            transformEventDictionary = new Dictionary<string, TransformEvent>();
            floatEventDictionary = new Dictionary<string, FloatEvent>();
        }
    }

    public void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StartListening(string eventName, UnityAction<Transform> listener) {
        TransformEvent thisEvent = null;
        if (transformEventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new TransformEvent();
            thisEvent.AddListener(listener);
            transformEventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StartListening(string eventName, UnityAction<float> listener) {
        FloatEvent thisEvent = null;
        if (floatEventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new FloatEvent();
            thisEvent.AddListener(listener);
            floatEventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StopListening(string eventName, UnityAction listener) {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    public void TriggerEvent(string eventName) {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke();
        }
    }

    public void TriggerEvent(string eventName, Transform transform) {
        TransformEvent thisEvent = null;
        if (transformEventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke(transform);
        }
    }

   public void TriggerEvent(string eventName, float val) {
        FloatEvent thisEvent = null;
        if (floatEventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke(val);
        }
    }
}