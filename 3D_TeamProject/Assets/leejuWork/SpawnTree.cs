using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTree : MonoBehaviour
{
    //나무데이터SO에서 끌어오기
    public SOSpawnTreeData treeData;
    //생성될 나무 프리팹
    public GameObject TreePrefab;
    //반환할 자원 프리팹
    public GameObject ReturnTreeTrefab;

    public float SpawnTreeRangeX;
    public float SpawnTreeRangeZ;

    //맵에 최대 나무 생성 개수
    public int MaxTreeCount = 5;
    public int CurTreeCount = 0;

    //어떤걸 넣어야 하지?

    //생성될 나무의 프리팹
    //생성될 나무의 위치

    //부쉈을 때 떨어지는 아이템 프리팹
    //일단 스포너로 만들까?
    //맵에 있어야할 최대 나무 스폰 개수
    //이거 셋액티브로 만들면 빈공간에서도 자원캐지는 오류 발생 할까? 


    void Start()
    {
        
    }

    void Update()
    {
        //코루틴은 나중에 추가할것 인벤토리 우선 구현
        //임시로 최대 스폰개수 5개까지로만 방어코드
        if (CurTreeCount <= MaxTreeCount)
        {
            SpawnTreeLogic();
        }
        else return;
    }

    void SpawnTreeLogic()
    {
        //스폰될 나무 범위의 위치 X값 범위
        float CalTreeRangeX = Random.Range(-SpawnTreeRangeX, SpawnTreeRangeX);
        //스폰될 나무 범위의 위치 Z값 범위
        float CalTreeRangez = Random.Range(-SpawnTreeRangeZ, SpawnTreeRangeZ);

        //최종 스폰될 나무 x,Z범위를 확정
        Vector3 ConfirmTreePos= new Vector3(CalTreeRangeX, 2.5f, CalTreeRangez);

        //계산된 정보를 바탕으로 씬에 오브젝트 생성
        Instantiate(TreePrefab, ConfirmTreePos, Quaternion.identity);
        
        //현재 생성된 나무의 개수를 생성될때마다 1씩 증가
        CurTreeCount += 1;
    }
}
