using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

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
        else if (vectorAction[2] == 2f)
        {
            leftOrRight = 1f;
        }

        //animator parameters

        animator.SetFloat("Vertical", forward);
        animator.SetFloat("Horizontal", leftOrRight);
    }
}
