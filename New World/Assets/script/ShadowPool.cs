using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject shadowPrefab;

    public int ShadowCount;

    private Queue<GameObject> availabeObjects = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;

        // initiate
    }

    public void FillPool()
    {
        for (int i = 0; i<ShadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);

            // 取消启用，返回对象池
            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availabeObjects.Enqueue(gameObject);
    }

    public GameObject GetFromPool()
    {
        if (availabeObjects.Count ==0)
        {
            FillPool();
        }
        var outShadow = availabeObjects.Dequeue();

        outShadow.SetActive(true);

        return outShadow;
    }
}
