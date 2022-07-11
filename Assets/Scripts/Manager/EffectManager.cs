using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private GameObject explodeEffect;

    public void Init()
    {
        explodeEffect = Resources.Load("Effect/Explosion") as GameObject;
    }
    public GameObject GetExplosion()
    {
        return explodeEffect;
    }
}
