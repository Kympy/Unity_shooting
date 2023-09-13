using System.Threading.Tasks;
using UnityEngine;

public abstract class SceneObject : MonoBehaviour
{
	public abstract void InitScene();
	public virtual async Task LoadSceneUI() { }
	public abstract void DestroyScene();

	protected virtual void OnDestroy() 
	{
		DestroyScene();
	}
}
