using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorialPause : MonoBehaviour, IPausable
{
    [SerializeField] private AttackTutorialDamage damage = null;
    private Rigidbody2D rb2D = null;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.linearVelocity = new Vector2(0, 0);
    }

    public void Paused()
    {
        this.enabled = false;
        damage.PauseSwitch(true);
        rb2D.Sleep();
    }
    public void Resumed()
    {
        this.enabled = true;
        damage.PauseSwitch(false);
        rb2D.WakeUp();
    }
}
