using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ړ����̏������B�Ǘ��҂������Rigidbody2D��ω�������B
//�܂��A�M�~�b�N�Ƃ��Ă̒�~�A�ғ����Ǘ��ҔC���B
public class MovingFloorPart : MonoBehaviour, IMovingFloor
{
    private Rigidbody2D rb2D = null;

    void Awake() { rb2D = GetComponent<Rigidbody2D>(); }
    public Vector2 MovingSpeed() { return rb2D.linearVelocity; }
}
