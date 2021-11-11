using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnFrame : MonoBehaviour
{
    public BattleCharacter assignedCharacter;
    public int turnNumber;
    public Vector3 framePos;
    public float frameMoveSpd = 3;
    public Image portrait;
    

    // Start is called before the first frame update
    void Start()
    {
        framePos = transform.position;
        if (assignedCharacter.icon != null)
            {portrait.sprite = assignedCharacter.icon;}
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, framePos, frameMoveSpd * Time.deltaTime);
    }
}
