using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneManagerEx
{
    private BaseScene curScene { get { return GameObject.FindObjectOfType<BaseScene>(); } } 

    public void LoadScene(SceneType type)
    {
        Managers.Clear();
        SceneManager.LoadScene(type.ToString()); // Define에 enum으로 정의해둔 이름을 기준으로 씬 이동
    }

    public void Clear()
    {
        curScene.Clear(); // 현재 씬에서 벗어나기 전에 씬 clear를 호출해서 정리
    }
}
