using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static Define;

public class ReadyScene : BaseScene
{
    private bool[] _LoadEnds =  new bool[Enum.GetValues(typeof(LoadLabel)).Length]; //각 라벨 별 로딩 완료 여부 보관
    private bool _load = true;
    protected override void Init()
    {
        Application.targetFrameRate = 60; //이거 지장 안해주면 30프레임까지 떨어진다.
        Type = SceneType.Ready; //내 씬 정의
        //데이터 로딩 시작
        //비동기 함수라 int ++ 이 문제가 생기지 않을까 생각 중. 혹시라도 로드 관련 불규칙한 오류가 생기면 확인할 것.
        Managers.Resource.LoadAllAsync<GameObject>(LoadLabel.Prefab.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Prefab] = key; });
        Managers.Resource.LoadAllAsync<RuntimeAnimatorController>(LoadLabel.Animation.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Animation] = key; });
        Managers.Resource.LoadAllAsync<TextAsset>(LoadLabel.Data.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Data] = key; });
        Managers.Resource.LoadAllAsync<AudioClip>(LoadLabel.Audio.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Audio] = key; });
        Managers.Resource.LoadAllAsync<GameObject>(LoadLabel.UI.ToString(), (key) => { _LoadEnds[(int)LoadLabel.UI] = key; });
        Managers.Resource.LoadAllAsync<Sprite>(LoadLabel.Sprite.ToString(), (key) => { _LoadEnds[(int)LoadLabel.Sprite] = key; });
        StartCoroutine(CoCheckLoading());
    }
    public override void Clear() // 레디 씬을 벗어날 때 총괄매니저에서 레디매니저 제거
    {
        
    }

    IEnumerator CoCheckLoading()
    {
        while(true)
        {
            foreach (bool isload in _LoadEnds) // 모든 로딩 여부 순회하며 하나라도 로딩 안되어 있다면 _load = false;
                _load = isload;
            if(_load == true)
            {
                Managers.Data.Init(); // json을 addressable로 긁어 온 다음에야 json을 읽어야 한다.
                Managers.UI.Get<Button>("StartButton").enabled = true; //데이터 전부 로딩되면 버튼 활성화;
                Managers.Audio.Init();
                Managers.Game.Init();
                Managers.Achive.Init();
                Managers.Audio.PlayBGM(true);
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.2f); //게임이 멈춰있어도 로딩이 진행되니 리얼 타임으로 
        }

    }
}
