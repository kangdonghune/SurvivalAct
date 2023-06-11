using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public abstract class BaseScene : MonoBehaviour
{
    public SceneType  Type { get; protected set; } = SceneType.None;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    public abstract void Clear();
}
