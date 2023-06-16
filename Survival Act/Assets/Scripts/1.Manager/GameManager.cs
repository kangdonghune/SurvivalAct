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
    public float MaxGameTime = 180f; //판 당 플레이 타임
    public bool IsLive = false; //게임 씬 플레이 여부
    public int PlayerId = -1; // id에 따라 플레이어 애니메이션 선택, 초기 무기 지급 등
    public float Hp;
    public float MaxHp = 100;
    //레벨.킬.경험치 등 인 게임 내에서만 존재하는 데이터는 매 게임마다 초기화
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

    //플레이어, 몬스터 별 애니메이션 데이터
    public RuntimeAnimatorController[] PlayerAniCtrl;
    public RuntimeAnimatorController[] EnemyAniCtrl;


    public void Init()
    {
        nextExp = Managers.Data.InGameDic["Exp"].value; //json 기준 데이터로 레벨 당 필요 경험치 고정 index[0] = lv.1 찍는데 필요 경험치
        nextGameTime = Managers.Data.InGameDic["LevelTime"].value;//Game level이 넘어가는 타임 배열
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
        GameTime = 0; //이 부분 빼먹으면 다음에 다시 게임 씬으로 올 때 이전 게임의 레벨이 유지가 된다.
        Pool = GameObject.FindObjectOfType<PoolManager>();
        Grid = GameObject.FindObjectOfType<GridController>();
        Player = Managers.Resource.Instantiate("Player").GetComponent<PlayerController>();
        GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = Player.transform; // 시네마틱 카메라 연결
        EnemyCleaner = GameObject.Find("EnemyCleaner");
        EnemyCleaner.SetActive(false);
        PlayerId = id;
        Hp = MaxHp * Charicter.HP;
        isGameLevelMax = false;
        Managers.UI.Get<LevelUp>().Select(id % 4); //4 는 무기 개수
        //조이스틱 ui 및 유니티 시간 설정 초기화
        Resume();
        //몬스터 스폰 및 기본 음향 설정
        Managers.Game.Player.GetComponentInChildren<Spawner>().SpawnCall(0); //0레벨 단계 좀비 소환 시작
        Managers.Game.Player.GetComponentInChildren<Spawner>().CreateFencePos();//매니저에 게임 플레이어 등록 후 스폰 장소 초기화
        Managers.Audio.ChangeBGM(1);
        Managers.Audio.PlayBGM(true);
    }

    public void GameRetry()
    {
        Managers.SceneEx.LoadScene(SceneType.Lobby); //로비로 돌아간다.
        Managers.Audio.ChangeBGM(0);
        Managers.Audio.PlayBGM(true);
    }

    public void GameQuit()
    {
        Application.Quit(); //게임 종료
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
        if(isGameLevelMax == false && GameTime >= nextGameTime[GameLevel]) // 현재 시간이 현재 레벨에서 다음 레벨로 넘어가는 시간보다 크다면
        {
            //게임 레벨 증가
            if (++GameLevel == nextGameTime.Length)
                isGameLevelMax = true;
            Managers.Game.Player.GetComponentInChildren<Spawner>().SpawnCall(GameLevel);//게임 레벨이 증가하면 스폰하는 쪽에 코루틴 호출
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
        yield return new WaitForSeconds(1); //모든몬스터가 죽는 모션이 끝나는 걸 잠시 대기
        EnemyCleaner.SetActive(false);
        Managers.UI.Get<Result>("GameResult").Win();
        Stop();

    }
}
