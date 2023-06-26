using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine.Jobs;

public class EnemyManager
{
    //�޸� ĳ�� �����ؼ��� ���ӵ� �޸𸮰� ������ �ִ�.
    //�ٵ� �̺κ��� ��ũ�� ����Ʈ ��ſ� ����Ʈ�� ������� �� ������ �ִ����� ����غ��� �� ����
    public List<EnemyController> EnemyLst = new List<EnemyController>();

    //������Ʈ���� �̵�
    //����Ʈ ������Ʈ���� ��� �� ó��?
    public void FixedUpdate()
    {
        //Move();


        if (Managers.Game.IsLive == false)
            return;

        if (Managers.Game.Player == null || Managers.Game.Hp <= 0)
            return;

        for (int idx = 0; idx < EnemyLst.Count; idx++)
        {
            //���� ���� �����̰ų� �ǰ� ���� ���¶��
            if (EnemyLst[idx].IsLive == false)
                continue;
            EnemyLst[idx].FixedUpdateByManager();
        }
    }

    public void Move()
    {
        if (EnemyLst.Count == 0)
            return;

        if (Managers.Game.IsLive == false)
            return;

        if (Managers.Game.Player == null || Managers.Game.Hp <= 0)
            return;

        NativeArray<Vector3> positionarr = new NativeArray<Vector3>(EnemyLst.Count, Allocator.TempJob);
        NativeArray<float> speedArr = new NativeArray<float>(EnemyLst.Count, Allocator.TempJob);
        for (int idx = 0; idx < EnemyLst.Count; idx++)
        {
            positionarr[idx] = EnemyLst[idx].transform.position;
            speedArr[idx] = EnemyLst[idx].Speed;
        }

        MoveEnemysParallelJob moveEnemysParallelJob = new MoveEnemysParallelJob {positionarr = positionarr ,deltaTime = Time.deltaTime, speedArr = speedArr,targtPos = EnemyController.Target.position };
        JobHandle handle = moveEnemysParallelJob.Schedule(EnemyLst.Count, 50);
        handle.Complete();

        for (int idx = 0; idx < EnemyLst.Count; idx++)
        {
            if (EnemyLst[idx].IsLive == false)
                continue;
            EnemyLst[idx].Move(positionarr[idx]);
        }

        positionarr.Dispose();
        speedArr.Dispose();
    }

    public void Clear()
    {
        //clear �� null ���̴� �Ҵ���� ������ ������ �����ϳ� ���ϳ� ����
        EnemyLst = null;
    }
}

[BurstCompile]
public struct MoveEnemysParallelJob : IJobParallelFor
{
    public NativeArray<Vector3> positionarr;
    [ReadOnly]
    public Vector3 targtPos;
    [ReadOnly]
    public NativeArray<float> speedArr;
    [ReadOnly]
    public float deltaTime;

    public void Execute(int index)
    {
        Vector3 dir = (targtPos - positionarr[index]).normalized;
        positionarr[index] += dir * speedArr[index] * deltaTime;
    }
}
