using UnityEngine;

public class GridCell : MonoBehaviour
{
	[Header("Cell Coordinates"), Space]
	[SerializeField] private int x;
	[SerializeField] private int y;

	private void OnMouseDown()
	{
		GameManager.Instance.GridCellClicked(x, y);
	}
}
