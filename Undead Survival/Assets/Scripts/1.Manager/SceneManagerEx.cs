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
        SceneManager.LoadScene(type.ToString()); // Define�� enum���� �����ص� �̸��� �������� �� �̵�
    }

    public void Clear()
    {
        curScene.Clear(); // ���� ������ ����� ���� �� clear�� ȣ���ؼ� ����
    }
}
