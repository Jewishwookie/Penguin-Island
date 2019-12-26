using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;
using System;

public class PenguinArea : Area {
    public PenguinAgent pAgent;
    public GameObject pBaby;
    public Fish fPrefab;
    public TextMeshPro rewardText;


    [HideInInspector]
    public float fSpeed = 0f;
    [HideInInspector]
    public float fRadius = 1f;

    private List<GameObject> fList;


    public override void ResetArea()
    {
        RemoveAllFIsh();
        placePenguin();
        placeBaby();
        spawnFish(4, fSpeed);
    }
    public void removeSpecificFish(GameObject fObject)
    {
        fList.Remove(fObject);
        Destroy(fObject);
    }
    public static Vector3 chooseRandPos(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        
        if (maxRadius > minRadius)
        {
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        }

        return center + Quaternion.Euler(0f, UnityEngine.Random.Range(minAngle, maxAngle), 0f) * Vector3.forward * radius;
    }

    private void RemoveAllFIsh()
    {
        if(fList != null)
        {
            for (int i = 0; i < fList.Count; i++)
            {
                if (fList[i] != null)
                    Destroy(fList[i]);
            }
        }
        fList = new List<GameObject>();
    }

    private void placePenguin()
    {
        pAgent.transform.position = chooseRandPos(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        pAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

    }

    private void placeBaby()
    {
        pBaby.transform.position = chooseRandPos(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        pBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void spawnFish(int count, float fSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject fObject = Instantiate<GameObject>(fPrefab.gameObject);
            fObject.transform.position = chooseRandPos(transform.position, 100, 260f, 2f, 13f) + Vector3.up * .5f;
            fObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            fObject.transform.parent = transform;
            fList.Add(fObject);
            fObject.GetComponent<Fish>().fSpeed = fSpeed;
        }
    }

    private void Update()
    {
        rewardText.text = pAgent.GetCumulativeReward().ToString("0.00");
    }
}
