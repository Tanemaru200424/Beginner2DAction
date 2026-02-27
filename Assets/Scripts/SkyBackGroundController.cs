using UnityEngine;

//カメラに追従する必要がある空はカメラの子にしているのでこれを経由して消す必要がある。
public class SkyBackGroundController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer skyRenderer = null;

    void OnEnable()
    {
        if (skyRenderer != null) { skyRenderer.enabled = true; }
    }
    void OnDisable()
    {
        if (skyRenderer != null) { skyRenderer.enabled = false; }
    }
}
