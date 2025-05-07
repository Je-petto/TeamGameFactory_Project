using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class CharacterChange : MonoBehaviour
{
    [Header("스폰 할 캐릭터")]
    public List<GameObject> Characters = new List<GameObject>(); // 스폰할 캐릭터 프리팹 리스트
    public Transform[] SpwanPoint; // 캐릭터 스폰 지점 배열
    [ReadOnly] public List<GameObject> spawnedCharacters = new List<GameObject>(); // 실제 스폰된 캐릭터 오브젝트 리스트

    private void Start()
    {
        CharacterSpwanPoint(); // 1. 스폰 포인트 초기화
        CharacterSpwan();      // 2. 초기 캐릭터 스폰 실행
    }

    // 캐릭터 스폰 메인 함수
    public void CharacterSpwan()
    {
        // 스폰 가능한 최소 개수 계산 (스폰 지점 vs 캐릭터 프리팹 중 작은 값)
        int countSpawn = Mathf.Min(SpwanPoint.Length, Characters.Count);
        
        // 계산된 개수만큼 캐릭터 생성
        for (int i = 0; i < countSpawn; i++)
        {
            // 스폰 지점에 캐릭터 생성 및 부모 설정
            GameObject character = Instantiate(Characters[i], SpwanPoint[i].position, SpwanPoint[i].rotation, SpwanPoint[i]);
            spawnedCharacters.Add(character); // 생성된 캐릭터 관리 리스트에 추가
        }
    }

    // 스폰 포인트 초기화 함수
    private void CharacterSpwanPoint()
    {
        // 현재 오브젝트의 자식 오브젝트들을 스폰 포인트로 지정
        SpwanPoint = new Transform[transform.childCount];
        
        // 모든 자식 오브젝트의 Transform 컴포넌트 추출
        for (int i = 0; i < SpwanPoint.Length; i++)
        {
            SpwanPoint[i] = transform.GetChild(i).transform;
        }
    }

    // 캐릭터 위치/회전 업데이트 함수
    private void UpdateCharacterTransforms()
    {
        // 모든 스폰된 캐릭터에 대해 위치/회전/부모 설정 갱신
        for (int i = 0; i < spawnedCharacters.Count; i++)
        {
            // 스폰 포인트 개수보다 많은 캐릭터가 있을 경우를 대비한 안전장치
            if (i < SpwanPoint.Length)
            {
                spawnedCharacters[i].transform.position = SpwanPoint[i].position;
                spawnedCharacters[i].transform.rotation = SpwanPoint[i].rotation;
                spawnedCharacters[i].transform.parent = SpwanPoint[i];
            }
        }
    }

    // 왼쪽 버튼 클릭 시 캐릭터 순환
    public void OnLButtonSwap()
    {
        if (spawnedCharacters.Count <= 1) return; // 1개 이하일 때 동작 방지

        // 리스트 순환 알고리즘: 첫 번째 요소를 끝으로 이동
        GameObject firstCharacter = spawnedCharacters[0];
        spawnedCharacters.RemoveAt(0);
        spawnedCharacters.Add(firstCharacter);//자동으로 Element2를 생성하여 앞으로 밀어버림

        // 선택 플레이어 인덱스 순환 처리
        // 0 = (0 == 2) ? 0(X) : 0 + 1(O); 
        // -> GameManager.selectPlayer = 1
        // 2 = (2 == 2) ? 0(O) : 2 + 1(X); 
        // -> GameManager.selectPlayer = 0

        // if (GameManager.selectPlayer == 2)
        //     GameManager.selectPlayer = 0;
        // else
        //     GameManager.selectPlayer++;

        GameManager.selectPlayer = (GameManager.selectPlayer == 2) ? 0 : GameManager.selectPlayer + 1;

        UpdateCharacterTransforms(); // 변경된 순서에 따라 위치 갱신
    }

    // 오른쪽 버튼 클릭 시 캐릭터 순환
    public void OnRButtonSwap()
    {
        if (spawnedCharacters.Count <= 1) return;

        // 리스트 순환 알고리즘: 마지막 요소를 처음으로 이동
        GameObject lastCharacter = spawnedCharacters[spawnedCharacters.Count - 1];
        // 스폰된 프리펨 리스트에서 제일 마지막 프리펩 요소를 제거
        spawnedCharacters.RemoveAt(spawnedCharacters.Count - 1);
        // 제거된 프리펩을 직접 0번째 Index에 넣음
        spawnedCharacters.Insert(0, lastCharacter);

        // 선택 플레이어 인덱스 역순환 처리
        GameManager.selectPlayer = (GameManager.selectPlayer == 0) ? 2 : GameManager.selectPlayer - 1;

        UpdateCharacterTransforms(); // 변경된 순서에 따라 위치 갱신
    }
}