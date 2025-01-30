using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public event EventHandler<GridCellClickedEventArgs> OnGridCellClicked;

	public void GridCellClicked(int coordX, int coordY)
	{
		Debug.Log($"Click cell: [{coordX}, {coordY}]");

		OnGridCellClicked?.Invoke(this, new GridCellClickedEventArgs()
		{
			x = coordX,
			y = coordY
		});
	}
}

public class GridCellClickedEventArgs : EventArgs
{
	public int x;
	public int y;
}