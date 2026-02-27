using UnityEngine;
using TMPro;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI explanatoryText = null;
    private SpriteRenderer spriteRenderer = null;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        IAreaObject iareaObject = GetComponent<IAreaObject>();
        iareaObject.OnActive += () =>
        {
            explanatoryText.enabled = true;
            spriteRenderer.enabled = true;
        };
        iareaObject.OnDeactive += () =>
        {
            explanatoryText.enabled = false;
            spriteRenderer.enabled = false;
        };
    }
}
