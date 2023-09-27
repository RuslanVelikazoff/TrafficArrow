using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    #region START

    [HideInInspector]
    public bool hasGameFinished;

    public static GameplayManager Instance;

    private void Awake()
    {
        Instance = this;

        hasGameFinished = false;
        GameManager.Instance.IsInitialized = true;

        score = 0;
        currentLevel = 0;
        scoreSpeed = _levelSpeed[currentLevel];
        _scoreText.text = ((int)score).ToString();

        for (int i = 0; i < 8; i++)
        {
            SpawnObstacle();
        }
    }

    #endregion

    #region SCORE

    private float score;
    private int currentLevel;
    private float scoreSpeed;

    [SerializeField] private List<int> _levelSpeed, _levelMax;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private float _spawnX, _spawnY;

    private void Update()
    {
        if (hasGameFinished)
        {
            return;
        }

        score += scoreSpeed * Time.deltaTime;
        _scoreText.text = ((int)score).ToString();

        if (score > _levelMax[Mathf.Clamp(currentLevel, 0, _levelMax.Count)])
        {
            currentLevel = Mathf.Clamp(currentLevel + 1, 0, _levelMax.Count);
            SpawnObstacle();
            scoreSpeed = _levelSpeed[currentLevel];
        }
    }

    private void SpawnObstacle()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-_spawnX, _spawnX), Random.Range(-_spawnY, _spawnY), 0f);
        RaycastHit2D hit = Physics2D.CircleCast(spawnPos, 1f, Vector2.zero);
        bool canSpawn = hit;

        while (canSpawn)
        {
            spawnPos = new Vector3(Random.Range(-_spawnX, _spawnX), Random.Range(-_spawnY, _spawnY), 0f);
            hit = Physics2D.CircleCast(spawnPos, 1f, Vector2.zero);
            canSpawn = hit;
        }

        Instantiate(_obstaclePrefab, spawnPos, Quaternion.Euler(0, 0, Random.Range(0, 37) * 10f));
    }

    #endregion

    #region GAME_OVER

    public UnityAction GameEnd;

    public void GamEnded()
    {
        //TODO: lose music

        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", (int)score);
        }

        GameEnd?.Invoke();
        hasGameFinished = true;
        GameManager.Instance.CurrentScore = (int)score;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        FindObjectOfType<AudioManager>().Play("Lose");
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GoToMainMenu();
    }

    #endregion
}
