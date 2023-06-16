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
    //���� ��ǥ -> Cell ��ǥ. Grid ���� �� ��ǥ ����
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
        if(_cellDic.TryGetValue(cellPos, out cell) == false) //ó�� ���� ��ǥ���
        {
            cell = new Cell();
            _cellDic.Add(cellPos, cell);
        }

        return cell;
    }


    public List<GameObject> GetObjects(Vector3 pos, float range)
    {
        List<GameObject> objects = new List<GameObject>();
        //grid�� ũ�� �ؼ� ������ 1���� ���� ���̳� 
        //grid�� �۰� �ϰ� �����¿� cell�� �ؼ� ���������� ì�� ���̳� <-- �����ϰ� ���� �������ϸ��� �غ��� ���ϰ� ���ϴٸ� ���� �������
        //�������� ���̱� ���� new Vector �������� ���� ���� ������ ���� ����� ��������.(�ش� �Լ��� ȣ���� �����)
        Vector3Int left = _grid.WorldToCell(pos + Vector3.left * range);
        Vector3Int right = _grid.WorldToCell(pos + Vector3.right * range);
        Vector3Int up= _grid.WorldToCell(pos + Vector3.up * range);
        Vector3Int down = _grid.WorldToCell(pos + Vector3.down * range);

        int minX = left.x;
        int maxX = right.x;
        int minY = down.y;
        int maxY = up.y;


        //Ȥ�ö� vector3int �� z ���� 0�̶� Ű Ž���� �ȵǴ� ��찡 �ִٸ� ������ ��.
        Vector3Int curVec = Vector3Int.zero; 
        for(int x = minX; x <= maxX; x++)
        {
            for(int y = minY; y <= maxY; y++)
            {
                curVec.x = x;
                curVec.y = y;
                if (_cellDic.ContainsKey(curVec) == false) //�湮�� �� ���� Cell
                    continue;
                //addrange�� add�� ���� ȿ�� ���̰� ũ��.
                objects.AddRange(_cellDic[curVec].Objects);
            }
        }

        return objects;
    }

}
