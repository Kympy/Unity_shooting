using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
	[SerializeField] private Image progressBar = null;

	public void SetLoadingValue(float progress)
	{
		if (progressBar == null) return;

		progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, 0.5f);
	}
}
