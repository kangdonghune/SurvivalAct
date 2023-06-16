using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneManagerEx
{
    public BaseScene CurScene { get { return GameObject.FindObjectOfType<BaseScene>(); } } 

    public void LoadScene(SceneType type)
    {
        Managers.Clear();
        SceneManager.LoadScene(type.ToString()); // Define�� enum���� �����ص� �̸��� �������� �� �̵�
    }

    public void Clear()
    {
        CurScene.Clear(); // ���� ������ ����� ���� �� clear�� ȣ���ؼ� ����
    }
}
