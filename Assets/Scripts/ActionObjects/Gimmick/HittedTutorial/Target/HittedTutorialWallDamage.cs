using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedTutorialWallDamage : MonoBehaviour
{
    private int targetCount = 3;
    private HittedTutorialWallEffectGenerator effGenerator = null;

    void Awake()
    {
        effGenerator = GetComponent<HittedTutorialWallEffectGenerator>();
    }

    public void Init() { targetCount = 3; }
    public void CountDown() 
    {  
        targetCount--; 
        if(targetCount <= 0) 
        {
            effGenerator.GenerateDestroyEffect();
            this.gameObject.SetActive(false); 
        }
    }
}
