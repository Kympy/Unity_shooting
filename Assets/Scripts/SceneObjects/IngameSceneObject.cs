using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class IngameSceneObject : SceneObject
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.Instance.LoadScene(ManagedSceneIndex.Title); // 게임 종료
			return;
		}
		if (currentPlayer == null) return;
		if (uiCrossHair == null) return;
		if (uiGameInfo == null) return;
	}
	public override void DestroyScene()
	{
		ReleaseIngameResources();
		UtilFunction.DestroyIfNotNull(uiCrossHair);
		UtilFunction.DestroyIfNotNull(uiGameInfo);
		UtilFunction.DestroyIfNotNull(uiGameResult);
	}
	private Camera ingameCamera = null;
	private CameraControl ingameCameraControl = null;
	public override async void InitScene()
	{
		await LoadSceneUI();
		uiCrossHair.Enable();
		uiGameInfo.Enable();

		await LoadIngameResources();
		CreatePlayer();

		ingameCamera = Camera.main;
		if (ingameCamera.TryGetComponent(out ingameCameraControl) == false)
		{
			ingameCameraControl = ingameCamera.AddComponent<CameraControl>();
		}
		ingameCameraControl.SetTarget(currentPlayer.transform);

		GameManager.Instance.UI.ToggleLoadingUI(false);

		await InitPool();
	}
	private UICrossHair uiCrossHair = null;
	private UIGameInfo uiGameInfo = null;
	private UIGameResult uiGameResult = null;
	public override async Task LoadSceneUI()
	{
		uiCrossHair = await GameManager.Instance.UI.CreateUI<UICrossHair>("Assets/Resources_moved/UI/CrossHair.prefab", UILayer.Normal);
		uiGameInfo = await GameManager.Instance.UI.CreateUI<UIGameInfo>("Assets/Resources_moved/UI/GameInfo.prefab", UILayer.Normal);
		uiGameResult = await GameManager.Instance.UI.CreateUI<UIGameResult>("Assets/Resources_moved/UI/GameResult.prefab", UILayer.Top);
	}
	private GameObject playerPrefab = null;
	private Missile missile = null;
	public GameObject ExplosionEffect { get; private set; } = null;
	public async Task LoadIngameResources()
	{
		playerPrefab = await ResourceManager.LoadGameObject("Player/PlayerCharacter");
		missile = await ResourceManager.LoadGameObjectAndGetComponent<Missile>("Bullet/Missile");
		ExplosionEffect = await ResourceManager.LoadGameObject("Effect/Explosion");
	}
	public void ReleaseIngameResources()
	{
		ResourceManager.ReleaseIfNotNull(playerPrefab);
		ResourceManager.ReleaseIfNotNull(missile.gameObject);
		ResourceManager.ReleaseIfNotNull(ExplosionEffect);
	}
	private Player currentPlayer = null;
	public void CreatePlayer()
	{
		if (currentPlayer != null)
		{
			Destroy(currentPlayer.gameObject);
		}
		currentPlayer = Instantiate(playerPrefab).GetComponent<Player>();
	}
	public void NotifyChangedAttackMode(AttackMode argMode)
	{
		uiCrossHair.SwitchCrossHair(argMode);
		uiGameInfo.DisplayAttackMode(argMode);
	}
	public void NotifyChangedVelocity(float value)
	{
		uiGameInfo.DisplaySpeed(value);
	}
	public void NotifyChangedHeight(float value)
	{
		uiGameInfo.DisplayHeight(value);
	}
	public void CreateExplosionEffectAtPoint(Transform explosionPoint)
	{
		if (ExplosionEffect == null) return;
		if (explosionPoint == null) return;

		Instantiate(ExplosionEffect, explosionPoint.transform.position, explosionPoint.transform.rotation);
	}
	private int gameScore = 0;
	public void AddScore(int value)
	{
		gameScore += value;
	}
	public void ResetScore()
	{
		gameScore = 0;
	}
	public GameObjectPool GameObjectPool { get; private set; } = null;
	private async Task InitPool()
	{
		GameObjectPool = new GameObject(typeof(GameObjectPool).Name, typeof(GameObjectPool)).GetComponent<GameObjectPool>();
		await GameObjectPool.CreateNewQueue(PoolObjectKey.Bullet);
	}
}
