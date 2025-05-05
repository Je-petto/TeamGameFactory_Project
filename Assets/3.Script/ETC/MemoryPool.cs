

using System.Collections.Generic;
using System.Security;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class MemoryPool
{
    private class PoolItem
    {
        // Object 활성화/비활성화
        public bool isActive;
        // 실제 오브젝트
        public GameObject gameObject;
    }
    // Object가 부족할 때 추가 생성할 Object 갯수
    private int increaseCount = 5;
    // 현재 등록되어 있는 Object 갯수
    private int maxCount;
    // 현재 활성화되어 있는 Object 갯수
    private int activeCount;

    // Object Pooling에서 관리하는 GameObject Prefabs
    private GameObject poolObject;
    // 관라하는 Object를 저장하는 자료구조
    private List<PoolItem> poolList;

    // 외부로 사용되는 데이터
    public int MaxCount => maxCount;
    public int ActiveCount => activeCount;

    // 생성자 메서드
    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;

        this.poolObject = poolObject;

        poolList = new List<PoolItem>();
        InstantiateObject();
    }

    // 오브젝트 생성 메서드
    public void InstantiateObject()
    {
        maxCount += increaseCount;
        for (int i = 0; i < increaseCount; i++)
        {
            PoolItem item = new PoolItem();
            item.isActive = false;
            item.gameObject = GameObject.Instantiate(poolObject);
            item.gameObject.SetActive(true);

            poolList.Add(item);
        }
    }

    // 현재 관리 중인 모든 오브젝트 삭제 메서드
    public void DestroyObject()
    {
        if (poolList == null) return;

        foreach(var p in poolList)
            GameObject.Destroy(p.gameObject);
        poolList.Clear();
    }

    // 현재 비활성화 상태인 오브젝트 중 하나를 활성화로 만들어 사용
    // 만약 비활성화 중인 오브젝트가 없다면 새로 생성
    public GameObject ActivePoolItem()
    {
        if (poolList == null) return null;

        if (maxCount.Equals(ActiveCount))
            InstantiateObject();
        foreach (var p in poolList)
        {
            if (!p.isActive)
            {
                activeCount++;

                p.isActive = true;
                p.gameObject.SetActive(true);

                return p.gameObject;
            }
        }
        return null;
    }

    // 사용이 끝난 오브젝트를 다시 비활성화 상태로 전환
    public void DeactivatePoolItem(GameObject o)
    {
        if (poolList == null && o == null) return;

        foreach(var p in poolList)
        {
            if (p.gameObject == o)
            {
                activeCount--;
                
                p.isActive = false;
                p.gameObject.SetActive(false);

                return;
            }
        }
    }

    // 게임의 사용 중인 모든 오브젝트를 비활성화
    public void DeactivateAllPoolItem()
    {
        if (poolList == null) return;

        foreach (var p in poolList)
        {
            if (p.gameObject != null && p.isActive)
            {
                p.isActive = false;
                p.gameObject.SetActive(false);
            }
        }
        activeCount = 0;
    }
}
