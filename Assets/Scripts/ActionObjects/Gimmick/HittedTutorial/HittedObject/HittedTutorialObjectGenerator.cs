using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedTutorialObjectGenerator : MonoBehaviour, IGenerator, IObjectContainer, IPausable
{
    [SerializeField] private GameObject originObject = null;
    private bool isGenerated = false; //生成済みフラグ。
    [SerializeField] private float maxCoolTime = 0;
    private float coolTime = 0;

    private IObjectContainer iobjectContainer = null;

    private bool isPaused = false;

    void Awake()
    {
        IAreaObject iareaObject = GetComponent<IAreaObject>();
        iareaObject.OnActive += () =>
        {
            coolTime = 0;
            this.enabled = true;
        };
        iareaObject.OnDeactive += () =>
        {
            this.enabled = false;
        };
    }

    void Update()
    {
        if (!isPaused && !isGenerated)
        {
            //生成していない状態でクールタイムを減らす。
            if (coolTime > 0) { coolTime -= Time.deltaTime; }
            //クールタイムが終わって生成していないなら生成。
            else { GenerateObject(); }
        }
    }

    private void GenerateObject()
    {
        GameObject generatedObject = Generate(originObject, this.transform.position, originObject.transform.localScale, 0);
        IContainedObject icontainedObject = generatedObject.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => { iobjectContainer.RegistObject(generatedObject); RegistObject(generatedObject); };
        icontainedObject.OnRemove += () => { iobjectContainer.RemoveObject(generatedObject); RemoveObject(generatedObject); };
        InitRegist(iobjectContainer, generatedObject);

        //エフェクト生成にコンテナ登録。
        IGenerator igenerator = generatedObject.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(iobjectContainer);

        coolTime = maxCoolTime;
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

    public void Paused() { isPaused = true; }
    public void Resumed() { isPaused = false; }

    public void RegistObject(GameObject obj) { isGenerated = true; }
    public void RemoveObject(GameObject obj) { isGenerated = false; }
}
