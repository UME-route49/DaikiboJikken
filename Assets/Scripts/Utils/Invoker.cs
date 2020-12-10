using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Invokable();

public class Invoker : MonoBehaviour {
	private struct InvokableItem
	{
		public Invokable Func;
		public float ExecuteAtTime;
		public static Invoker Instance = null;
		
		public InvokableItem(Invokable func, float delaySeconds)
		{
			this.Func = func;
			
			// realtimeSinceStartup is never 0, and Time.time is affected by timescale (though it is 0 on startup).  Use a combination 
			// http://forum.unity3d.com/threads/205773-realtimeSinceStartup-is-not-0-in-first-Awake()-call
			if(Time.time == 0) this.ExecuteAtTime = delaySeconds;
			else this.ExecuteAtTime = Time.realtimeSinceStartup + delaySeconds;
		}
	}
	
	private static Invoker _instance = null;
	public static Invoker Instance
	{
		get
		{
			if( _instance == null )
			{
				GameObject go = new GameObject();
				go.AddComponent<Invoker>();
				go.name = "_FunoniumInvoker";
				_instance = go.GetComponent<Invoker>();
			}
			return _instance;
		}
	}
	
	float fRealTimeLastFrame = 0;
	float fRealDeltaTime = 0;
	
	List<InvokableItem> invokeList = new List<InvokableItem>();
	List<InvokableItem> invokeListPendingAddition = new List<InvokableItem>();
	List<InvokableItem> invokeListExecuted = new List<InvokableItem>();
	
	public float RealDeltaTime()
	{
		return fRealDeltaTime;	
	}
	/// Invokes the function with a time delay.  This is NOT 
	/// affected by timeScale like the Invoke function in Unity.
	/// <param name='func'>Function to invoke</param>
	/// <param name='delaySeconds'>Delay in seconds.</param>
	public static void InvokeDelayed(Invokable func, float delaySeconds)
	{
		Instance.invokeListPendingAddition.Add(new InvokableItem(func, delaySeconds));
	}
	
	// must be maanually called from a game controller or something similar every frame
	public void Update()
	{
		fRealDeltaTime = Time.realtimeSinceStartup - fRealTimeLastFrame;
		fRealTimeLastFrame = Time.realtimeSinceStartup;
		
		// Copy pending additions into the list (Pending addition list 
		// is used because some invokes add a recurring invoke, and
		// this would modify the collection in the next loop, 
		// generating errors)
		foreach(InvokableItem item in invokeListPendingAddition)
		{
			invokeList.Add(item);	
		}
		invokeListPendingAddition.Clear();
		
		
		// Invoke all items whose time is up
		foreach(InvokableItem item in invokeList)
		{
			if(item.ExecuteAtTime <= Time.realtimeSinceStartup)	
			{
				if(item.Func != null)
					item.Func();
				
				invokeListExecuted.Add(item);
			}
		}
		
		// Remove invoked items from the list.
		foreach(InvokableItem item in invokeListExecuted)
		{
			invokeList.Remove(item);
		}
		invokeListExecuted.Clear();
	}
}