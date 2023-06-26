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
    //메모리 캐싱 관련해서도 연속된 메모리가 장점이 있다.
    //근데 이부분은 링크드 리스트 대신에 리스트를 사용했을 떄 장점이 있는지는 고민해봐야 할 문제
    public List<EnemyController> EnemyLst = new List<EnemyController>();

    //업데이트에서 이동
    //레이트 업데이트에서 사망 시 처리?
    public void FixedUpdate()
    {
        //Move();


        if (Managers.Game.IsLive == false)
            return;

        if (Managers.Game.Player == null || Managers.Game.Hp <= 0)
            return;

        for (int idx = 0; idx < EnemyLst.Count; idx++)
        {
            //적이 죽은 상태이거나 피격 중인 상태라면
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
        //clear 와 null 차이는 할당받은 데이터 영역을 유지하냐 안하냐 차이
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
