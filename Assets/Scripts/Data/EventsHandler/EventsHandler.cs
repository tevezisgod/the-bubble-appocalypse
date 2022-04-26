using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Data.EventsHandler
{
   public class EventsHandler : MonoBehaviour
   {
      private Dictionary<string, UnityEvent> m_EventsDictionary;

      private static EventsHandler _eventsHandler;

      [SerializeField] private EventNames eventNames;
      public static EventNames EventNames;

      public static EventsHandler Instance;

      private void Awake()
      {
         DontDestroyOnLoad(gameObject);
         if (!Instance)
         {
            _eventsHandler = this;
            _eventsHandler.Init();
            Instance = _eventsHandler;
         } 
         else Destroy(gameObject);
      }

      private void Init()
      {
         m_EventsDictionary ??= new Dictionary<string, UnityEvent>();
         EventNames = eventNames;
      }

      public static void StartListening(string eventName, UnityAction listener)
      {
         if (Instance.m_EventsDictionary.TryGetValue(eventName, out var thisEvent))
         {
            thisEvent.AddListener(listener);
         }
         else
         {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.m_EventsDictionary.Add(eventName,thisEvent);
         }
      }

      public static void StopListening(string eventName, UnityAction listener)
      {
         if (_eventsHandler == null) return;
         if (Instance.m_EventsDictionary.TryGetValue(eventName, out var thisEvent))
         {
            thisEvent.RemoveListener(listener);
         }
         else Debug.Log($"no such event to stop listening {eventName}");
      }

      public static void TriggerEvent(string eventName)
      {
         if (Instance.m_EventsDictionary.TryGetValue(eventName, out var thisEvent))
         {
            thisEvent.Invoke();
         }
         else Debug.Log($"no such event to invoke {eventName}");
      }
   }
}
