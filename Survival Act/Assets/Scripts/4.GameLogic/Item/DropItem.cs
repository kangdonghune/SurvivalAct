using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private static WaitForSeconds _wait = new WaitForSeconds(30f); // ������ ������ ���� �ڷ�ƾ ȣ�⵵ ������ �ƿ� static���� ����
    private static WaitForEndOfFrame _waitFrame = new WaitForEndOfFrame(); 
    private void OnEnable()
    {
        StartCoroutine(CoLiveCount());
        //OnEnable �� �� pool���� �������µ� �̶� Ʈ������ ���� ���� �Ҹ� �� ��ġ. �� ������ ���� init�Ǹ� �׸��忡 �߰�
        StartCoroutine(CoAddGrid());   
    }

    private void OnDisable()
    {
        StopAllCoroutines(); // �÷��̾ �����ϴ� ������ ��Ȱ��ȭ �Ǿ��� �� ���� ���� �ִ� �ڷ�ƾ�� �ߴ�
        if(Managers.Game?.Grid != null)
            Managers.Game.Grid.Remove(gameObject); // �׸��忡���� ����
    }

    IEnumerator CoLiveCount()
    {
        yield return _wait;
        gameObject.SetActive(false);
    }

    IEnumerator CoAddGrid()
    {
        yield return _waitFrame;
        if(Managers.Game?.Grid != null)
            Managers.Game.Grid.Add(gameObject); // �׸��忡 �߰�
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}

