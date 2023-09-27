using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsInitialized { get; set; }

    public int CurrentScore { get; set; }

    private const string highScoreKey = "HighScore";

    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt(highScoreKey, 0);
        }
        set
        {
            PlayerPrefs.SetInt(highScoreKey, value);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        IsInitialized = false;
        CurrentScore = 0;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToGameplay()
    {
        SceneManager.LoadScene(1);
    }
}
