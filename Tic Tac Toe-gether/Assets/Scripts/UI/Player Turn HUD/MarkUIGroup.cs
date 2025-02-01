using UnityEngine;

public class MarkUIGroup : MonoBehaviour
{
	[Header("Game Objects"), Space]
	[SerializeField] private GameObject turnArrow;
	[SerializeField] private GameObject youText;

	public GameObject TurnArrow => turnArrow;
	public GameObject YouText => youText;

	public void SetActiveAll(bool state)
	{
		turnArrow.SetActive(state);
		youText.SetActive(state);
	}
}