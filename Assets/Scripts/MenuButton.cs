using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public void OnClick()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
