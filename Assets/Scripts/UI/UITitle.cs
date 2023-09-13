using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UITitle : UIBase
{
	[SerializeField] private Button startButton = null;
	[SerializeField] private Button exitButton = null;

	protected override void BeforeEnable()
	{
		base.BeforeEnable();
		startButton.onClick.AddListener(() => { GameManager.Instance.LoadScene(ManagedSceneIndex.Ingame); });
		exitButton.onClick.AddListener(() => 
		{
#if !UNITY_EDITOR
			Application.Quit();
#else
			EditorApplication.isPlaying = false;
#endif
		});
	}
	protected override void BeforeDisable()
	{
		base.BeforeDisable();
		startButton.onClick.RemoveAllListeners();
		exitButton.onClick.RemoveAllListeners();
	}
}
