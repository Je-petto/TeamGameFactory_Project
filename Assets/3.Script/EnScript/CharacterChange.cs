using System.Collections.Generic;
using UnityEngine;

public class CharacterChange : MonoBehaviour
{
    [Header("스폰 할 캐릭터")]
    public List<GameObject> Characters = new List<GameObject>();
    public Transform[] SpwanPoint;
    private List<GameObject> spawnedCharacters = new List<GameObject>();

    private void Start()
    {
        CharacterSpwanPoint(); //스폰 포인트 먼저 설정
        CharacterSpwan();      //설정된 스폰 포인트에 캐릭터 스폰
    }

    public void CharacterSpwan()
    {
        // 스폰 포인트 개수와 캐릭터 프리팹 개수 중 작은 값만큼만 스폰
        int countSpawn = Mathf.Min(SpwanPoint.Length, Characters.Count);
        for (int i = 0; i < countSpawn; i++)
        {
            GameObject character = Instantiate(Characters[i], SpwanPoint[i].position, SpwanPoint[i].rotation, SpwanPoint[i]);
            spawnedCharacters.Add(character);
        }
    }

    private void CharacterSpwanPoint()
    {
        SpwanPoint = new Transform[transform.childCount];

        for (int i = 0; i < SpwanPoint.Length; i++)
        {
            SpwanPoint[i] = transform.GetChild(i).transform;
        }
    }

    private void UpdateCharacterTransforms()
    {
        for (int i = 0; i < spawnedCharacters.Count; i++)
        {
            //스폰 포인트가 캐릭터 수보다 많을 수 있으므로, spawnedCharacters.Count 만큼만 사용하고 SpwanPoint 배열의 범위를 넘지 않도록 함.
            if (i < SpwanPoint.Length)
            {
                spawnedCharacters[i].transform.position = SpwanPoint[i].position;
                spawnedCharacters[i].transform.rotation = SpwanPoint[i].rotation;
                spawnedCharacters[i].transform.parent = SpwanPoint[i];
            }
        }
    }

    public void OnLButtonSwap()
    {
        if (spawnedCharacters.Count <= 1) return; // 스왑할 캐릭터가 없거나 1개면 동작 안 함

        //spawnedCharacters 리스트를 왼쪽으로 한 칸씩 이동하며 순환
        GameObject firstCharacter = spawnedCharacters[0];
        spawnedCharacters.RemoveAt(0);
        spawnedCharacters.Add(firstCharacter);

        //변경된 리스트 순서를 따라서 캐릭터들의 위치를 계속 업데이트 업데이트
        UpdateCharacterTransforms();
    }

    public void OnRButtonSwap()
    {
        if (spawnedCharacters.Count <= 1) return;

        //spawnedCharacters 리스트를 오른쪽으로 한 칸씩 이동하며며 순환 
        GameObject lastCharacter = spawnedCharacters[spawnedCharacters.Count - 1];
        spawnedCharacters.RemoveAt(spawnedCharacters.Count - 1);
        spawnedCharacters.Insert(0, lastCharacter);

        //변경된 리스트 순서에 따라 캐릭터들의 Transform 업데이트
        UpdateCharacterTransforms();
    }
}