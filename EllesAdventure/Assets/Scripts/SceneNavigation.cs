using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Does scene navigation.
/// </summary>
public class SceneNavigation : MonoBehaviour 
{
    #region Fields
    public static SceneNavigation Instance;
    #endregion

    #region Unity Call Functions
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    #region Public Methods
    public void SwitchScene(string newScene)
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
    }

    public void ReloadScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}