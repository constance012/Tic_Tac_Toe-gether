using UnityEngine;

public class GridCell : MonoBehaviour
{
	[Header("Cell Coordinates"), Space]
	[SerializeField] private float x;
	[SerializeField] private float y;

	private void OnMouseDown()
	{
		Debug.Log($"Click cell: [{x}, {y}]");
	}
}
