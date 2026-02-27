using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移動床、流れる床の影響を受けるオブジェクトの足元に付ける。
public class AffectedByFloor : MonoBehaviour
{
    private int groundTag = "Ground".GetHashCode();
    private int headGroundTag = "HeadThroughGround".GetHashCode();
    private List<IFlowingFloor> contactFlowingFloors = new List<IFlowingFloor>();
    private List<IMovingFloor> contactMovingFloors = new List<IMovingFloor>();

    //流れる床による影響
    public float AffectedFlowingFloor()
    {
        float resultXSpeed = 0;

        float plusMax = 0;
        float minusMax = 0;
        foreach (IFlowingFloor iflowingFloor in contactFlowingFloors)
        {
            float refFlowingSpeed = iflowingFloor.FlowingSpeed();
            if (refFlowingSpeed == 0) { continue; }
            else if(refFlowingSpeed > 0 && refFlowingSpeed > plusMax) { plusMax = refFlowingSpeed; }
            else if(refFlowingSpeed < 0 && refFlowingSpeed < minusMax) {  minusMax = refFlowingSpeed; }
        }
        resultXSpeed = plusMax + minusMax;

        return resultXSpeed;
    }

    //動く床による影響
    public Vector2 AffectedMovingFloor()
    {
        float resultXSpeed = 0;
        float resultYSpeed = 0;

        foreach (IMovingFloor imovingFloor in contactMovingFloors)
        {
            Vector2 refMovingSpeed = imovingFloor.MovingSpeed();
            resultXSpeed += refMovingSpeed.x;
            resultYSpeed += refMovingSpeed.y;
        }

        return new Vector2(resultXSpeed, resultYSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.GetHashCode() == groundTag ||
            collision.gameObject.tag.GetHashCode() == headGroundTag)
        {
            IFlowingFloor iflowingFloor = collision.gameObject.GetComponent<IFlowingFloor>();
            if (iflowingFloor != null && !contactFlowingFloors.Contains(iflowingFloor)) { contactFlowingFloors.Add(iflowingFloor); }

            IMovingFloor imovingFloor = collision.gameObject.GetComponent<IMovingFloor>();
            if (imovingFloor != null && !contactMovingFloors.Contains(imovingFloor)) { contactMovingFloors.Add(imovingFloor); }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.GetHashCode() == groundTag ||
            collision.gameObject.tag.GetHashCode() == headGroundTag)
        {
            IFlowingFloor iflowingFloor = collision.gameObject.GetComponent<IFlowingFloor>();
            if (iflowingFloor != null && contactFlowingFloors.Contains(iflowingFloor)) { contactFlowingFloors.Remove(iflowingFloor); }

            IMovingFloor imovingFloor = collision.gameObject.GetComponent<IMovingFloor>();
            if (imovingFloor != null && contactMovingFloors.Contains(imovingFloor)) { contactMovingFloors.Remove(imovingFloor); }
        }
    }
}
