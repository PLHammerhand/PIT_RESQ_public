using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
	public enum GameEventType
	{
		DEFEAT,
		VICTORY,
		START,
		WAVE,
	}

	private Dictionary<GameEventType, UnityEvent>           __events;


	public override void Initialize()
	{
		__events = new Dictionary<GameEventType, UnityEvent>();

		ready = true;
	}

	public void StartListening(GameEventType eventType, UnityAction listener)
	{
		UnityEvent usedEvent;

		if(__events.TryGetValue(eventType, out usedEvent))
		{
			usedEvent.AddListener(listener);
		}
		else
		{
			usedEvent = new UnityEvent();
			usedEvent.AddListener(listener);
			__events.Add(eventType, usedEvent);
		}
	}

	public void StopListening(GameEventType eventType, UnityAction listener)
	{
		UnityEvent usedEvent;

		if(__events.TryGetValue(eventType, out usedEvent))
		{
			usedEvent.RemoveListener(listener);
		}
	}

	public void TriggerEvent(GameEventType eventType)
	{
		UnityEvent usedEvent;

		if(__events.TryGetValue(eventType, out usedEvent))
		{
			usedEvent.Invoke();
		}
	}
}
