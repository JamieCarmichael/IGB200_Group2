using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Displays a UI promps.
/// </summary>
public class UIPrompt : MonoBehaviour 
{
    private Image thisImage;

    private void Start()
    {
        thisImage = GetComponent<Image>();
    }

    /// <summary>
    /// Show a prompt for a period of time.
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="timeAtFull"></param>
    /// <param name="timeToFade"></param>
    public void StartPrompt(Sprite sprite, float timeAtFull, float timeToFade)
    {
        StopAllCoroutines();
        StartCoroutine(ShowPrompt(sprite, timeAtFull, timeToFade));
    }

    /// <summary>
    /// Show the current prompt for 1 second.
    /// </summary>
    public void StartPrompt()
    {
        StopAllCoroutines();
        StartCoroutine(ShowPrompt(thisImage.sprite, 1.0f, 0.0f));
    }

    private IEnumerator ShowPrompt(Sprite sprite, float timeAtFull, float timeToFade)
    {
        thisImage.enabled = true;
        thisImage.sprite = sprite;

        // Initailize sprite opactive colour
        Color usedColour = thisImage.color;
        usedColour.a = 1.0f;
        thisImage.color = usedColour;
       
        float timer = 0.0f;
        // Time at full visability
        while (timer < timeAtFull)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0.0f;
        // Change in opacity as fades.
        while (timer < timeToFade)
        {
            timer += Time.deltaTime;

            usedColour.a = 1 - (timer / timeToFade);

            thisImage.color = usedColour;

            yield return null;
        }
        thisImage.enabled = false;
    }
}