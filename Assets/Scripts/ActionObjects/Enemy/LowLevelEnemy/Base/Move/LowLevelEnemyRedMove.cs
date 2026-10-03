using UnityEngine;

public class LowLevelEnemyRedMove : LowLevelEnemyMove
{
    [SerializeField] private GroundChecker groundWalkStopper = null;

    protected override Vector3 UpdateScale()
    {
        Vector3 result = this.transform.localScale;
        if (state.CanTurn() && (wallWalkStopper.IsGround() || !groundWalkStopper.IsGround()))
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        return result;
    }
}
