using UnityEngine;
using TMPro;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Displays a UI text prompt.
/// </summary>
public class InteractableTextPrompt : MonoBehaviour 
{
    private TextMeshProUGUI thisTextField;

    private void Start()
    {
        thisTextField = GetComponent<TextMeshProUGUI>();
    }

    public void HidePrompt()
    {
        StopAllCoroutines();
        thisTextField.enabled = false;
    }

    public void DisplayPrompt(Vector3 promptPosition, string displayText)
    {
        StopAllCoroutines();
        ShowPrompt(promptPosition, displayText);
    }

    public void DisplayPrompt(string displayText)
    {
        StopAllCoroutines();
        ShowPrompt(displayText);
    }

    private void ShowPrompt(Vector3 promptPosition, string displayText)
    {
        thisTextField.enabled = true;
        thisTextField.text = displayText;

        Vector2 pos = Camera.main.WorldToScreenPoint(promptPosition);
        transform.position = pos;
    }
    private void ShowPrompt(string displayText)
    {
        thisTextField.enabled = true;
        thisTextField.text = displayText;

        Vector2 pos = new Vector2(Screen.width/2, Screen.height/2);
        transform.position = pos;
    }
}