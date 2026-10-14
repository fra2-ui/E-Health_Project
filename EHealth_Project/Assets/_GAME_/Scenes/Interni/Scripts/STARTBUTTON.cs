using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public string characterSelectScene = "CharacterSelect";

    public void StartGame()
    {
        SceneManager.LoadScene(characterSelectScene);
    }
}