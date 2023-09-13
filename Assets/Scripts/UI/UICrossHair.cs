using UnityEngine;

public class UICrossHair : UIBase
{
	[SerializeField] private GameObject gunCrossHair = null;
	[SerializeField] private GameObject missileCrossHair = null;

	private GameObject currentCrossHair = null;
	private void Awake()
	{
		gunCrossHair.SetActive(false);
		missileCrossHair.SetActive(false);
	}
	protected override void AfterEnable()
	{
		base.AfterEnable();
		SwitchCrossHair(AttackMode.Gun);
	}
	public void SwitchCrossHair(AttackMode mode)
	{
		if (currentCrossHair != null)
		{
			currentCrossHair.SetActive(false);
		}
		switch(mode)
		{
			case AttackMode.Gun:
				gunCrossHair.SetActive(true);
				currentCrossHair = gunCrossHair;
				break;
			case AttackMode.Missile:
				missileCrossHair.SetActive(true);
				currentCrossHair = missileCrossHair;
				break;
		}
	}
}
