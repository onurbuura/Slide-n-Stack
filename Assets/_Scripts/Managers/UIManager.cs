using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] GameObject Message;
    [SerializeField] TMP_Text TargetText;
    [SerializeField] Transform Canvas;
    public List<Color> Colors;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        EventManager.Instance.OnLevelEnded += OnLevelEnded;
    }
    private void OnDestroy()
    {
        EventManager.Instance.OnLevelEnded -= OnLevelEnded;
    }

    void OnLevelEnded(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Play:
                WinPanel.SetActive(false);
                LosePanel.SetActive(false);
                break;

            case GameManager.GameState.Lose:
                LosePanel.SetActive(true);
                break;

            case GameManager.GameState.Win:
                WinPanel.SetActive(true);
                break;
        }
    }

    public void PrintTarget(int val)
    {
        string hexCode = GetHexCodeOfColor(val);
        TargetText.text = $"Stack up to <color={hexCode}>{val}</color> to win level.";
    }

    public void Replay()
    {
        EventManager.Instance.UIButtonClicked(false);
    }

    public void PlayNext()
    {
        EventManager.Instance.UIButtonClicked(true);
    }

    public void ThrowMessage(string _text)
    {
        GameObject msg = Instantiate(Message, Canvas);
        msg.GetComponent<TMP_Text>().text = _text;
    }

    public string GetHexCodeOfColor(int val)
    {
        Color col = ReturnColor(val);

        return $"#{ ColorUtility.ToHtmlStringRGB(col)}";
    }

    public Color ReturnColor(int val)
    {
        int index;
        switch (val)
        {
            case 2:
                index = 0;
                break;

            case 4:
                index = 1;
                break;

            case 8:
                index = 2;
                break;

            case 16:
                index = 3;
                break;

            case 32:
                index = 4;
                break;

            case 64:
                index = 5;
                break;

            case 128:
                index = 6;
                break;

            case 256:
                index = 7;
                break;

            case 512:
                index = 8;
                break;

            case 1024:
                index = 9;
                break;

            case 2048:
            default:
                index = 10;
                break;

        }

        return Colors[index];
    }
}
