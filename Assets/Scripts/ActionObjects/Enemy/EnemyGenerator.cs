using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//徘徊エネミー1体生成するギミック。起動時につき1体のみ生成可能。
public class EnemyGenerator : MonoBehaviour, IGenerator, IObjectContainer
{
    [System.Serializable] private struct GenerateData
    {
        public GameObject originEnemy;
        public Transform generateTrans;
        public GameObject originGenerateEffect;
        public Vector3 effectSize;
    }

    [SerializeField] private List<GenerateData> generateDataList = new List<GenerateData>();

    private List<GameObject> generatedEnemyList = new List<GameObject>(); //生成済みのインデックス
    private bool hasGenerated = false; //生成済みかどうか

    private IObjectContainer iobjectContainer = null;

    void Awake()
    {
        IAreaObject iareaObject = GetComponent<IAreaObject>();
        iareaObject.OnActive += () =>
        {
            hasGenerated = false;
        };
        iareaObject.OnDeactive += () =>
        {
            hasGenerated = false;
        };
    }

    public void SetObjectContainer(IObjectContainer iobjectContainer) { this.iobjectContainer = iobjectContainer; }
    public GameObject Generate(GameObject gameObject, Vector3 generatePos, Vector3 generateScale, float zAngle)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, zAngle);
        GameObject generatedEnemy = Instantiate(gameObject, generatePos, newRotation);
        generatedEnemy.transform.localScale = generateScale;
        return generatedEnemy;
    }
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject)
    {
        if (generateObject.activeSelf)
        {
            iobjectContainer.RegistObject(generateObject);
        }
    }

    public void RegistObject(GameObject obj)
    {
        if(obj !=  null && !generatedEnemyList.Contains(obj)) { generatedEnemyList.Add(obj); }
    }
    public void RemoveObject(GameObject obj)
    {
        if(generatedEnemyList.Contains(obj)) { generatedEnemyList.Remove(obj); }
    }

    //生成位置は自分の位置、x軸のスケールによって敵を反転させたりする。
    public void GenerateEnemy()
    {
        //生成済みなら何もしない
        //1体でも生成済みの敵がいる場合は何もしない
        if (hasGenerated || generatedEnemyList.Count > 0) { return; }

        foreach (GenerateData generateData in generateDataList)
        {
            GameObject originEnemy = generateData.originEnemy;
            Vector3 generatePos = generateData.generateTrans.position;
            GameObject originGenerateEffect = generateData.originGenerateEffect;
            Vector3 effectSize = generateData.effectSize;

            GameObject enemy = Generate(originEnemy, generatePos, originEnemy.transform.localScale, 0);

            //敵をゲームオブジェクトコンテナに登録
            IContainedObject icontainedObject = enemy.GetComponent<IContainedObject>();
            icontainedObject.OnRegist += () => iobjectContainer.RegistObject(enemy);
            icontainedObject.OnRegist += () => RegistObject(enemy);
            icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(enemy);
            icontainedObject.OnRemove += () => RemoveObject(enemy);
            InitRegist(iobjectContainer, enemy);
            InitRegist(this, enemy);

            //エフェクト生成スクリプト等にコンテナ登録。
            IGenerator igenerator = enemy.GetComponent<IGenerator>();
            igenerator?.SetObjectContainer(iobjectContainer);

            IAimPlayer enemyIAimPlayer = enemy.GetComponent<IAimPlayer>();
            if (enemyIAimPlayer != null)
            {
                enemyIAimPlayer.SetPlayerTrans(GetComponent<IAimPlayer>().GetPlayerTrans());
            }

            GameObject effect = Generate(originGenerateEffect, generatePos, effectSize, 0);
            icontainedObject = effect.GetComponent<IContainedObject>();
            icontainedObject.OnRegist += () => iobjectContainer.RegistObject(effect);
            icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(effect);
            InitRegist(iobjectContainer, effect);
        }
        hasGenerated = true;
    }
}
