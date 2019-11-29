using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {

    public float fSpeed;

    private float randomisedSpeed = 0f;
    private float nextActionTime = -1f;
    private Vector3 targetPosition;

    private void FixedUpdate()
    {
        if (fSpeed > 0)
        {
            Swim();
        }
    }

    private void Swim()
    {
        if(Time.fixedTime >= nextActionTime)
        {
            //Randomise speed
            randomisedSpeed = fSpeed * UnityEngine.Random.Range(.5f, 1.5f);

            //pick random target
            targetPosition = PenguinArea.chooseRandPos(transform.parent.position, 100f, 260f, 2f, 13f);

            //rotate toward the target
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
        }
    }
}
