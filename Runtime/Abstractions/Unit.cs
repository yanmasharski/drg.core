namespace DRG.Core
{
	/// <summary>
	/// Sentinel type for observables that signal a fact without carrying a payload
	/// (e.g. remote config refreshed).
	/// </summary>
	public readonly struct Unit
	{
		public static readonly Unit Value = default;
	}
}