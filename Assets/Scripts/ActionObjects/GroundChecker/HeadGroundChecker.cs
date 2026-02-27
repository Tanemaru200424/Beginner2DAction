using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadGroundChecker : MonoBehaviour
{
    private int groundTag = "Ground".GetHashCode();
    //private int contactCount = 0;
    private bool isEnter = false;
    private bool isStay = false;

    public bool IsHeadGround()
    {
        //return contactCount > 0;
        return isEnter || isStay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.GetHashCode() == groundTag)
        {
            //contactCount++;
            isEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.GetHashCode() == groundTag)
        {
            isStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.GetHashCode() == groundTag)
        {
            //contactCount = Mathf.Max(0, contactCount - 1);
            isEnter = false;
            isStay = false;
        }
    }
}
