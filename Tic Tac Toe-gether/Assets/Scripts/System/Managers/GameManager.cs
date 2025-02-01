using System;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkSingleton<GameManager>
{
	public event EventHandler<GridCellClickedEventArgs> OnGridCellClicked;

	public MarkType LocalMarkType => _localMarkType;

	// Private fields.
	private MarkType _localMarkType;
	private MarkType _currentPlayableMarkType = MarkType.None;

	// Call when the network connection is first established.
	public override void OnNetworkSpawn()
	{
		Debug.Log($"OnNetworkSpawn: {NetworkManager.Singleton.LocalClientId}");

		_localMarkType = (MarkType)NetworkManager.Singleton.LocalClientId;

		if (IsServer)
		{
			_currentPlayableMarkType = _localMarkType;
		}
	}

	[Rpc(SendTo.Server)]
	public void GridCellClickedRpc(int coordX, int coordY, MarkType currentMarkType)
	{
		if (currentMarkType == _currentPlayableMarkType)
		{
			OnGridCellClicked?.Invoke(this, new GridCellClickedEventArgs()
			{
				x = coordX,
				y = coordY,
				markType = currentMarkType
			});

			_currentPlayableMarkType = _currentPlayableMarkType switch
			{
				MarkType.Nought => MarkType.Cross,
				MarkType.Cross => MarkType.Nought,
				_ => MarkType.Nought,
			};
		}
	}
}

public class GridCellClickedEventArgs : EventArgs
{
	public int x;
	public int y;
	public MarkType markType;
}