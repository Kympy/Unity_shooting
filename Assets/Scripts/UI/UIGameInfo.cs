using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGameInfo : UIBase
{
	[SerializeField] private TextMeshProUGUI attackMode; // ���� ���
	[SerializeField] private TextMeshProUGUI speed; // �ӵ�
	[SerializeField] private TextMeshProUGUI height; // ��
	[SerializeField] private TextMeshProUGUI score; // ����

	[SerializeField] private Slider HPbar; // ü�� ��

	private Color basicColor; // �⺻ �׸� �ʷϻ�

	private void Awake()
	{
		basicColor = height.color;
	}
	protected override void BeforeEnable()
	{
		base.BeforeEnable();
		attackMode.text = "";
		speed.text = "SPEED : 0";
		height.text = "HEIGHT : 0";
		score.text = "SCORE : 0";
		HPbar.value = 1f;
	}
	public void DisplayAttackMode(AttackMode mode)
	{
		attackMode.text = $"{mode.ToString().ToUpper()} MODE";
	}
	public void DisplaySpeed(float value)
	{
		speed.text = $"SPEED : {string.Format("{0:#,###}",value)} km/s";
	}
	public void DisplayHeight(float value)
	{
		if (value < 150.0f)
		{
			height.color = Color.red;
		}
		else if (value < 230.0f)
		{
			height.color = Color.yellow;
		}
		else
		{
			height.color = basicColor;
		}
		height.text = $"HEIGHT : {value} m";
	}
	public void DisplayScore(float value)
	{
		score.text = $"SCORE : {value}";
	}
}
