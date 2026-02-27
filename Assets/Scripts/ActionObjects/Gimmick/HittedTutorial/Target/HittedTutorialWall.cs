using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedTutorialWall : MonoBehaviour, IPausable
{
    private BoxCollider2D boxCollider2D = null;
    [SerializeField] private List<HittedTutorialTarget> targets = new List<HittedTutorialTarget>();
    private HittedTutorialWallDamage damage = null;

    void Awake()
    {
        damage = GetComponent<HittedTutorialWallDamage>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        IAreaObject iareaObject = GetComponent<IAreaObject>();
        iareaObject.OnActive += () =>
        {
            boxCollider2D.enabled = true;
            foreach (HittedTutorialTarget target in targets) { target.Init(); }
            damage.Init();
        };
        iareaObject.OnDeactive += () =>
        {
            boxCollider2D.enabled = false;
        };
    }

    public void Paused()
    {
        foreach (HittedTutorialTarget target in targets) { target.PauseSwitch(true); }
    }
    public void Resumed()
    {
        foreach (HittedTutorialTarget target in targets) { target.PauseSwitch(false); }
    }
}
