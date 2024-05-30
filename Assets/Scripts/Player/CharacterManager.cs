using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacerManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    // 생명주기 함수(Awake)가 실행되었다는 것은
    // 게임오브젝트로 스크립트가 붙어서 실행되었다는 것
    // 게임 오브젝트를 만드는 로직을 넣을 필요 없다
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {   
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
