using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private static WaitForSeconds _wait = new WaitForSeconds(30f); // 동전은 개수가 많고 코루틴 호출도 많으니 아예 static으로 관리
    private static WaitForEndOfFrame _waitFrame = new WaitForEndOfFrame(); 
    private void OnEnable()
    {
        StartCoroutine(CoLiveCount());
        //OnEnable 될 때 pool에서 가져오는데 이때 트랜스폼 값이 이전 소멸 시 위치. 한 프레임 다음 init되면 그리드에 추가
        StartCoroutine(CoAddGrid());   
    }

    private void OnDisable()
    {
        StopAllCoroutines(); // 플레이어가 습득하는 등으로 비활성화 되었을 땐 기존 돌고 있던 코루틴을 중단
        if(Managers.Game?.Grid != null)
            Managers.Game.Grid.Remove(gameObject); // 그리드에서도 제거
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
            Managers.Game.Grid.Add(gameObject); // 그리드에 추가
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}

