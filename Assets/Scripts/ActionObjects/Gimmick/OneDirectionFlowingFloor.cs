using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//àÍï˚å¸Ç…ó¨ÇÍÇÈè∞
public class OneDirectionFlowingFloor : MonoBehaviour, IPausable, IFlowingFloor
{
    [SerializeField] private float beltSpeed = 1.0f;
    private Collider2D c2D = null;
    private bool isPause = false;

    IContainedObject icontainedObject = null;

    void Awake()
    {
        c2D = GetComponent<Collider2D>();

        icontainedObject = GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { c2D.enabled = true; };
        icontainedObject.OnRemove += () => { c2D.enabled = false; };
    }

    public float FlowingSpeed()
    {
        return isPause ? 0 : beltSpeed;
    }

    public void Paused()
    {
        isPause = true;
    }
    public void Resumed()
    {
        isPause = false;
    }
}
