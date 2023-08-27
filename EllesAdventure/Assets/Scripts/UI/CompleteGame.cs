using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Finishes the game.
/// </summary>
public class CompleteGame : MonoBehaviour 
{
    #region Fields
    [SerializeField] private GameObject gameCompleteObject;
    #endregion

    #region Public Methods
    public void FinishGame()
    {
        FindObjectOfType<Pause>().StopGame();
        gameCompleteObject.SetActive(true);
    }
    #endregion
}