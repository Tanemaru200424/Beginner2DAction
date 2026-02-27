using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseImageController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = null;
    private Image image = null;

    void Awake()
    {
        image = GetComponent<Image>();
        DisplaySwitch(false);
    }

    public void DisplaySwitch(bool isdisplay)
    {
        text.enabled = isdisplay;
        image.enabled = isdisplay;
    }
}
