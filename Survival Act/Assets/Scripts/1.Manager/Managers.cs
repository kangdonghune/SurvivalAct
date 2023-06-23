using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static bool _isInit = false; // ���� �������� �Ŵ��� ���� ȣ��� ����� ����
    private static Managers _instance = null;
    public static Managers Instance { get { Init(); return _instance; } }

    private DataManager _data = new DataManager();
    public static DataManager Data { get { return Instance?._data; } }

    private SceneManagerEx _scene = new SceneManagerEx();
    public static SceneManagerEx SceneEx { get { return Instance?._scene; } } //? �� Managers�� �����Ǿ� ���� �� ���� �� �η��۷��� ������ ����

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
    public static BaseUI UI { get { if (Instance == null) return null; else return (Instance._curUI == null) ? Instance._curUI = GameObject.FindAnyObjectByType<BaseUI>() : Instance._curUI; } } //�� �� �̵����� ���� ���

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
            DontDestroyOnLoad(managers); //�� �̵� �������� ���� ���� ����   
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


    //Managers�� ������ �Ŵ��� �� �� �̵� �������� �̺�Ʈ, �ڷ�ƾ, �ڷᱸ���� �о���� �ϴ� ���
    public static void Clear()
    {
        Managers.Instance.StopAllCoroutines(); //���࿡�� ���ư��� �ڷ�ƾ �ִٸ� ���� �о�����
        Instance._curUI = null; //�Ŵ����� ���� UI �ʱ�ȭ
        SceneEx.Clear();
    }

}
