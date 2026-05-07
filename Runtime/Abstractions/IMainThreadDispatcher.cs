using System;

namespace DRG.Core
{
	/// <summary>
	/// Schedules an action to run on the Unity main thread from any thread.
	/// Used by platform adapters to marshal SDK callbacks back to the main thread
	/// before resolving a TaskCompletionSource.
	/// </summary>
	public interface IMainThreadDispatcher
	{
		void Dispatch(Action action);
	}
}
