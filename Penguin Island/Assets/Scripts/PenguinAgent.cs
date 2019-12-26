using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class PenguinAgent : Agent {

    public GameObject heartPrefab;
    public GameObject regurgFishPrefab;

    private PenguinArea pArea;
    private Animator animator;
    private RayPerception3D rayPerception;
    private GameObject baby;

    private bool fullTummy;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //converts actions to axis values
        float forward = vectorAction[0];
        float leftOrRight = 0f;
        if(vectorAction[1] == 1f)
        {
            leftOrRight = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            leftOrRight = 1f;
        }

        //animator parameters
        animator.SetFloat("Vertical", forward);
        animator.SetFloat("Horizontal", leftOrRight);

        //reward every step
        AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        fullTummy = false;
        pArea.ResetArea();
    }
    public override void CollectObservations()
    {
        //Penguin eaten?
        AddVectorObs(fullTummy);

        //distance to baby
        AddVectorObs(Vector3.Distance(baby.transform.position, transform.position));
        //direction to baby
        AddVectorObs((baby.transform.position - transform.position).normalized);

        //direction penguin is facing
        AddVectorObs(transform.forward);

        //RayPerception (sight)
        float rayDistance = 20f;
        float[] rayAngles = { 30f, 60f, 90f, 120f, 150f };
        string[] detectableObjects = { "baby", "fish", "wall" };

        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects,0f, 0f));
    }

    public override float[] Heuristic()
    {
        float[] playerInput = { 0f, 0f };

        if (Input.GetKey(KeyCode.W))
        {
            playerInput[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerInput[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerInput[1] = 2;
        }
        return playerInput;
    }

    private void Start()
    {
        pArea = GetComponentInParent<PenguinArea>();
        baby = pArea.pBaby;
        animator = GetComponent<Animator>();
        rayPerception = GetComponent<RayPerception3D>();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position,baby.transform.position) < pArea.fRadius)
        {
            //close enough to try to feed
            RegurgitateFish();

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("fish"))
        {
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag("baby"))
        {
            RegurgitateFish();
        }
    }

    private void EatFish(GameObject fishObject)
    {
        if (fullTummy) return;
        fullTummy = true;

        pArea.removeSpecificFish(fishObject);
        AddReward(1f);
    }

    private void RegurgitateFish()
    {
        if (!fullTummy) return;
        fullTummy = false;

        //generate regurgitated fish
        GameObject regurgitatedFish = Instantiate<GameObject>(regurgFishPrefab);
        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.position = baby.transform.position;
        Destroy(regurgitatedFish, 4f);

        //spawn heart
        GameObject heart = Instantiate<GameObject>(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.position = baby.transform.position + Vector3.up;
        Destroy(heart, 4f);

        AddReward(1f);
    }
}
