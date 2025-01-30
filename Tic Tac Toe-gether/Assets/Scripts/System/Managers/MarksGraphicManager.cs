using Unity.Netcode;
using UnityEngine;

public class MarksGraphicManager : MultiplePrefabsPool<MarksGraphicManager, MarkType, PlayerMark>
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
		PlayerMark spawnedMark = Spawn(MarkType.Nought, ToWorldPosition(e.x, e.y), Quaternion.identity);
		
		spawnedMark.GetComponent<NetworkObject>().Spawn(destroyWithScene: true);
	}

	private Vector3 ToWorldPosition(int gridX, int gridY)
	{
		return new Vector3(-GRID_SIZE + gridX * GRID_SIZE, -GRID_SIZE + gridY * GRID_SIZE);
	}
}

public enum MarkType
{
	Cross,
	Nought
}