using UnityEngine;
using System.Collections;

public class TimeManager : Singleton<TimeManager>
{
	private bool        __gameplayState				= true;


	public bool gameplayState
	{
		get
		{
			return __gameplayState;
		}
		set
		{
			__gameplayState = value;
		}
	}

	public override void Initialize()
	{
		ready = true;
	}
}
