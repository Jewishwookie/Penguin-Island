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
        throw new NotImplementedException();
    }

    private void placeBaby()
    {
        throw new NotImplementedException();
    }

    private void spawnFish(int v, float fSpeed)
    {
        throw new NotImplementedException();
    }
}
