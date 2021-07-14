using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string CurrentPlayerName { get; private set; } = "";
    public HighScoreData HighScoreData { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPlayerData();
        LoadHighScoreData();
    }

    #region Player Data Setters

    public void CheckForHighScore(int points)
    {
        if (points <= HighScoreData.value) return;

        HighScoreData.value = points;
        HighScoreData.playerName = CurrentPlayerName;
    }

    public void SetPlayerName(string newName)
    {
        CurrentPlayerName = newName;
    }

    #endregion

    #region Persistence

    public void SavePlayerData()
    {
        var playerData = new PlayerData
        {
            playerName = CurrentPlayerName,
        };

        var json = JsonUtility.ToJson(playerData);
        File.WriteAllText(Application.persistentDataPath + "playerData.json", json);
    }

    public void LoadPlayerData()
    {
        var path = Application.persistentDataPath + "playerData.json";
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<PlayerData>(json);

        CurrentPlayerName = data.playerName;
    }

    public void SaveHighScoreData()
    {
        var json = JsonUtility.ToJson(HighScoreData);
        File.WriteAllText(Application.persistentDataPath + "highScoreData.json", json);
    }

    public void LoadHighScoreData()
    {
        var path = Application.persistentDataPath + "highScoreData.json";
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<HighScoreData>(json);
            HighScoreData = data;
        }
        else
        {
            HighScoreData = new HighScoreData();
        }
    }

    #endregion
}