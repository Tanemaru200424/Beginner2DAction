using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//プレイヤーが入ったら実行
//ボスを生成し登場させるイベントを実行させる。
public class BossGenerator : MonoBehaviour, IGenerator
{
    [SerializeField] private GameObject originBoss = null;
    [SerializeField] private Transform bossGenerateTrans = null;
    private GameObject generatedBoss = null;

    [SerializeField] private ActionObjectContainer actionObjectContainer = null;//ボスを登録するコンテナ
    private IObjectContainer iobjectContainer = null;

    public bool isEntry { get; private set; } = false;
    public bool isRetire { get; private set; } = false;
    public event Action OnBossDeath;

    [SerializeField] private ActionUIController actionUIController = null;
    [SerializeField] private AimPlayerManager aimPlayerManager = null;

    void Awake()
    {
        iobjectContainer = actionObjectContainer.GetComponent<IObjectContainer>();
    }

    public void SetObjectContainer(IObjectContainer iobjectContainer) { this.iobjectContainer = iobjectContainer; }
    public GameObject Generate(GameObject gameObject, Vector3 generatePos, Vector3 generateScale, float zAngle)
    {
        Quaternion newRotation = Quaternion.Euler(0, 0, zAngle);
        GameObject generatedBoss = Instantiate(gameObject, generatePos, newRotation);
        generatedBoss.transform.localScale = generateScale;
        return generatedBoss;
    }
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject)
    {
        if (generateObject.activeSelf) { iobjectContainer.RegistObject(generateObject); }
    }

    private void BossStartrFlip(bool isFlip) //生成時の向きを指定する。
    {
        if (generatedBoss != null)
        {
            if ((generatedBoss.transform.localScale.x > 0 && isFlip) ||
                (generatedBoss.transform.localScale.x < 0 && !isFlip))
            {
                generatedBoss.transform.localScale = Vector3.Scale(generatedBoss.transform.localScale, new Vector3(-1, 1, 1));
            }
        }
    }

    public void GenerateBoss()
    {
        if (generatedBoss != null) { return; }

        //ボスを生成しコンテナに登録
        generatedBoss = Generate(originBoss, bossGenerateTrans.position, originBoss.transform.localScale, 0);
        BossStartrFlip(bossGenerateTrans.localScale.x < 0);
        IContainedObject icontainedObject = generatedBoss.GetComponent<IContainedObject>();
        icontainedObject.OnRegist += () => iobjectContainer.RegistObject(generatedBoss);
        icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(generatedBoss);
        InitRegist(iobjectContainer, generatedBoss);

        //ボスの攻撃生成スクリプトにコンテナを登録
        IGenerator igenerator = generatedBoss.GetComponent<IGenerator>();
        igenerator?.SetObjectContainer(actionObjectContainer);

        IAimPlayer iaimPlayer = generatedBoss.GetComponent<IAimPlayer>();
        if (iaimPlayer != null) { aimPlayerManager.InitSetPlayerTrans(iaimPlayer); }

        //UIに与えるイベント設定。
        BossDataForUI dataForUI = generatedBoss.GetComponent<BossDataForUI>();
        dataForUI.OnHpChanged += actionUIController.SetBossHpRate;

        //ボスの登場退場イベントを設定。
        ICharactorEvents bossEvents = generatedBoss.GetComponent<ICharactorEvents>();
        bossEvents.OnBirthStart += () => BirthStart();
        bossEvents.OnBirthEnd += () => BirthEnd();
        bossEvents.OnDeathStart += () => DeathStart();
        bossEvents.OnDeathEnd += () => DeathEnd();
    }

    public void BossBirthStart() { generatedBoss.GetComponent<ICharactorEvents>().BirthStart(); }
    private void BirthStart()
    {
        isEntry = true;
    }
    private void BirthEnd()
    {
        isEntry = false;
    }

    private void DeathStart()
    {
        isRetire = true;
        OnBossDeath.Invoke();
    }
    private void DeathEnd()
    {
        isRetire = false;
    }
}
