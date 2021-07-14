using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private InputField inputField;

    private void Start()
    {
        inputField.text = GameManager.Instance.CurrentPlayerName;
        inputField.onEndEdit.AddListener(HandlePlayerNameChanged);
    }

    private void HandlePlayerNameChanged(string newName)
    {
        GameManager.Instance.SetPlayerName(newName);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/main");
    }

    public void Exit()
    {
        GameManager.Instance.SavePlayerData();
        GameManager.Instance.SaveHighScoreData();
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}
