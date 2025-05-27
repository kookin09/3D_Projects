using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingManager : MonoBehaviour
{
    // 임시 (tab) 키를 눌러서 건설 메뉴를 불러옴
    // 유령(프리뷰) 오브젝트가 마우스 위치에 따라 이동/연결
    // 설치 가능 위치면 "유효" 색상, 불가능하면 "무효" 색상
    // 클릭 시 실제 건설, 연결 상태 갱신
    // 할일 : 플레이어 화면 움직임 연동 (건설 메뉴 열었을 때 화면이 돌아가지 않게끔 설정해주기)
    //        가구 추가해보기 (작업대도 만들어야 할것 같네...)
    //        아이템 구현 완성되면 연동해서 재료 소모방식 추가

    [Header("건축 물체")]
    [SerializeField] private List<GameObject> floorObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> wallObjects = new List<GameObject>();

    [Header("건축 설정")]
    [SerializeField] private SelectedBuildType currentBuildType;
    [SerializeField] private LayerMask connectorLayer;

    [Header("건축 해제 설정")]
    [SerializeField] private bool isDestroying = false;
    private Transform lastHitDestroyTransfrom;
    private List<Material> LastHitMaterials = new List<Material>();

    [Header("프리뷰 설정")]
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;
    [SerializeField] private float connectorOverlapRadious = 1f;
    [SerializeField] private float maxGroundAngle = 45f;    // 90도 가능

    [Header("Internal State 내적 상태")]
    [SerializeField] private bool isBuilding = false;
    [SerializeField] private int currentBuildingIndex;
    private GameObject ghostBuildGameObject;
    private bool isGhostInValidPosition = false;
    private Transform ModelParent = null;

    [Header("UI")]
    [SerializeField] private GameObject buildingUI;
    [SerializeField] private TMP_Text destroyText;

    private void Update()
    {
        // input system 으로 추후 변경
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleBuildingUI(!buildingUI.activeInHierarchy);
        }

        // 건설 모드일 때
        if (isBuilding && !isDestroying)
        {
            GhostBuild();   // 프리뷰(유령) 건축물 처리

            // 마우스 왼쪽 클릭 시 건설 시도
            if (Input.GetMouseButtonDown(0))
                PlaceBuild();
        }
        else if (ghostBuildGameObject)  // 건설 모드 아닐 때, 프리뷰 오브젝트 파괴
        {
            Destroy(ghostBuildGameObject);
            ghostBuildGameObject = null;
        }

        if (isDestroying)
        {
            GhostDestroy();

            if (Input.GetMouseButtonDown(0))
                DestroyBuild();
        }
    }

    private void GhostBuild()
    {
        GameObject currentBuild = GetCurrentBuild();    // 현재 선택된 건축 프리팹 얻기
        CreateGhostPrefab(currentBuild);                // 프리뷰(유령) 프리팹 생성

        MoveGhostPrefabToRaycast();                     // 마우스 포인터 기준으로 프리뷰 위치 이동
        CheckBuildValidity();                           // 설치 가능/불가능 상태 확인
    }

    private void CreateGhostPrefab(GameObject currentBuild)
    {
        // 프리뷰 오브젝트가 아직 없으면 생성
        if (ghostBuildGameObject == null)
        {
            ghostBuildGameObject = Instantiate(currentBuild);

            ModelParent = ghostBuildGameObject.transform.GetChild(0);   // 모델의 루트 트랜스폼 (보통 첫번째 자식)

            GhostifyModel(ModelParent, ghostMaterialValid);             // 프리뷰(유령) 재질 적용 (유효한 위치 기준)
            GhostifyModel(ghostBuildGameObject.transform);              // 콜라이더 비활성화
        }
    }

    private void MoveGhostPrefabToRaycast()
    {
        // 화면의 마우스 위치를 기준으로 Ray 쏨
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            // 레이 맞은 지점에 프리뷰 이동
            ghostBuildGameObject.transform.position = hit.point;
        }
    }

    private void CheckBuildValidity()
    {
        // 프리뷰 오브젝트 주변에 연결 가능한 Connector 있는지 확인
        Collider[] colliders = Physics.OverlapSphere(ghostBuildGameObject.transform.position, connectorOverlapRadious, connectorLayer);
        if (colliders.Length > 0)
        {
            // 연결 가능한 오브젝트 존재 시 연결 시도
            GhostConnectBuild(colliders);
        }
        else
        {
            // 연결 불가(분리된) 상태 처리
            GhostSeperateBuild();

            if (isGhostInValidPosition)
            {
                Collider[] overlapColliders = Physics.OverlapBox(ghostBuildGameObject.transform.position, new Vector3(2f, 2f, 2f), ghostBuildGameObject.transform.rotation);
                foreach (Collider overlapCollider in overlapColliders)
                {
                    if (overlapCollider.gameObject != ghostBuildGameObject && overlapCollider.transform.root.CompareTag("Buildables"))
                    {
                        GhostifyModel(ModelParent, ghostMaterialInvalid);
                        isGhostInValidPosition = false;
                        return;
                    }
                }
            }
        }
    }

    private void GhostConnectBuild(Collider[] colliders)
    {
        Connector bestConnector = null;

        // 주변 Connector 중 연결 가능한 첫 번째 찾기
        foreach (Collider collider in colliders)
        {
            Connector connector = collider.GetComponent<Connector>();

            if (connector.canConnectTo)
            {
                bestConnector = connector;
                break;
            }
        }

        // 연결 조건을 만족하지 않으면 유효하지 않은 상태로 처리
        if (bestConnector == null || currentBuildType == SelectedBuildType.Floor && bestConnector.isConnectedToFloor || currentBuildType == SelectedBuildType.Wall && bestConnector.isConnectedToWall)
        {
            GhostifyModel(ModelParent, ghostMaterialInvalid);
            isGhostInValidPosition = false;
            return;
        }

        // 연결 지점에 스냅(착 달라붙기)
        SnapGhostPrefabToConnector(bestConnector);
    }

    private void SnapGhostPrefabToConnector(Connector connector)
    {
        // 프리뷰 오브젝트에서 스냅할 커넥터 찾기
        Transform ghostConnector = FindSnapConnector(connector.transform, ghostBuildGameObject.transform.GetChild(1));
        // 실제 커넥터 위치에 프리뷰 위치를 맞춤
        ghostBuildGameObject.transform.position = connector.transform.position - (ghostConnector.position - ghostBuildGameObject.transform.position);

        // 벽 설치 시, 회전도 맞춰줌 (Y축만)
        if (currentBuildType == SelectedBuildType.Wall)
        {
            Quaternion newRotation = ghostBuildGameObject.transform.rotation;
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, connector.transform.rotation.eulerAngles.y, newRotation.eulerAngles.z);
            ghostBuildGameObject.transform.rotation = newRotation;
        }

        // 스냅 성공 → 유효한 프리뷰 재질, 설치 가능 상태
        GhostifyModel(ModelParent, ghostMaterialValid);
        isGhostInValidPosition = true;
    }

    private void GhostSeperateBuild()
    {
        // 레이캐스트로 바닥/설치 영역 검사
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (currentBuildType == SelectedBuildType.Wall)          // 벽은 따로 분리 설치 불가 (무조건 연결 필요)
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
                return;
            }           

            // 바닥 각도(경사도) 검사: 설정된 최대 각도 이하면 설치 가능
            if (Vector3.Angle(hit.normal, Vector3.up) < maxGroundAngle)
            {
                GhostifyModel(ModelParent, ghostMaterialValid);
                isGhostInValidPosition = true;
            }
            else
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
            }
        }
    }

    private Transform FindSnapConnector(Transform snapConnector, Transform ghostConnectorParent)
    {
        // 연결할 커넥터와 반대 위치의 커넥터를 프리뷰에서 찾음
        ConnectorPosition OppositeConnectorTag = GetOppositePosition(snapConnector.GetComponent<Connector>());

        foreach (Connector connector in ghostConnectorParent.GetComponentsInChildren<Connector>())
        {
            if (connector.connectorPosition == OppositeConnectorTag)
                return connector.transform;
        }

        return null;
    }

    private ConnectorPosition GetOppositePosition(Connector connector)
    {
        ConnectorPosition position = connector.connectorPosition;

        // 벽을 바닥에 붙일 때는 항상 아래쪽
        if (currentBuildType == SelectedBuildType.Wall && connector.connectorParentType == SelectedBuildType.Floor)
            return ConnectorPosition.Bottom;

        // 바닥을 벽에 붙일 때, 붙일 벽의 위치/회전에 따라 다르게 처리
        if (currentBuildType == SelectedBuildType.Floor && connector.connectorParentType == SelectedBuildType.Wall && connector.connectorPosition == ConnectorPosition.Top)
        {
            if (connector.transform.root.rotation.y == 0)
            {
                return GetConnectorClosestToPlayer(true);
            }
            else
            {
                return GetConnectorClosestToPlayer(false);
            }
        }

        // 일반적인 반대 방향(좌우, 상하) 리턴
        switch (position)
        {
            case ConnectorPosition.Left:
                return ConnectorPosition.Right;
            case ConnectorPosition.Right:
                return ConnectorPosition.Left;
            case ConnectorPosition.Bottom:
                return ConnectorPosition.Top;
            case ConnectorPosition.Top:
                return ConnectorPosition.Bottom;
            default:
                return ConnectorPosition.Bottom;
        }
    }

    // 플레이어(카메라) 위치에 따라 위/아래/좌/우 결정
    private ConnectorPosition GetConnectorClosestToPlayer(bool topBottom)
    {
        Transform cameraTransform = Camera.main.transform;

        if (topBottom)
            return cameraTransform.position.z >= ghostBuildGameObject.transform.position.z ? ConnectorPosition.Bottom : ConnectorPosition.Top;
        else
            return cameraTransform.position.x >= ghostBuildGameObject.transform.position.x ? ConnectorPosition.Left : ConnectorPosition.Right;
    }

    // 모델의 모든 MeshRenderer에 유령(프리뷰) 재질 적용, 콜라이더 비활성화
    private void GhostifyModel(Transform modelParent, Material ghostMaterial = null)
    {
        if (ghostMaterial != null)
        {
            // 유령 재질 적용
            foreach (MeshRenderer meshRenderer in modelParent.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = ghostMaterial;
            }
        }
        else
        {
            // 콜라이더 비활성화
            foreach (Collider modelCollider in modelParent.GetComponentsInChildren<Collider>())
            {
                modelCollider.enabled = false;
            }
        }
    }

    // 현재 건설 타입에 따라 사용할 프리팹 반환
    private GameObject GetCurrentBuild()
    {
        switch (currentBuildType)
        {
            case SelectedBuildType.Floor:
                return floorObjects[currentBuildingIndex];
            case SelectedBuildType.Wall:
                return wallObjects[currentBuildingIndex];
        }

        return null;
    }

    // 실제 건설(배치) 처리
    private void PlaceBuild()
    {
        if (ghostBuildGameObject != null && isGhostInValidPosition)
        {
            // 프리뷰 위치에 실제 오브젝트 생성
            GameObject newBuild = Instantiate(GetCurrentBuild(), ghostBuildGameObject.transform.position, ghostBuildGameObject.transform.rotation);

            // 프리뷰 삭제
            Destroy(ghostBuildGameObject);
            ghostBuildGameObject = null;

            //isBuilding = false;

            // 새로 설치한 오브젝트의 연결 상태 갱신
            foreach (Connector connector in newBuild.GetComponentsInChildren<Connector>())
            {
                connector.UpdateConnectors(true);
            }
        }
    }

    // 마우스 위치 기준으로 삭제 가능한 오브젝트(빌드) 미리보기 처리
    private void GhostDestroy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.root.CompareTag("Buildables"))    // Buildables 태그를 가진 오브젝트 위에 있을 때
            {
                if (!lastHitDestroyTransfrom)                   // 처음으로 삭제 대상 감지 시
                {
                    lastHitDestroyTransfrom = hit.transform.root;

                    // 원래 머티리얼 저장
                    LastHitMaterials.Clear();
                    foreach (MeshRenderer lastHitMeshRenderers in lastHitDestroyTransfrom.GetComponentsInChildren<MeshRenderer>())
                    {
                        LastHitMaterials.Add(lastHitMeshRenderers.material);
                    }

                    // 삭제 미리보기: 유령 머티리얼(Invalid)로 변경
                    GhostifyModel(lastHitDestroyTransfrom.GetChild(0), ghostMaterialInvalid);
                }
                else if (hit.transform.root != lastHitDestroyTransfrom)     // 삭제 대상이 바뀌면 원상복귀
                {
                    ResetLastHitDestroyTransfrom();
                }
            }
            else if (lastHitDestroyTransfrom)                               // 빌드 오브젝트가 아니거나 커서가 벗어나면 원상복귀
            {
                ResetLastHitDestroyTransfrom();
            }
        }
    }

    // 마지막으로 삭제 미리보기 적용했던 오브젝트의 머티리얼 복원
    private void ResetLastHitDestroyTransfrom()
    {
        int counter = 0;
        foreach (MeshRenderer lastHitMeshRenderers in lastHitDestroyTransfrom.GetComponentsInChildren<MeshRenderer>())
        {
            lastHitMeshRenderers.material = LastHitMaterials[counter];
            counter++;
        }

        lastHitDestroyTransfrom = null;
    }

    // 실제로 빌드 오브젝트를 삭제하는 함수
    private void DestroyBuild()
    {
        if (lastHitDestroyTransfrom)
        {
            // 삭제 대상의 모든 Connector 비활성화 및 상태 갱신
            foreach (Connector connector in lastHitDestroyTransfrom.GetComponentsInChildren<Connector>())
            {
                connector.gameObject.SetActive(false);
                connector.UpdateConnectors(true);
            }

            // 오브젝트 삭제
            Destroy(lastHitDestroyTransfrom.gameObject);

            // 파괴 모드 토글(Off) 및 상태 초기화
            DestroyBuildingToggle(true);
            lastHitDestroyTransfrom = null;
        }
    }

    // 건설 관련 UI(패널 등) 토글, 커서 표시 등 관리
    public void ToggleBuildingUI(bool active)
    {
        isBuilding = false;

        buildingUI.SetActive(active);

        // 카메라 움직임 멈춤 필요

        // 커서 활성화/비활성화 (화면 움직이 추가되면 사용될 코드)
        //Cursor.visible = active;
        //Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // 파괴 모드 on/off 스위치 관리 (UI 갱신 포함)
    public void DestroyBuildingToggle(bool fromScript = false)
    {
        if (fromScript)
        {
            isDestroying = false;
            destroyText.text = "Destroy Off";
            destroyText.color = Color.green;
        }
        else
        {
            isDestroying = !isDestroying;
            destroyText.text = isDestroying ? "Destroy On" : "Destroy Off";
            destroyText.color = isDestroying ? Color.red : Color.green;
            ToggleBuildingUI(false);
        }
    }

    // 버튼 클릭 등으로 건축 타입 변경 (Floor, Wall 등)
    public void ChangeBuildTypeButton(string selectedBuildType)
    {
        if (System.Enum.TryParse(selectedBuildType, out SelectedBuildType result))
        {
            currentBuildType = result;
        }
        else
        {
            Debug.Log("건축 유형이 없습니다");
        }
    }

    public void StartBuildingButton(int buildIndex)
    {
        currentBuildingIndex = buildIndex;
        ToggleBuildingUI(false);

        isBuilding = true;
    }
}

[System.Serializable]
public enum SelectedBuildType
{
    Floor,
    Wall
}
