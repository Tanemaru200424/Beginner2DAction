using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//地形検知。
public class GroundChecker : MonoBehaviour
{
    private int groundTag = "Ground".GetHashCode();
    private int headGroundTag = "HeadThroughGround".GetHashCode();
    //private int contactCount = 0;
    private bool isEnter = false;
    private bool isStay = false;

    public bool IsGround()
    {
        //return contactCount > 0;
        return isEnter || isStay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.GetHashCode() == groundTag ||
            collision.gameObject.tag.GetHashCode() == headGroundTag)
        {
            //contactCount++;
            isEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.GetHashCode() == groundTag ||
            collision.gameObject.tag.GetHashCode() == headGroundTag)
        {
            isStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.GetHashCode() == groundTag ||
            collision.gameObject.tag.GetHashCode() == headGroundTag)
        {
            //contactCount = Mathf.Max(0, contactCount - 1);
            isEnter = false;
            isStay = false;
        }
    }
}
