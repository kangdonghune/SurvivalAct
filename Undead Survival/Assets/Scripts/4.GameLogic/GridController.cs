using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Cell
{
    public HashSet<GameObject> Objects { get; } = new HashSet<GameObject>();
}

public class GridController : MonoBehaviour
{
    Grid _grid;
    //월드 좌표 -> Cell 좌표. Grid 상의 셀 좌표 보관
    Dictionary<Vector3Int, Cell> _cellDic = new Dictionary<Vector3Int, Cell>();

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        _grid = gameObject.GetComponent<Grid>();
        if (_grid == null)
            _grid = gameObject.AddComponent<Grid>();
    }

    public void Add(GameObject go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);
        Cell cell = GetCell(cellPos);
        cell.Objects.Add(go);
    }

    public void Remove(GameObject go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);
        Cell cell = GetCell(cellPos);
        cell.Objects.Remove(go);
    }

    Cell GetCell(Vector3Int cellPos)
    {
        Cell cell = null;
        if(_cellDic.TryGetValue(cellPos, out cell) == false) //처음 만난 좌표라면
        {
            cell = new Cell();
            _cellDic.Add(cellPos, cell);
        }

        return cell;
    }


    public List<GameObject> GetObjects(Vector3 pos, float range)
    {
        List<GameObject> objects = new List<GameObject>();
        //grid를 크게 해서 연산을 1번만 해줄 것이냐 
        //grid를 작게 하고 상하좌우 cell을 해서 디테일함을 챙길 것이냐 <-- 선택하고 추후 프로파일링을 해보고 부하가 심하다면 위의 방법으로
        //가비지를 줄이기 위해 new Vector 구현보단 기존 벡터 가져다 쓰는 방법을 선택하자.(해당 함수는 호출이 빈번함)
        Vector3Int left = _grid.WorldToCell(pos + Vector3.left * range);
        Vector3Int right = _grid.WorldToCell(pos + Vector3.right * range);
        Vector3Int up= _grid.WorldToCell(pos + Vector3.up * range);
        Vector3Int down = _grid.WorldToCell(pos + Vector3.down * range);

        int minX = left.x;
        int maxX = right.x;
        int minY = down.y;
        int maxY = up.y;


        //혹시라도 vector3int 의 z 값이 0이라서 키 탐색이 안되는 경우가 있다면 수정할 것.
        Vector3Int curVec = Vector3Int.zero; 
        for(int x = minX; x <= maxX; x++)
        {
            for(int y = minY; y <= maxY; y++)
            {
                curVec.x = x;
                curVec.y = y;
                if (_cellDic.ContainsKey(curVec) == false) //방문한 적 없는 Cell
                    continue;
                //addrange가 add에 비해 효율 차이가 크다.
                objects.AddRange(_cellDic[curVec].Objects);
            }
        }

        return objects;
    }

}
