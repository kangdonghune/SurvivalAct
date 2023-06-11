using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using static Define;

public class GameManager
{

    public PoolManager Pool = null;
    public GridController Grid = null;

    public float GameTime = 0;
    public float MaxGameTime = 180f; //�� �� �÷��� Ÿ��
    public bool IsLive = false; //���� �� �÷��� ����
    public int PlayerId = -1; // id�� ���� �÷��̾� �ִϸ��̼� ����, �ʱ� ���� ���� ��
    public float Hp;
    public float MaxHp = 100;
    //����.ų.����ġ �� �� ���� �������� �����ϴ� �����ʹ� �� ���Ӹ��� �ʱ�ȭ
    public int Level = 0;
    public int GameLevel = 0;
    public int Kill = 0;
    public float exp = 0;
  
    //InGame Level Design Data
    public SpawnData[] SpawnDatas;
    public float[] nextExp;
    public float[] nextGameTime;
    public bool isGameLevelMax = false;

    public PlayerController Player;
    public GameObject EnemyCleaner;

    //�÷��̾�, ���� �� �ִϸ��̼� ������
    public RuntimeAnimatorController[] PlayerAniCtrl;
    public RuntimeAnimatorController[] EnemyAniCtrl;


    public void Init()
    {
        nextExp = Managers.Data.InGameDic["Exp"].value; //json ���� �����ͷ� ���� �� �ʿ� ����ġ ���� index[0] = lv.1 ��µ� �ʿ� ����ġ
        nextGameTime = Managers.Data.InGameDic["LevelTime"].value;//Game level�� �Ѿ�� Ÿ�� �迭
        SpawnDatas = new SpawnData[Managers.Data.SpawnDataDic.Count];
        for(int idx = 0; idx < SpawnDatas.Length; idx++)
        {
            SpawnDatas[idx] = Managers.Data.SpawnDataDic[(EnemyType)idx];
        }
        PlayerAniCtrl = new RuntimeAnimatorController[Enum.GetValues(typeof(PlayerName)).Length];
        for(int idx = 0; idx < PlayerAniCtrl.Length; idx++)
        {
            PlayerAniCtrl[idx] = Managers.Resource.Load<RuntimeAnimatorController>(((PlayerName)idx).ToString());
        }
        EnemyAniCtrl = new RuntimeAnimatorController[Enum.GetValues(typeof(EnemyName)).Length];
        for (int idx = 0; idx < EnemyAniCtrl.Length; idx++)
        {
            EnemyAniCtrl[idx] = Managers.Resource.Load<RuntimeAnimatorController>(((EnemyName)idx).ToString());
        }
    }

    public void GameStart(int id)
    {
        Level = 0;
        GameLevel = 0;
        Kill = 0;
        exp = 0;
        GameTime = 0; //�� �κ� �������� ������ �ٽ� ���� ������ �� �� ���� ������ ������ ������ �ȴ�.
        Pool = GameObject.FindObjectOfType<PoolManager>();
        Grid = GameObject.FindObjectOfType<GridController>();
        Player = Managers.Resource.Instantiate("Player").GetComponent<PlayerController>();
        GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = Player.transform; // �ó׸�ƽ ī�޶� ����
        EnemyCleaner = GameObject.Find("EnemyCleaner");
        EnemyCleaner.SetActive(false);
        PlayerId = id;
        Hp = MaxHp * Charicter.HP;
        isGameLevelMax = false;
        Managers.UI.Get<LevelUp>().Select(id % 4); //4 �� ���� ����
        //���̽�ƽ ui �� ����Ƽ �ð� ���� �ʱ�ȭ
        Resume();
        //���� ���� �� �⺻ ���� ����
        Managers.Game.Player.GetComponentInChildren<Spawner>().SpawnCall(0); //0���� �ܰ� ���� ��ȯ ����
        Managers.Game.Player.GetComponentInChildren<Spawner>().CreateFencePos();//�Ŵ����� ���� �÷��̾� ��� �� ���� ��� �ʱ�ȭ
        Managers.Audio.ChangeBGM(1);
        Managers.Audio.PlayBGM(true);
    }

    public void GameRetry()
    {
        Managers.SceneEx.LoadScene(SceneType.Lobby); //�κ�� ���ư���.
        Managers.Audio.ChangeBGM(0);
        Managers.Audio.PlayBGM(true);
    }

    public void GameQuit()
    {
        Application.Quit(); //���� ����
    }

    public void GameOver()
    {
        Managers.Achive.UpdateAchive();
        Managers.Instance.StartCoroutine(CoGameOver());
        Managers.Audio.PlayBGM(false);
        Managers.Audio.PlaySFX(AudioManager.SFX.Lose);
    }

    public void GameVictory()
    {
        Managers.Achive.UpdateAchive();
        Managers.Instance.StartCoroutine(CoGameVictory());
        Managers.Audio.PlayBGM(false);
        Managers.Audio.PlaySFX(AudioManager.SFX.Win);
    }

    public void Update()
    {
        if (IsLive == false)
            return;
        GameTime += Time.deltaTime;
        if(isGameLevelMax == false && GameTime >= nextGameTime[GameLevel]) // ���� �ð��� ���� �������� ���� ������ �Ѿ�� �ð����� ũ�ٸ�
        {
            //���� ���� ����
            if (++GameLevel == nextGameTime.Length)
                isGameLevelMax = true;
            Managers.Game.Player.GetComponentInChildren<Spawner>().SpawnCall(GameLevel);//���� ������ �����ϸ� �����ϴ� �ʿ� �ڷ�ƾ ȣ��
        }
        if(GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
            GameVictory();
        }

    }

    public void GetExp()
    {
        if (IsLive == false)
            return;
        exp++;
        if(exp >= nextExp[Mathf.Min(Level, nextExp.Length -1)])
        {
            exp = 0;
            Level++;
            Managers.UI.Get<LevelUp>().Show();
        }    
    }

    public void Stop()
    {
        IsLive = false;
        Time.timeScale = 0;
        Managers.UI.Get<Transform>("Joy").localScale = Vector3.zero;
    }

    public void Resume()
    {
        IsLive = true;
        Time.timeScale = 1;
        Managers.UI.Get<Transform>("Joy").localScale = Vector3.one;
    }

    IEnumerator CoGameOver()
    {
        Managers.UI.Get<Result>("GameResult").Lose();
        yield return new WaitForSeconds(1);
        Stop();
    }

    IEnumerator CoGameVictory()
    {
        IsLive = false;
        EnemyCleaner.SetActive(true);
        yield return new WaitForSeconds(1); //�����Ͱ� �״� ����� ������ �� ��� ���
        EnemyCleaner.SetActive(false);
        Managers.UI.Get<Result>("GameResult").Win();
        Stop();

    }
}
