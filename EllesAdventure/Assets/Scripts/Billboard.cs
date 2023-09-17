using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Billboards this object to look at the main camera.
/// </summary>
public class Billboard : MonoBehaviour 
{
    #region Fields
    private Camera thisCamera;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCamera = Camera.main;
    }  
    private void LateUpdate()
    {
        transform.forward = thisCamera.transform.position - transform.position;
    }
    #endregion
}