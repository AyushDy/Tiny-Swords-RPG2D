using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Forest01");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
