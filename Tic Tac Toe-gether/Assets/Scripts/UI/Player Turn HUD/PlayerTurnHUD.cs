using System;
using UnityEngine;

public class PlayerTurnHUD : MonoBehaviour
{
	[Header("Mark UI Groups"), Space]
	[SerializeField] private MarkUIGroup crossMarkUI;
	[SerializeField] private MarkUIGroup noughtMarkUI;

	private void Awake()
	{
		crossMarkUI.SetActiveAll(false);
		noughtMarkUI.SetActiveAll(false);
	}

	private void Start()
	{
		GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
		GameManager.Instance.OnPlayerTurnChanged += GameManager_OnPlayerTurnChanged;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameStarted -= GameManager_OnGameStarted;
		GameManager.Instance.OnPlayerTurnChanged -= GameManager_OnPlayerTurnChanged;
	}

	private void GameManager_OnGameStarted(object sender, EventArgs e)
	{
		MarkType localMark = GameManager.Instance.LocalMarkType;

		crossMarkUI.YouText.SetActive(localMark == MarkType.Cross);
		noughtMarkUI.YouText.SetActive(localMark == MarkType.Nought);

		UpdateTurnArrow();
	}

	private void GameManager_OnPlayerTurnChanged(object sender, EventArgs e)
	{
		UpdateTurnArrow();
	}

	private void UpdateTurnArrow()
	{
		MarkType currentMark = GameManager.Instance.CurrentPlayableMarkType;

		crossMarkUI.TurnArrow.SetActive(currentMark == MarkType.Cross);
		noughtMarkUI.TurnArrow.SetActive(currentMark == MarkType.Nought);
	}
}