using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public ConnectorPosition connectorPosition;
    public SelectedBuildType connectorParentType;

    [HideInInspector] public bool isConnectedToFloor = false;
    [HideInInspector] public bool isConnectedToWall = false;
    [HideInInspector] public bool canConnectTo = true;

    [SerializeField] private bool canConnectToFloor = true;
    [SerializeField] private bool canConnectToWall = true;

    private void OnDrawGizmos()
    {
        // 빨강 - 어디어도 설치 불가, 파랑 - 벽에만 설치 가능, 초록 - 바닥 벽 둘다 설치 가능, 노랑 - 바닥에만 설치 가능
        Gizmos.color = isConnectedToFloor ? (isConnectedToWall ? Color.red : Color.blue) : (!isConnectedToWall ? Color.green : Color.yellow);
        Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x / 2f);
    }

    public void UpdateConnectors(bool rootCall = false)
    {
        // 자기 위치를 중심으로 일정 반경 내에 있는 모든 Collider 검색
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.lossyScale.x / 2f);

        // 연결 상태 초기화 (연결 불가 상태로 설정)
        isConnectedToFloor = !canConnectToFloor;
        isConnectedToWall = !canConnectToWall;

        foreach (Collider collider in colliders)
        {
            // 자기 자신은 무시
            if (collider.GetInstanceID() == GetComponent<Collider>().GetInstanceID())
            {
                continue;
            }

            if (!collider.gameObject.activeInHierarchy)
            {
                continue;
            }

            // 같은 레이어의 오브젝트만 연결 대상으로 삼음
            if (collider.gameObject.layer == gameObject.layer)
            {
                // 해당 오브젝트에서 Connector 컴포넌트 가져오기
                Connector foundConnector = collider.GetComponent<Connector>();

                // 주변 오브젝트가 Floor(바닥) 타입이면, 바닥 연결됨으로 설정
                if (foundConnector.connectorParentType == SelectedBuildType.Floor)
                    isConnectedToFloor = true;

                // 주변 오브젝트가 Wall(벽) 타입이면, 벽 연결됨으로 설정
                if (foundConnector.connectorParentType == SelectedBuildType.Wall)
                    isConnectedToWall = true;

                // rootCall 옵션이 true면, 인접 오브젝트의 연결 상태도 재귀적으로 갱신
                if (rootCall)
                    foundConnector.UpdateConnectors();
            }
        }

        // 일단 연결 가능 상태로 설정
        canConnectTo = true;

        // 바닥과 벽에 모두 연결되어 있으면 연결 불가로 설정
        if (isConnectedToFloor && isConnectedToWall)
            canConnectTo = false;
    }
}

[System.Serializable]
public enum ConnectorPosition
{
    Left,
    Right,
    Top,
    Bottom
}