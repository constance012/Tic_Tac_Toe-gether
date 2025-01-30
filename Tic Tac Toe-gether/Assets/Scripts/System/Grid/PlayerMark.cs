using UnityEngine;

public class PlayerMark : MonoBehaviour, IPoolable
{
	public void Allocate()
	{
		gameObject.SetActive(true);
	}

	public void Deallocate()
	{
		gameObject.SetActive(false);
	}
}