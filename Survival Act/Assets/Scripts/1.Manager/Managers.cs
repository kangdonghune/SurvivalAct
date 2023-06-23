using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static bool _isInit = false; // 종료 과정에서 매니저 임의 호출로 재생성 방지
    private static Managers _instance = null;
    public static Managers Instance { get { Init(); return _instance; } }

    private DataManager _data = new DataManager();
    public static DataManager Data { get { return Instance?._data; } }

    private SceneManagerEx _scene = new SceneManagerEx();
    public static SceneManagerEx SceneEx { get { return Instance?._scene; } } //? 로 Managers가 삭제되어 있을 때 접근 시 널레퍼런스 문제를 방지

    private ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instance?._resource; } }

    private AudioManager _audio = new AudioManager();
    public static AudioManager Audio { get { return Instance?._audio; } }

    private AchiveManager _achive = new AchiveManager();
    public static AchiveManager Achive { get { return Instance?._achive; } }

    private GameManager _game = new GameManager();
    public static GameManager Game { get { return Instance?._game; } }

    private EnemyManager _enemy = new EnemyManager();
    public static EnemyManager Enemy { get { return Instance?._enemy; } }

    private BaseUI _curUI = null;
    public static BaseUI UI { get { if (Instance == null) return null; else return (Instance._curUI == null) ? Instance._curUI = GameObject.FindAnyObjectByType<BaseUI>() : Instance._curUI; } } //매 씬 이동마다 새로 등록

    public static bool isOk = false;

    private void Awake()
    {
        Init();
    }

    private static void Init()
    {
        if(_isInit == false)
        {
            _isInit = true;
            GameObject managers = GameObject.Find("@Managers");
            if(managers == null)
            {
                managers = new GameObject("@Managers");
                _instance = managers.AddComponent<Managers>();
            }
            DontDestroyOnLoad(managers); //씬 이동 과정에서 삭제 등을 방지   
        }
    }

    private void Update()
    {
        Game.Update();
    }

    private void FixedUpdate()
    {
        Enemy.FixedUpdate();
    }


    //Managers가 보유한 매니저 중 씬 이동 과정에서 이벤트, 코루틴, 자료구조를 밀어줘야 하는 경우
    public static void Clear()
    {
        Managers.Instance.StopAllCoroutines(); //만약에라도 돌아가는 코루틴 있다면 전부 밀어주자
        Instance._curUI = null; //매니저의 현재 UI 초기화
        SceneEx.Clear();
    }

}
