using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float ScanRange;
    public LayerMask TargetLayer;
    public RaycastHit2D[] Targets;
    public Transform NearstTarget;

    private void FixedUpdate()
    {
        if (Managers.Game.IsLive == false)
            return;

        Targets = Physics2D.CircleCastAll(transform.position, ScanRange, Vector2.zero, 0, TargetLayer);
        NearstTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;

        float diff = float.MaxValue;

        foreach (RaycastHit2D hit in Targets)
        {
            float curDiff = (transform.position - hit.transform.position).magnitude;
            if (diff > curDiff)
            {
                diff = curDiff;
                result = hit.transform;
            }
        }
        return result;
    }

}
