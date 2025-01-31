using System;

public class GameManager : MonoSingleton<GameManager>
{
	public event EventHandler<GridCellClickedEventArgs> OnGridCellClicked;

	public void GridCellClicked(int coordX, int coordY)
	{
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