using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents; //using machine learniing functionality
using TMPro; //used to dispaly reward text
using System;

public class PenguinArea : Area {

    public PenguinAgent pAgent;
    public PenguinAgent pAgent2;

    public List<PenguinAgent> pengs;
    public GameObject pBaby;
    public Fish fPrefab;
    public TextMeshPro rewardText;


    [HideInInspector]
    public float fSpeed = 0f;
    [HideInInspector]
    public float fRadius = 1f;

    private List<GameObject> fList;


    public override void ResetArea() //area resets periodically
    {
        RemoveAllFIsh();
        placePenguin();
        placeBaby();
        spawnFish(7, fSpeed);
    }
    public void removeSpecificFish(GameObject fObject) //remove a fish from the list when the penguin eats it
    {
        fList.Remove(fObject);
        Destroy(fObject);
    }
    public static Vector3 chooseRandPos(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius) //Function used to calculate a random position in the radius of the penguin's enclosure 
    {
        float radius = minRadius;
        
        if (maxRadius > minRadius)
        {
            radius = UnityEngine.Random.Range(minRadius, maxRadius); //chooses a random radius
        }

        return center + Quaternion.Euler(0f, UnityEngine.Random.Range(minAngle, maxAngle), 0f) * Vector3.forward * radius;
    }

    private void RemoveAllFIsh() //Removes all fish from the area on reset
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

    private void placePenguin() //places the penguin randomly somewhere in the scene's radius  
    {

        for (int i = 0; i < pengs.Count; i++)
        {
            pengs[i].transform.position = chooseRandPos(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
            pengs[i].transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        }
    }

    private void placeBaby() //places the baby randomly somewhere in the scene's radius  
    {
        pBaby.transform.position = chooseRandPos(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        pBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void spawnFish(int count, float fSpeed) //spawns the chosen amount of fish
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

    private void Update() //Constantly outputs the penguins current reward
    {
        //rewardText.text = "peep"; //pAgent.GetCumulativeReward().ToString("0.00");
        string test ="";
        for (int i = 0; i < pengs.Count; i++)
        {
            test += "p" + i + ": " + pengs[i].GetCumulativeReward().ToString("0.00");
            test += "@";
        }
        test = test.Replace("@", System.Environment.NewLine);
        rewardText.text = test;
    }
}
