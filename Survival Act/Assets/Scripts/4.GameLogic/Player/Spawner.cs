using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Spawner : MonoBehaviour
{
    public Transform[] FenceSpawnPoints; // ���� ��ȯ ���� �� ��Ÿ�� �������� ���� �� �� ��ġ ������
    public Transform[] RushSpawnPoints;
    private Transform[] SpawnPoints;

    //���� �� ���� ��� �ð�
    private WaitForSeconds[] _waits;
    private WaitForSeconds _disableWait = new WaitForSeconds(10f);
    private WaitForSeconds _disableWaitBig = new WaitForSeconds(3f);
    private int totalFenceSize;


    private void Awake()
    {
        //�ڱ� �ڽŵ� �����̱⿡ 0��° �ε����� �ڽ��̴�.
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

    //��Ÿ�� ����� ���� ��ǥ�� �ʹ� ���� �����ͷ� �밡�� �ϱ⺸�� �ڵ� ������ �����. 360�� ���� �����ϰ��� �ϴ� ���� ���� ������ ���� ��ġ 
    public void CreateFencePos()
    {
        for(int count = 0; count < totalFenceSize - 1; count++)
        {
            float degree = count * (360.0f / (totalFenceSize - 1)); //count �� ������ count�� ���� �� �ش� ������ ���� ���� ���� �� Ư�� �Ÿ��� ����
            //nomalize vector
            Vector3 dir = new Vector3(Mathf.Sin(degree * Mathf.Deg2Rad), Mathf.Cos(degree * Mathf.Deg2Rad), 0);
            FenceSpawnPoints[count].position = Managers.Game.Player.transform.position + dir * 10f;
        }
    }

    //�Ŵ��� �ʿ��� ������ �� ������ ȣ��
    public void SpawnCall(int GameLevel)
    {
        //�����ص� ���� �� ���Ϻ��� ���� �Էµ� ���� ������ �� ���ٸ� �ƹ��͵� �� �ϰ� ó��
        if (GameLevel >= _waits.Length) 
            return;
        //�Ŵ��� �ν��Ͻ����� ���ִ� ��� �ڷ�ƾ�� �� �̵� �� ���� Stop ���ְ� �Ѿ��.(���� Stop ���� X)
        Managers.Instance.StartCoroutine(CoSpawnByLevel(GameLevel));
    }
    private void SpawnChaser(int type)
    {
        //type 0~2 ���� 3������ �ܼ� �߰ݿ� ����
        GameObject enemy = Managers.Game.Pool.Get("Enemy"); //����Ʈ�� �� �������� ��������
        //����Ƽ ���� �Լ��� ������ start ~ end -1 ���� float �� start~end�����̴� ����(0�� �����͸� ����� ������Ʈ)
        enemy.transform.position = SpawnPoints[Random.Range(1, SpawnPoints.Length)].position; //��ġ�� �÷��̾� ȭ�� �� �ֺ����� ����
        enemy.GetComponent<EnemyChaserCtrl>().Init(Managers.Game.SpawnDatas[type], type);//���� ������ ���� �����ͷ� �ʱ�ȭ
    }

    //��Ÿ�� 30��. �ش� �潺�� 10�� ���� �� ����(json-InGameData-SpawnTime ����)
    private void SpawnFenceEnemy()
    {

        for(int count =0; count < totalFenceSize; count++)
        {
            EnemyController enemy = Managers.Game.Pool.Get("Tree").GetComponent<EnemyController>();
            enemy.transform.position = FenceSpawnPoints[count].position;
            enemy.GetComponent<EnemyController>().Init(Managers.Game.SpawnDatas[3], 3);
            Managers.Instance.StartCoroutine(CoDead(enemy, _disableWait)); // �ش� ������ ���� �����̱⿡ ���� �ð� �� ��Ȱ��ȭ �����ش�.
        }
    }

    private void SpawnRush()
    {
        EnemyRushCtrl enemy = Managers.Game.Pool.Get("EnemyBig").GetComponent<EnemyRushCtrl>();
        int random = Random.Range(1, RushSpawnPoints.Length); //���� ���� ���� ���� �� �� ���� �������� ����
        enemy.transform.position = RushSpawnPoints[random].position;
        enemy.GetComponent<EnemyRushCtrl>().Init(Managers.Game.SpawnDatas[4], 4);
        Managers.Instance.StartCoroutine(CoDead(enemy, _disableWaitBig)); // �ش� ������ ���� �����̰� �Կ� ���� ������ ���� �Ŀ��� ��� ������ �ʿ䰡 ����.
    }

    //���� ���� �� �����Ǵ� ���� ����
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