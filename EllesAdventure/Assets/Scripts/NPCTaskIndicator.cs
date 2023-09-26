using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: The indicator that highlights an NPC that has a task.
/// </summary>
public class NPCTaskIndicator : MonoBehaviour 
{
    #region Fields
    private Camera thisCamera;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region Unity Call Functions
    private void Awake()
    {
        thisCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        if (spriteRenderer.enabled)
        {
            transform.forward = thisCamera.transform.position - transform.position;
        }
    }

    public void ShowIcon(Color color)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.color = color;
    }

    public void HideIcon()
    {
        spriteRenderer.enabled = false;
    }
    #endregion
}