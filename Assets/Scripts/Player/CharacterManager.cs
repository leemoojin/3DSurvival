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

    // �����ֱ� �Լ�(Awake)�� ����Ǿ��ٴ� ����
    // ���ӿ�����Ʈ�� ��ũ��Ʈ�� �پ ����Ǿ��ٴ� ��
    // ���� ������Ʈ�� ����� ������ ���� �ʿ� ����
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
