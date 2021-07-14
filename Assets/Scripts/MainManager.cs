using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Brick brickPrefab;
    [SerializeField] private int lineCount = 6;
    [SerializeField] private Rigidbody ball;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private GameObject gameOverText;

    private bool _started = false;
    private bool _gameOver = false;
    private int _points = 0;

    private string _playerName;
    private HighScoreData _highScoreData = new HighScoreData();

    private void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = {1, 1, 2, 2, 5, 5};
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        if (GameManager.Instance != null)
        {
            _playerName = GameManager.Instance.CurrentPlayerName;
            _highScoreData = GameManager.Instance.HighScoreData;
        }

        InitUI();
    }

    private void InitUI()
    {
        scoreText.text = $"{_playerName} Score : {_points}";
        highScoreText.text =
            $"{_highScoreData.playerName} Score : {_highScoreData.value}";
    }

    private void Update()
    {
        if (!_started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (_gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void AddPoint(int point)
    {
        _points += point;
        scoreText.text = $"{_playerName} Score : {_points}";
    }

    public void GameOver()
    {
        _gameOver = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckForHighScore(_points);
        }

        gameOverText.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerData();
            GameManager.Instance.SaveHighScoreData();
        }
    }
}