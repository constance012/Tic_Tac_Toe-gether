using UnityEngine;
using Unity.Netcode;

public class LobbyUI : MonoBehaviour
{
	public void StartHost()
	{
		NetworkManager.Singleton.StartHost();
		gameObject.SetActive(false);
	}

	public void StartClient()
	{
		NetworkManager.Singleton.StartClient();
		gameObject.SetActive(false);
	}
}