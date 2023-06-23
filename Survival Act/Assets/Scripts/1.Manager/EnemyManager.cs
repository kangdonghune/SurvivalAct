using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public HashSet<EnemyController> EnemySet = new HashSet<EnemyController>();

    //������Ʈ���� �̵�
    //����Ʈ ������Ʈ���� ��� �� ó��?
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
        //clear �� null ���̴� �Ҵ���� ������ ������ �����ϳ� ���ϳ� ����
        EnemySet = null;
    }
}
