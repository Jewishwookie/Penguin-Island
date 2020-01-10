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
    private List<GameObject> babyList;

    private bool fullTummy;

    public override void AgentAction(float[] vectorAction, string textAction) //gets called everytime the agent makes a decision
    {                                                                         //only actions it can make is move forward or not, or turn left or right //actions come in the form of an array of floats
        //converts actions to axis values that will control the agent
        float forward = vectorAction[0]; //looks at first value in vectoraction array 
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
        //animator controls the penguin using loop motion
        //gets passed into a blend tree
        animator.SetFloat("Vertical", forward); 
        animator.SetFloat("Horizontal", leftOrRight);

        //deducts reward for every step a penguin does nothing
        AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        //sets penguins stomach back to empty and resets penguin environment
        fullTummy = false;
        pArea.ResetArea();
    }
    public override void CollectObservations() //function from mlagents - the agent uses this to perceive its environment
    {
        //addobs gives gives the observer information everytime this function is called

        //Penguin eaten?
        AddVectorObs(fullTummy);

        //distance to baby
        AddVectorObs(Vector3.Distance(baby.transform.position, transform.position));
        //direction to baby
        AddVectorObs((baby.transform.position - transform.position).normalized);

        //direction penguin is facing
        AddVectorObs(transform.forward);

        //RayPerception (sight)
        float rayDistance = 20f; //how far to cast the ray
        float[] rayAngles = { 30f, 60f, 90f, 120f, 150f }; //angles to cast the rays
        string[] detectableObjects = { "baby", "fish", "wall" }; //tags that will correspond when raycast detects them

        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects,0f, 0f));
    }

    public override float[] Heuristic() //base class function enables player control
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
        //initiates essential variables
        pArea = GetComponentInParent<PenguinArea>();
        baby = pArea.pBaby;
        animator = GetComponent<Animator>();
        rayPerception = GetComponent<RayPerception3D>();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, baby.transform.position) < pArea.fRadius) //checks if penguin is in range of the baby
        {
            //close enough to try to feed
            RegurgitateFish();

        }
    }
    private void OnCollisionEnter(Collision collision) //makes sure penguin connects with the baby
    {
        if(collision.transform.CompareTag("fish"))
            EatFish(collision.gameObject);

        else if (collision.transform.CompareTag("baby"))
            RegurgitateFish();

    }

    private void EatFish(GameObject fishObject)
    {
        //sets penguin's stomach to full until he feeds baby
        if (fullTummy) return;
        fullTummy = true;

        //removes eaten fish and gives the agent a reward for eating the fish
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

        //baby.transform.localScale += new Vector3(1.3f,0.5f,1);
        
        AddReward(1f);
    }
}
