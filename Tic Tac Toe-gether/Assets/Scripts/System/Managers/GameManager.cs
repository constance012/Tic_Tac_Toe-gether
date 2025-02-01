using System;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkSingleton<GameManager>
{
	public event EventHandler<GridCellClickedEventArgs> OnGridCellClicked;
	public event EventHandler OnGameStarted;
	public event EventHandler OnPlayerTurnChanged;

	public MarkType LocalMarkType => _localMarkType;
	public MarkType CurrentPlayableMarkType => _currentPlayableMarkType;

	// Private fields.
	private MarkType _localMarkType;
	private MarkType _currentPlayableMarkType = MarkType.None;

	// Call when the network connection is first established.
	public override void OnNetworkSpawn()
	{
		_localMarkType = (MarkType)NetworkManager.Singleton.LocalClientId;

		if (IsServer)
		{
			NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
		}
	}

	#region RPC methods.
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

			TriggerOnPlayerTurnChangedEventRpc();
		}
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void TriggerOnGameStartedEventRpc()
	{
		OnGameStarted?.Invoke(this, EventArgs.Empty);
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void TriggerOnPlayerTurnChangedEventRpc()
	{
		OnPlayerTurnChanged?.Invoke(this, EventArgs.Empty);
	}
	#endregion

	private void NetworkManager_OnClientConnectedCallback(ulong clientID)
	{
		if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
		{
			_currentPlayableMarkType = _localMarkType;

			TriggerOnGameStartedEventRpc();
		}
	}
}

public class GridCellClickedEventArgs : EventArgs
{
	public int x;
	public int y;
	public MarkType markType;
}