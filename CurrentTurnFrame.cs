using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTurnFrame : MonoBehaviour
{
    public BattleCharacter assignedCharacter;
    public Image portrait;


    // Update is called once per frame


    public void turnIconChange(BattleCharacter newTarget)
    {
        Debug.Log("running turniconchange");
        //rotate 90 degrees
        //this.transform.Rotate(new Vector3(90, 0, 0), Space.World);                DOESNT WORK
        assignedCharacter = newTarget;
        //this.transform.Rotate(new Vector3(-90, 0, 0), Space.World);
        //rotate 90 degrees
        if (assignedCharacter.icon != null)
            {portrait.sprite = assignedCharacter.icon;}
    }
}
