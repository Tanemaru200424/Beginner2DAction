using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//必要はほぼないけどプレイヤーの追従機能がうまくいってるか試したいのでIAimPlayerが付いている。
//敵の生成オブジェクトもこれをもとに生成位置との距離に応じて生成破壊するようにする。
public class AttackTutorialGenerator : MonoBehaviour, IGenerator, IObjectContainer, IPausable
{
    private IAimPlayer iaimPlayer = null;
    private bool CanGenerate = true;
    [SerializeField] private GameObject originObject = null;
    private GameObject generateObject = null;
    [SerializeField] private float generateDistance = 20;
    [SerializeField] private float destroyDistance = 30;

    private IObjectContainer iobjectContainer = null;

    void Awake()
    {
        iaimPlayer = GetComponent<IAimPlayer>();
        IAreaObject iareaObject = GetComponent<IAreaObject>();
        iareaObject.OnActive += () =>
        {
            if(generateObject == null) { CanGenerate = true; }
            this.enabled = true;
        };
        iareaObject.OnDeactive += () =>
        {
            iaimPlayer.CancelPlayerTrans();
            this.enabled = false;
        };
    }

    void Update()
    {
        if (iaimPlayer.IsExistPlayer())
        {
            if(Vector2.Distance(iaimPlayer.GetPlayerPos(), this.transform.position) <= generateDistance) 
            {
                if (generateObject == null && CanGenerate) { GenerateTarget(); } 
            }
            else if(Vector2.Distance(iaimPlayer.GetPlayerPos(), this.transform.position) >= destroyDistance)
            {
                if (generateObject != null && !CanGenerate) 
                {
                    Destroy(generateObject);
                    CanGenerate = true;
                }
                if (generateObject == null && !CanGenerate) { CanGenerate = true; }
            }
        }
    }

    private void GenerateTarget()
    {
        GameObject generatedObject = Generate(originObject, this.transform.position, new Vector3(8, 8, 1), 0);
        IContainedObject icontainedObject = generatedObject.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedObject); RegistObject(generatedObject); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedObject); RemoveObject(generatedObject); };
        InitRegist(iobjectContainer, generatedObject);

        //エフェクト生成にコンテナ登録。
        IGenerator igenerator = generatedObject.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(iobjectContainer);

        CanGenerate = false;
    }

    public void SetObjectContainer(IObjectContainer iobjectContainer)
    {
        this.iobjectContainer = iobjectContainer;
    }
    public GameObject Generate(GameObject generateObject, Vector3 generatePos, Vector3 generateScale, float zAngle)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, zAngle);
        GameObject generated = Instantiate(generateObject, generatePos, newRotation);
        generated.transform.localScale = generateScale;
        return generated;
    }
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject)
    {
        if (generateObject.activeSelf)
        {
            iobjectContainer.RegistObject(generateObject);
            RegistObject(generateObject);
        }
    }

    public void Paused() { this.enabled = false; }
    public void Resumed() { this.enabled = true; }

    public void RegistObject(GameObject obj)
    {
        if (generateObject == null) 
        { 
            generateObject = obj;
        }
    }
    public void RemoveObject(GameObject obj)
    {
        if (generateObject == obj) 
        { 
            generateObject = null;
        }
    }
}
