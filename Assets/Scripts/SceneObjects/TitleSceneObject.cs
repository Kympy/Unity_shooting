using System.Threading.Tasks;
using UnityEngine;

public class TitleSceneObject : SceneObject
{
	public override async void InitScene()
	{
		GameManager.Instance.UI.ToggleLoadingUI(false);
		// �ӽ� ���� �̹��� : �ε� �۾� ���� ����
		GameObject tempBlackPanel = GameObject.Find("TempBlack");
		
		await LoadSceneUI();
		uiTitle.Enable();

		Destroy(tempBlackPanel);
	}
	private UITitle uiTitle = null;
	public override async Task LoadSceneUI()
	{
		var handle = GameManager.Instance.UI.CreateUI<UITitle>("Assets/Resources_moved/UI/Title.prefab", UILayer.Normal);
		await handle;
		uiTitle = handle.Result;
	}
	public override void DestroyScene()
	{
		UtilFunction.DestroyIfNotNull(uiTitle.gameObject);
	}
}
