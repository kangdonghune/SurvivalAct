using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Spawner : MonoBehaviour
{
    public Transform[] FenceSpawnPoints; // 몬스터 소환 패턴 중 울타리 형식으로 가둘 때 쓸 위치 데이터
    public Transform[] RushSpawnPoints;
    private Transform[] SpawnPoints;

    //몬스터 별 등장 대기 시간
    private WaitForSeconds[] _waits;
    private WaitForSeconds _disableWait = new WaitForSeconds(10f);
    private WaitForSeconds _disableWaitBig = new WaitForSeconds(3f);
    private int totalFenceSize;


    private void Awake()
    {
        //자기 자신도 포함이기에 0번째 인덱스는 자신이다.
        SpawnPoints = gameObject.transform.GetChild(0).GetComponentsInChildren<Transform>();
        FenceSpawnPoints = gameObject.transform.GetChild(1).GetComponentsInChildren<Transform>();
        RushSpawnPoints = gameObject.transform.GetChild(2).GetComponentsInChildren<Transform>();
        totalFenceSize = FenceSpawnPoints.Length;
        _waits = new WaitForSeconds[Managers.Data.InGameDic["SpawnTime"].value.Length];
        for(int idx = 0; idx < _waits.Length; idx++)
        {
            _waits[idx] = new WaitForSeconds(Managers.Data.InGameDic["SpawnTime"].value[idx]);
        }

    }

    //울타리 방식은 찍을 좌표가 너무 많아 에디터로 노가다 하기보단 코드 상으로 만든다. 360도 기준 스폰하고자 하는 몬스터 수로 각도를 구해 위치 
    public void CreateFencePos()
    {
        for(int count = 0; count < totalFenceSize - 1; count++)
        {
            float degree = count * (360.0f / (totalFenceSize - 1)); //count 당 각도에 count를 곱한 후 해당 각도로 방향 벡터 생성 후 특정 거리로 스폰
            //nomalize vector
            Vector3 dir = new Vector3(Mathf.Sin(degree * Mathf.Deg2Rad), Mathf.Cos(degree * Mathf.Deg2Rad), 0);
            FenceSpawnPoints[count].position = Managers.Game.Player.transform.position + dir * 10f;
        }
    }

    //매니저 쪽에서 레벨업 할 때마다 호출
    public void SpawnCall(int GameLevel)
    {
        //제작해둔 레벨 별 패턴보다 현재 입력된 게임 레벨이 더 높다면 아무것도 안 하게 처리
        if (GameLevel >= _waits.Length) 
            return;
        //매니저 인스턴스에서 해주는 모든 코루틴은 씬 이동 시 전부 Stop 해주고 넘어간다.(별도 Stop 관리 X)
        Managers.Instance.StartCoroutine(CoSpawnByLevel(GameLevel));
    }
    private void SpawnChaser(int type)
    {
        //type 0~2 까지 3종류는 단순 추격용 몬스터
        GameObject enemy = Managers.Game.Pool.Get("Enemy"); //디펄트한 적 프리팹을 가져오고
        //유니티 랜덤 함수는 정수는 start ~ end -1 까지 float 은 start~end까지이니 주의(0은 포인터를 묶어둔 오브젝트)
        enemy.transform.position = SpawnPoints[Random.Range(1, SpawnPoints.Length)].position; //위치는 플레이어 화면 밖 주변에서 랜덤
        enemy.GetComponent<EnemyChaserCtrl>().Init(Managers.Game.SpawnDatas[type], type);//스폰 레벨에 따른 데이터로 초기화
    }

    //쿨타임 30초. 해당 펜스는 10초 유지 후 해제(json-InGameData-SpawnTime 참고)
    private void SpawnFenceEnemy()
    {

        for(int count =0; count < totalFenceSize; count++)
        {
            EnemyController enemy = Managers.Game.Pool.Get("Tree").GetComponent<EnemyController>();
            enemy.transform.position = FenceSpawnPoints[count].position;
            enemy.GetComponent<EnemyController>().Init(Managers.Game.SpawnDatas[3], 3);
            Managers.Instance.StartCoroutine(CoDead(enemy, _disableWait)); // 해당 패턴의 몹은 무적이기에 일정 시간 후 비활성화 시켜준다.
        }
    }

    private void SpawnRush()
    {
        EnemyRushCtrl enemy = Managers.Game.Pool.Get("EnemyBig").GetComponent<EnemyRushCtrl>();
        int random = Random.Range(1, RushSpawnPoints.Length); //러쉬 몬스터 스폰 지정 중 한 곳을 랜덤으로 선택
        enemy.transform.position = RushSpawnPoints[random].position;
        enemy.GetComponent<EnemyRushCtrl>().Init(Managers.Game.SpawnDatas[4], 4);
        Managers.Instance.StartCoroutine(CoDead(enemy, _disableWaitBig)); // 해당 패턴의 몹은 무적이고 촬영 범위 밖으로 나간 후에는 계속 생존할 필요가 없다.
    }

    //게임 레벨 별 스폰되는 몬스터 종류
    IEnumerator CoSpawnByLevel(int GameLevel)
    {
        while(true)
        {
            switch (GameLevel)
            {
                case 0:
                case 1:
                case 2:
                    SpawnChaser(GameLevel);
                    break;
                case 3:
                    SpawnFenceEnemy();
                    break;
                case 4:
                    SpawnRush();
                    break;
                default:
                    break;
            }
            yield return _waits[GameLevel];
        }

    }

    IEnumerator CoDead(EnemyController enemy, WaitForSeconds wait)
    {
        yield return wait;
        enemy.ReadyDead();
    }
}