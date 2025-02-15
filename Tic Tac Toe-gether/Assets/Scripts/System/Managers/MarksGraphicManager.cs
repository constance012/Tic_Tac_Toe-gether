using Unity.Netcode;
using UnityEngine;

// Scripts that contain RPC methods must be inherited from the "NetworkBehaviour" class.
public class MarksGraphicManager : NetworkMultiplePrefabsPool<MarksGraphicManager, MarkType, PlayerMark>
{
	private const float GRID_SIZE = 3f;

	private void Start()
	{
		GameManager.Instance.OnGridCellClicked += GameManager_OnGridCellClicked;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGridCellClicked -= GameManager_OnGridCellClicked;
	}

	private void GameManager_OnGridCellClicked(object sender, GridCellClickedEventArgs e)
	{
		Debug.Log("GameManager_OnGridCellClicked");
		SpawnMarkOnGridRpc(e.x, e.y, e.markType);
	}

	// RPC methods signature must end with "Rpc" suffix.
	// RPC methods don't support object-type parameters. If absolutely need to to something like that, use the "NetworkObjectReference" struct.
	[Rpc(SendTo.Server)]
	private void SpawnMarkOnGridRpc(int x, int y, MarkType type)
	{
		Debug.Log("SpawnMarkOnGrid");
		PlayerMark spawnedMark = Spawn(type, ToWorldPosition(x, y), Quaternion.identity);
		
		spawnedMark.GetComponent<NetworkObject>().Spawn(destroyWithScene: true);
	}

	private Vector3 ToWorldPosition(int gridX, int gridY)
	{
		return new Vector3(-GRID_SIZE + gridX * GRID_SIZE, -GRID_SIZE + gridY * GRID_SIZE);
	}
}

public enum MarkType
{
	None = 0,
	Cross = 1,
	Nought = 2
}