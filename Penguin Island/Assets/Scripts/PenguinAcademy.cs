using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PenguinAcademy : Academy {

    private PenguinArea[] pAreas;

    public override void AcademyReset()
    {
        if (pAreas == null)
        {
            pAreas = FindObjectsOfType<PenguinArea>();
        }

        foreach (PenguinArea pArea in pAreas)
        {
            pArea.fSpeed = resetParameters["fish_speed"];
            pArea.fRadius = resetParameters["feed_radius"];
            pArea.ResetArea();
        }
    }
}
