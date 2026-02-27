using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEGenerator : MonoBehaviour, IGenerator, IObjectContainer
{
    [SerializeField] private GameObject originEnemy = null;
    private GameObject enemy = null; //生成済みかどうか

    [SerializeField] private ActionObjectContainer actionObjectContainer = null;
    private IObjectContainer iobjectContainer = null;
    [SerializeField] private AimPlayerManager aimPlayerManager = null;

    IContainedObject icontainedObject = null;

    void Awake()
    {
        SetObjectContainer(actionObjectContainer.GetComponent<IObjectContainer>());

        icontainedObject = GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => GenerateEnemy();
    }

    public void SetObjectContainer(IObjectContainer iobjectContainer) { this.iobjectContainer = iobjectContainer; }
    public GameObject Generate(GameObject gameObject, Vector3 generatePos, Vector3 generateScale, float zAngle)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, zAngle);
        GameObject generatedEnemy = Instantiate(originEnemy, generatePos, newRotation);
        generatedEnemy.transform.localScale = generateScale;
        return generatedEnemy;
    }
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject)
    {
        if (generateObject.activeSelf) { iobjectContainer.RegistObject(generateObject); }
    }

    public void RegistObject(GameObject obj)
    {
        if (obj != null) { enemy = obj; }
    }
    public void RemoveObject(GameObject obj)
    {
        if (enemy == obj) { enemy = null; }
    }

    //生成位置は自分の位置、x軸のスケールによって敵を反転させたりする。
    private void GenerateEnemy()
    {
        if (enemy != null) { return; }
        enemy = Generate(originEnemy, this.transform.position, new Vector3(1, 1, 1), 0);

        //敵をゲームオブジェクトコンテナに登録
        IObjectContainer iobjectContainer = actionObjectContainer.GetComponent<IObjectContainer>();
        IContainedObject icontainedObject = enemy.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => iobjectContainer.RegistObject(enemy);
        icontainedObject.OnRegist += () => RegistObject(enemy);
        icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(enemy);
        icontainedObject.OnRemove += () => RemoveObject(enemy);
        InitRegist(iobjectContainer, enemy);

        //エフェクト生成スクリプト等にコンテナ登録。
        IGenerator igenerator = enemy.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(actionObjectContainer);

        //プレイヤーをターゲットとして登録
        IAimPlayer iaimPlayer = enemy.GetComponent<IAimPlayer>();
        if(iaimPlayer != null) { aimPlayerManager.InitSetPlayerTrans(iaimPlayer); }
    }
}
