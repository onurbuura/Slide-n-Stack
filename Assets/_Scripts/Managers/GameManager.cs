using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Play,
        Lose,
        Win
    }

    int PlayerLevel;

    [Range(1,4)]
    public int SpawnRatio = 1;

    public const int StartingTileCount = 2;
    int MaxTarget;
    int HighestRank;
    int HighestStack = 2;

    [SerializeField] GameObject ParticleObject;


    [Header("Game State")]
    public GameState State;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        EventManager.Instance.OnUIButtonClicked += HandleLevel;
        EventManager.Instance.OnStacksMerged += CheckTarget;
        EventManager.Instance.OnLevelEnded += SetGameState;

        FetchPlayerData();
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnUIButtonClicked -= HandleLevel;
        EventManager.Instance.OnStacksMerged -= CheckTarget;
        EventManager.Instance.OnLevelEnded -= SetGameState;
    }

    void FetchPlayerData()
    {

        if (!PlayerPrefs.HasKey("PlayerLevel"))
            PlayerPrefs.SetInt("PlayerLevel", 1);

        PlayerLevel = PlayerPrefs.GetInt("PlayerLevel");

        if (!PlayerPrefs.HasKey("HighestRank"))
            PlayerPrefs.SetInt("HighestRank", 4);

        HighestRank = PlayerPrefs.GetInt("HighestRank");

    }

    public Level FetchLevel()
    {
        string path = $"{Application.dataPath}/StreamingAssets/Levels/{PlayerLevel}.json";
        string lvl = File.ReadAllText(path);
        Level level = JsonUtility.FromJson<Level>(lvl);
        MaxTarget = level.Target;

        return level;

    }

    void HandleLevel(bool isNext)
    {
        if (isNext)
        {
            if (File.Exists($"{Application.dataPath}/StreamingAssets/Levels/{PlayerLevel + 1}.json"))
            {
                PlayerLevel++;
                PlayerPrefs.SetInt("PlayerLevel", PlayerLevel);
            }    
        }

        SceneManager.LoadScene(0);
    }

    void SetGameState(GameState state)
    {
        State = state;
    }

    void CheckTarget(int val, Vector3 pos)
    {
        if (HighestStack < val)
            HighestStack = val;

        if (val > HighestRank)
        {
            HighestRank = val;
            PlayerPrefs.SetInt("HighestRank", val);
            string hexCode = UIManager.Instance.GetHexCodeOfColor(val);
            UIManager.Instance.ThrowMessage($"Reached new Record: <color={hexCode}>{HighestRank}</color>");
            Instantiate(ParticleObject, pos, Quaternion.Euler(new Vector3(-90, 0, 0)));
        }


        if (val == MaxTarget)
        {
            EventManager.Instance.LevelEnded(GameState.Win);
        }
    }
}
