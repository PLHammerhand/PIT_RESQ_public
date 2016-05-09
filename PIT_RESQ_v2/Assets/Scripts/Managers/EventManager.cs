using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
	private Dictionary<string, UnityEvent>           __events;


	public override void Initialize()
	{
		__events = new Dictionary<string, UnityEvent>();

		ready = true;
	}

	public void StartListening(string eventName, UnityAction listener)
	{
		UnityEvent usedEvent;

		if(__events.TryGetValue(eventName, out usedEvent))
		{
			usedEvent.AddListener(listener);
		}
		else
		{
			usedEvent = new UnityEvent();
			usedEvent.AddListener(listener);
			__events.Add(eventName, usedEvent);
		}
	}

	public void StopListening(string eventName, UnityAction listener)
	{
		UnityEvent usedEvent;

		if(__events.TryGetValue(eventName, out usedEvent))
		{
			usedEvent.RemoveListener(listener);
		}
	}

	public void TriggerEvent(string eventName)
	{
		UnityEvent usedEvent;

		if(__events.TryGetValue(eventName, out usedEvent))
		{
			usedEvent.Invoke();
		}
	}
}
