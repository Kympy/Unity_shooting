using System.Threading.Tasks;
using UnityEngine;

public class UIBase : MonoBehaviour
{
	public virtual async void Enable()
	{
		BeforeEnable();
		await BeforeEnableAsync();
		gameObject.SetActive(true);
		transform.SetAsLastSibling();
		AfterEnable();
		await AfterEnableAsync();
	}
	public virtual async Task EnableAsync()
	{
		BeforeEnable();
		await BeforeEnableAsync();
		gameObject.SetActive(true);
		transform.SetAsLastSibling();
		AfterEnable();
		await AfterEnableAsync();
	}
	protected virtual void BeforeEnable() { }
	protected virtual async Task BeforeEnableAsync() { }
	protected virtual void AfterEnable() { }
	protected virtual async Task AfterEnableAsync() { }
	public virtual async void Disable()
	{
		BeforeDisable();
		await BeforeDisableAsync();
		gameObject.SetActive(false);
		AfterDisable();
		await AfterDisableAsync();
	}
	public virtual async Task DisableAsync()
	{
		BeforeDisable();
		await BeforeDisableAsync();
		gameObject.SetActive(false);
		AfterDisable();
		await AfterDisableAsync();
	}
	protected virtual void BeforeDisable() { }
	protected virtual async Task BeforeDisableAsync() { }
	protected virtual void AfterDisable() { }
	protected virtual async Task AfterDisableAsync() { }
}
