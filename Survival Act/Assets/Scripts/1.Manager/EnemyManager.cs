using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public HashSet<EnemyController> EnemySet = new HashSet<EnemyController>();

    //업데이트에서 이동
    //레이트 업데이트에서 사망 시 처리?
    public void FixedUpdate()
    {
        if (Managers.Game.IsLive == false)
            return;

        if (Managers.Game.Player == null || Managers.Game.Hp <= 0)
            return;

        foreach(EnemyController ctrl in EnemySet)
        {
            if (ctrl.IsLive == false)
                continue;
            ctrl.FixedUpdateByManager();
        }
    }

    public void Clear()
    {
        //clear 와 null 차이는 할당받은 데이터 영역을 유지하냐 안하냐 차이
        EnemySet = null;
    }
}
