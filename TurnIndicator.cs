using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TurnIndicator : MonoBehaviour
{
    [SerializeField] private BattleGUI battleUI;
    [SerializeField] private GameObject timeline;
    [SerializeField] private GameObject topFrame;
    [SerializeField] private GameObject bottomFrame;
    // Start is called before the first frame update
    private List<GameObject> turnFrames = new List<GameObject>();
    [SerializeField] private float frameMoveSpd;
    [SerializeField] private CurrentTurnFrame currentTurnFrame;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateTurnFrames()
    {
        //get the length of the thing
        RectTransform rt = (RectTransform)timeline.transform;
        float realWidth = rt.rect.width;
        //Debug.Log(realWidth);
        //get longest turn, we then know our realWidth represents the range of 0 to longest turn (or an average of turns would work better)
        float longestTurn = 0;
        for (int i = 0; i < battleUI.allyListAlive.Count; i++)
        {
            if (battleUI.allyListAlive[i].actSpeed > longestTurn)
                {
                    longestTurn = battleUI.allyListAlive[i].actSpeed;
                }
        }
        for (int i = 0; i < battleUI.enemyListAlive.Count; i++)
        {
            if (battleUI.enemyListAlive[i].actSpeed > longestTurn)
                {
                    longestTurn = battleUI.enemyListAlive[i].actSpeed;
                }            
        }     

        //instantiate a top frame for allies with X position of % across the total timeline
        //that X position will be realwidth/longestturn * nextturn for each unit
    //     for (int i = 0; i < battleUI.allyListAlive.Count; i++)
    //     {
    //         //go through all of them (for)
    //         //if one matches then set create bool to false
    //         bool create = true; // the default is to create
    //         if (turnFrames.Count > 0)
    //         {for (int x = 0; x < turnFrames.Count; x++)                                                                            //check the existing frames
    //         {  
    //             TurnFrame tfx = turnFrames[x].GetComponent<TurnFrame>();
    //             if ((tfx.turnNumber == tfx.assignedCharacter.turnNumber) && (battleUI.allyListAlive[i] == tfx.assignedCharacter)) //if one exists already for this character
    //                 {
    //                     create = false;
    //                 }       //then don't create new
    //         }}
    //         if (create == true)
    //         {
    //         float frameXCoords = ((realWidth/longestTurn)*battleUI.allyListAlive[i].nextTurn) - (float)(0.5*realWidth);
    //         Vector3 framepos = timeline.transform.position + new Vector3(frameXCoords, 0);
    //         GameObject newGO = Instantiate<GameObject>(topFrame, framepos, Quaternion.identity, timeline.GetComponent<Transform>());
    //         //Debug.Log("instantiated turn frame on timeline");
    //         TurnFrame gotf = newGO.GetComponent<TurnFrame>();
    //         gotf.assignedCharacter = battleUI.allyListAlive[i];
    //         gotf.turnNumber = battleUI.allyListAlive[i].turnNumber;
    //         turnFrames.Add(newGO);
    //         //Debug.Log(turnFrames.Count);

    //         // get photo for battleUI.allyListAlive[i]                                                                              <--------------------
    //         }

    //         //if allies next turn (current turn timer + default turn timer) should also show, put it on there                       <--------------------

    //     }


    //     for (int i = 0; i < battleUI.enemyListAlive.Count; i++)
    //     {
            
    //         //go through all of them (for)
    //         //if one matches then set create bool to false
    //         bool create = true; // the default is to create
    //         if (turnFrames.Count > 0)
    //         {for (int x = 0; x < turnFrames.Count; x++)
    //         {  
    //             TurnFrame tfx = turnFrames[x].GetComponent<TurnFrame>();
    //             if ((tfx.turnNumber == tfx.assignedCharacter.turnNumber) && (battleUI.enemyListAlive[i] == tfx.assignedCharacter)) //if one exists already for this character
    //                 {
    //                     create = false;
    //                 }       //then don't create new
    //         }}
    //         if (create == true)
    //         {
    //         float frameXCoords = ((realWidth/longestTurn)*battleUI.enemyListAlive[i].nextTurn) - (float)(0.5*realWidth);
    //         Vector3 framepos = timeline.transform.position + new Vector3(frameXCoords, 0);
    //         GameObject newGO = Instantiate<GameObject>(bottomFrame, framepos, Quaternion.identity, timeline.GetComponent<Transform>());
    //         //Debug.Log("instantiated turn frame on timeline");
    //         TurnFrame gotf = newGO.GetComponent<TurnFrame>();
    //         gotf.assignedCharacter = battleUI.enemyListAlive[i];
    //         gotf.turnNumber = battleUI.enemyListAlive[i].turnNumber;
    //         turnFrames.Add(newGO);
    //         //Debug.Log(turnFrames.Count);

    //         // get photo for battleUI.enemyListAlive[i]                                                                              <--------------------
    //         }
            
    //     }
        
    // //REPEAT FOR TURN FRAME 2  - THIS WHOLE SECTION CAN BE CONDENSED BY MAKING IF ALLY SPAWN TOP PREFAB, IF ENEMY SPAWN BOT PREFAB, THEN YOU CAN USE THE ALL LIST

    //     for (int i = 0; i < battleUI.allyListAlive.Count; i++)
    //     {
    //         //go through all of them (for)
    //         //if one matches then set create bool to false
    //         bool create = true; // the default is to create
    //         if (turnFrames.Count > 0)
    //         {for (int x = 0; x < turnFrames.Count; x++)                                                                            //check the existing frames
    //         {  
    //             TurnFrame tfx = turnFrames[x].GetComponent<TurnFrame>();
    //             if ((tfx.turnNumber == tfx.assignedCharacter.turnNumber + 1) && (battleUI.allyListAlive[i] == tfx.assignedCharacter)) //if one exists already for this character
    //                 {
    //                     create = false;
    //                 }       //then don't create new
    //         }}
    //         if (create == true && battleUI.allyListAlive[i].nextTurn + battleUI.allyListAlive[i].actSpeed < longestTurn)
    //         {
    //         float frameXCoords = ((realWidth/longestTurn)*battleUI.allyListAlive[i].nextTurn + battleUI.allyListAlive[i].actSpeed) - (float)(0.5*realWidth);
    //         Vector3 framepos = timeline.transform.position + new Vector3(frameXCoords, 0);
    //         GameObject newGO = Instantiate<GameObject>(topFrame, framepos, Quaternion.identity, timeline.GetComponent<Transform>());
    //         //Debug.Log("instantiated turn frame on timeline");
    //         TurnFrame gotf = newGO.GetComponent<TurnFrame>();
    //         gotf.assignedCharacter = battleUI.allyListAlive[i];
    //         gotf.turnNumber = battleUI.allyListAlive[i].turnNumber + 1;
    //         turnFrames.Add(newGO);
    //         //Debug.Log(turnFrames.Count);

    //         // get photo for battleUI.allyListAlive[i]                                                                              <--------------------
    //         }

    //         //if allies next turn (current turn timer + default turn timer) should also show, put it on there                       <--------------------

    //     }

    //     for (int i = 0; i < battleUI.enemyListAlive.Count; i++)
    //     {
            
    //         //go through all of them (for)
    //         //if one matches then set create bool to false
    //         bool create = true; // the default is to create
    //         if (turnFrames.Count > 0)
    //         {for (int x = 0; x < turnFrames.Count; x++)
    //         {  
    //             TurnFrame tfx = turnFrames[x].GetComponent<TurnFrame>();
    //             if ((tfx.turnNumber == (tfx.assignedCharacter.turnNumber +1)) && (battleUI.enemyListAlive[i] == tfx.assignedCharacter)) //if one exists already for this character
    //                 {
    //                     create = false;
    //                 }       //then don't create new
    //         }}
    //         if (create == true && battleUI.enemyListAlive[i].nextTurn + battleUI.enemyListAlive[i].actSpeed < longestTurn)
    //         {
    //         float frameXCoords = ((realWidth/longestTurn)*(battleUI.enemyListAlive[i].nextTurn + battleUI.enemyListAlive[i].actSpeed)) - (float)(0.5*realWidth);
    //         Vector3 framepos = timeline.transform.position + new Vector3(frameXCoords, 0);
    //         GameObject newGO = Instantiate<GameObject>(bottomFrame, framepos, Quaternion.identity, timeline.GetComponent<Transform>());
    //         //Debug.Log("instantiated turn frame on timeline");
    //         TurnFrame gotf = newGO.GetComponent<TurnFrame>();
    //         gotf.assignedCharacter = battleUI.enemyListAlive[i];
    //         gotf.turnNumber = battleUI.enemyListAlive[i].turnNumber + 1;
    //         turnFrames.Add(newGO);
    //         //Debug.Log(turnFrames.Count);

    //         // get photo for battleUI.enemyListAlive[i]                                                                              <--------------------
    //         }
            
    //     }

        List<BattleCharacter> aliveAll = new List<BattleCharacter>();
        aliveAll = battleUI.allyListAlive.Union<BattleCharacter>(battleUI.enemyListAlive).ToList<BattleCharacter>();

        for (int z = 0; z < 3; z++)
        {
        for (int i = 0; i < aliveAll.Count; i++)
        {
            
            //go through all of them (for)
            //if one matches then set create bool to false
            bool create = true; // the default is to create
            if (turnFrames.Count > 0) //if there are already turn frames
            {for (int x = 0; x < turnFrames.Count; x++) //check em
            {  
                TurnFrame tfx = turnFrames[x].GetComponent<TurnFrame>();
                if ((tfx.turnNumber == (tfx.assignedCharacter.turnNumber +z)) && (aliveAll[i] == tfx.assignedCharacter)) //if one exists already for this character
                    {
                        create = false;
                    }       //then don't create new
            }}
            if (create == true && aliveAll[i].nextTurn + (z*aliveAll[i].actSpeed) <= longestTurn)
            {
            Debug.Log("smaller than longest turn: " + aliveAll[i].nameSO);
            float frameXCoords = ((realWidth/longestTurn)*(aliveAll[i].nextTurn + (aliveAll[i].actSpeed*z))) - (float)(0.5*realWidth);
            Vector3 framepos = timeline.transform.position + new Vector3(frameXCoords, 0);
            //if enemy spawn bottom frame
            if (aliveAll[i].enemyOrAlly == BattleCharacter.enemyAlly.Enemy)
            {
            GameObject newGO = Instantiate<GameObject>(bottomFrame, framepos, Quaternion.identity, timeline.GetComponent<Transform>());
                        //Debug.Log("instantiated turn frame on timeline");
            TurnFrame gotf = newGO.GetComponent<TurnFrame>();
            gotf.assignedCharacter = aliveAll[i];
            gotf.turnNumber = aliveAll[i].turnNumber + z;
            turnFrames.Add(newGO);
            }
            //if ally spawn top frame
            if (aliveAll[i].enemyOrAlly == BattleCharacter.enemyAlly.Ally)
            {
            GameObject newGO = Instantiate<GameObject>(topFrame, framepos, Quaternion.identity, timeline.GetComponent<Transform>());
                        //Debug.Log("instantiated turn frame on timeline");
            TurnFrame gotf = newGO.GetComponent<TurnFrame>();
            gotf.assignedCharacter = aliveAll[i];
            gotf.turnNumber = aliveAll[i].turnNumber + z;
            turnFrames.Add(newGO);
            }

            //Debug.Log(turnFrames.Count);

            // get photo for battleUI.enemyListAlive[i]                                                                              <--------------------
            }
            
        }}
        // seperate it into generate turnframes and animate frames, run in sequence "animate - generate" on the battlecontroller calculate turns script
    }
    

    public void UpdateTurnFrames()
    {
        RectTransform rt = (RectTransform)timeline.transform;
        float realWidth = rt.rect.width;
        //Debug.Log(realWidth);
        //get longest turn, we then know our realWidth represents the range of 0 to longest turn (or an average of turns would work better)
        float longestTurn = 0;
        for (int i = 0; i < battleUI.allyListAlive.Count; i++)
        {
            if (battleUI.allyListAlive[i].actSpeed > longestTurn)
                {
                    longestTurn = battleUI.allyListAlive[i].actSpeed;
                }
        }
        for (int i = 0; i < battleUI.enemyListAlive.Count; i++)
        {
            if (battleUI.enemyListAlive[i].actSpeed > longestTurn)
                {
                    longestTurn = battleUI.enemyListAlive[i].actSpeed;
                }            
        }   
        //new list that's a combination of all live units
        List<BattleCharacter> aliveAll = new List<BattleCharacter>();
        aliveAll = battleUI.allyListAlive.Union<BattleCharacter>(battleUI.enemyListAlive).ToList<BattleCharacter>();
        for (int i = 0; i < aliveAll.Count; i++) //go through each alive unit
        {
            for (int x = 0; x < turnFrames.Count; x++) //go through each turn frame
            {
                TurnFrame tfx = turnFrames[x].GetComponent<TurnFrame>();
                if ((tfx.turnNumber < tfx.assignedCharacter.turnNumber) && (aliveAll[i] == tfx.assignedCharacter) && (tfx.assignedCharacter != battleUI.battleController.characterTurn)) //if this turn has already gone
                { 
                    Destroy(turnFrames[x]); //destroy gameobject
                    turnFrames.Remove(turnFrames[x]); //remove from list
                }
                if ((tfx.turnNumber == tfx.assignedCharacter.turnNumber) && (aliveAll[i] == tfx.assignedCharacter)) //if we're on the right character and turn
                {
                    //Debug.Log(aliveAll[i].nextTurn);
                    float frameXCoords = ((realWidth/longestTurn)*aliveAll[i].nextTurn) - (float)(0.5*realWidth);
                    //Debug.Log(frameXCoords);
                    Vector3 framepos = timeline.transform.position + new Vector3(frameXCoords, 0);
                    //lerp move
                    //Debug.Log("starting LERP");
                    tfx.framePos = framepos;
                    tfx.frameMoveSpd = frameMoveSpd;
                    

                }
            }

        }
        List<BattleCharacter> deadAll = new List<BattleCharacter>();
        deadAll = battleUI.allyListDead.Union<BattleCharacter>(battleUI.enemyListDead).ToList<BattleCharacter>();
        for (int i = 0; i < deadAll.Count; i++) //go through each dead unit
        {
            for (int x = 0; x < turnFrames.Count; x++) //go through each turn frame
            {
                TurnFrame tfx = turnFrames[x].GetComponent<TurnFrame>();
                Destroy(turnFrames[x]); //destroy gameobject
                turnFrames.Remove(turnFrames[x]); //remove from list
            }
        }

        //if this mofo dead, grey out or destroy gameobject
        currentTurnFrame.turnIconChange(battleUI.battleController.characterTurn); //changes the current turn frame

        //and also add the additional jobbies
        
    }
}
