using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeImageController : MonoBehaviour
{
    private Image fade = null;

    void Awake()
    {
        fade = GetComponent<Image>();
        fade.enabled = false;
    }

    public IEnumerator FadeIn()
    {
        if (!fade.enabled) { fade.enabled = true; }
        float t = 0;
        fade.color = new Color(0, 0, 0, 0);
        while (t < 1)
        {
            t += Time.deltaTime;
            fade.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        fade.color = new Color(0, 0, 0, 1);
    }

    public IEnumerator FadeOut()
    {
        if (!fade.enabled) { fade.enabled = true; }
        float t = 1;
        fade.color = new Color(0, 0, 0, 1);
        while (t > 0)
        {
            t -= Time.deltaTime;
            fade.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        fade.color = new Color(0, 0, 0, 0);
        fade.enabled = false;
    }
}
