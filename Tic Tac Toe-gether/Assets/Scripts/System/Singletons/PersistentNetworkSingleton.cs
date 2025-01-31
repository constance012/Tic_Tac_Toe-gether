using Unity.Netcode;

/// <summary>
/// Makes a permanent network singleton reference for the entire game session, which persists between different scenes.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PersistentNetworkSingleton<T> : NetworkSingleton<T> where T : NetworkBehaviour
{
	protected override void SetInstance()
	{
		base.SetInstance();
		DontDestroyOnLoad(gameObject);
	}
}
