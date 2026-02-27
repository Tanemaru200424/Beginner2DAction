using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ړ����鏰�̊Ǘ�
public class LoopMovingFloorManager : MonoBehaviour, IPausable, IInitGimmick
{
    [SerializeField] private GameObject floorObject = null; //���{�̂̃I�u�W�F�N�g�B
    private Rigidbody2D floorRb2D = null; //���{�̂̕�������B����̑��x��ω�������B
    [SerializeField] private float speed = 1; //���{�̂𓮂����X�s�[�h
    [SerializeField] private List<Transform> movePoints = new List<Transform>(); //���{�̂𓮂������߂̍��W�B

    private Vector2 moveVector = new Vector2(1, 0); //�ړ�����
    private int basepointnum = 0; //���݋N�_�ƂȂ郌�[���̔ԍ��B
    private int nextpointnum = 0; //�ڎw�����[���̔ԍ�

    void Awake()
    {
        floorRb2D = floorObject.GetComponent<Rigidbody2D>();

        IContainedObject icontainedObject = GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => FloorSwitch(true);
        icontainedObject.OnRemove += () => FloorSwitch(false);
    }

    public void Initialize()
    {
        basepointnum = 0;
        nextpointnum = 1;
        floorObject.transform.position = movePoints[basepointnum].position;
        moveVector = DefMoveVector();
    }

    private void FloorSwitch(bool isactive)
    {
        if (isactive)
        {
            this.enabled = true;
        }
        else
        {
            this.enabled = false;
            floorRb2D.linearVelocity = new Vector2(0, 0);
        }
    }

    void Update()
    {
        if(Vector3.Distance(floorObject.transform.position, movePoints[nextpointnum].position) < 0.05f)
        {
            basepointnum++;
            if(basepointnum > movePoints.Count-1)
            {
                basepointnum = 0;
            }
            moveVector = DefMoveVector();
        }
    }

    void FixedUpdate()
    {
        floorRb2D.linearVelocity = moveVector * speed;
    }

    //���݂̋N�_�̔ԍ�����ړ�������ݒ�B
    private Vector2 DefMoveVector()
    {
        nextpointnum = basepointnum + 1;
        if(nextpointnum > movePoints.Count-1) { nextpointnum = 0; }
        Vector2 vector = movePoints[nextpointnum].position - movePoints[basepointnum].position;
        return vector;
    }

    public void Paused() 
    {
        this.enabled = false;
        floorRb2D.linearVelocity = new Vector2(0, 0);
    }
    public void Resumed()
    {
        this.enabled = true;
    }
}
