using UnityEngine;

public enum GameState
{
    Play,
    Pause,
    Stop
}

public enum Map
{
    Field,
    Dungeon_1,
    Dungeon_2,
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Instantiate(Resources.Load("GameManager"));
                instance = FindObjectOfType<GameManager>();
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }
    public GameState GameState { get; set; }
    public Map CurrentMap { get; set; }


    public int Coin { get; set; }

    void Awake()
    {
        CurrentMap = Map.Field;
        Coin = 10000;
    }

    private Player player;
    public Player Player
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>().GetComponent<Player>();
            }
            return player;
        }
    }
    
    public StatusManager statusMgr;
    public DataManager dataMgr;
    public SceneController sceneController;

    public GameObject FindGameObject(string path)
    {
        return GameObject.Find(path);
    }
}
