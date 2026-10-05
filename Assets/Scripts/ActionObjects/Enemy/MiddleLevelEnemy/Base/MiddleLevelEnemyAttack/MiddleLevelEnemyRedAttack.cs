using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLevelEnemyRedAttack : MiddleLevelEnemyAttack
{
    //アニメー所にベントが呼ぶ弾発射
    public override void Attack()
    {
        Vector2 playerVector = (aimPlayer.GetPlayerPos() - this.transform.position).normalized;
        Vector2 baseVector = Vector2.right; ;
        float angle = Vector2.SignedAngle(baseVector, playerVector);

        bool isFlip = (this.transform.localScale.x < 0f);
        float zAngle = 0.0f;
        if(angle > 30.0f && angle < 150.0f) { zAngle = isFlip ? 135.0f : 45.0f; }
        else if (angle > -150.0f && angle < -30.0f) { zAngle = isFlip ? -135.0f : -45.0f; }
        else { zAngle = isFlip ? 180.0f : 0.0f; }

        effectsGenerator.GenerateBullet(shootTrans.position, zAngle);
    }
}
