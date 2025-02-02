using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class GameManager : NetworkSingleton<GameManager>
{
	public event EventHandler<GridCellClickedEventArgs> OnGridCellClicked;
	public event EventHandler OnGameStarted;
	public event EventHandler OnPlayerTurnChanged;

	public MarkType LocalMarkType => _localMarkType;
	public MarkType CurrentPlayableMarkType => _currentPlayableMarkType.Value;

	// Private fields.
	private MarkType[,] _gameBoard;
	private MarkType _localMarkType;
	private NetworkVariable<MarkType> _currentPlayableMarkType = new NetworkVariable<MarkType>(MarkType.None);

	protected override void Awake()
	{
		base.Awake();

		_gameBoard = new MarkType[3, 3];  // Default all cells to value 0, or MarkType.None.
	}

	// Call when the network connection is first established.
	public override void OnNetworkSpawn()
	{
		_localMarkType = (MarkType)(NetworkManager.Singleton.LocalClientId + 1);

		if (IsServer)
		{
			// Only the server can listen to this event.
			NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
		}
		
		// This event will be triggered for both the server and clients.
		_currentPlayableMarkType.OnValueChanged += (MarkType previousMarkTurn, MarkType currentMarkTurn) =>
		{
			OnPlayerTurnChanged?.Invoke(this, EventArgs.Empty);
		};
	}

	#region RPC methods.
	[Rpc(SendTo.Server)]
	public void GridCellClickedRpc(int coordX, int coordY, MarkType currentMarkType)
	{
		if (CanPlaceMarkAtGridCell(coordX, coordY, currentMarkType))
		{
			_gameBoard[coordX, coordY] = currentMarkType;

			OnGridCellClicked?.Invoke(this, new GridCellClickedEventArgs()
			{
				x = coordX,
				y = coordY,
				markType = currentMarkType
			});

			_currentPlayableMarkType.Value = _currentPlayableMarkType.Value switch
			{
				MarkType.Nought => MarkType.Cross,
				MarkType.Cross => MarkType.Nought,
				_ => MarkType.Nought,
			};
		}
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void TriggerOnGameStartedEventRpc()
	{
		OnGameStarted?.Invoke(this, EventArgs.Empty);
	}
	#endregion

	private void NetworkManager_OnClientConnectedCallback(ulong clientID)
	{
		if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
		{
			_currentPlayableMarkType.Value = _localMarkType;

			TriggerOnGameStartedEventRpc();
		}
	}

	private bool CanPlaceMarkAtGridCell(int x, int y, MarkType currentMarkType)
	{
		return currentMarkType == _currentPlayableMarkType.Value && _gameBoard[x, y] == MarkType.None;
	}
}

public class GridCellClickedEventArgs : EventArgs
{
	public int x;
	public int y;
	public MarkType markType;
}