using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambitController : MonoBehaviour
{
    public BattleController battleController;
    //load up the gambit array
    private Gambit[] gambitArray;
    private float percent;
    private BattleCharacter tempCharHolder;
    private BattleCharacter gambitTarget;

    void AI(BattleCharacter character)
    {
        tempCharHolder = character; //i could just pass in the character straight to the next class rather than store it as it's own variable, doing it this way so i can see current gambit running in inspector atm
        gambitArray = character.gambits;
    //to run:
        for (int i = 0; i < gambitArray.Length; i++)
        {
            if ( gambitArray[i].action != null) //gambitArray[i].target != null && gambitArray[i].condition != null &&
            {
                //now check the conditions with a case switch
                switch(gambitArray[i].condition){

                    case(conditionType.Health):
                        //now look for the higher/lower/etc.
                        GambitTargetter (i, 0);
                        break;  

                    case(conditionType.Mana):
                        GambitTargetter (i, 1);
                        break;  

                    case(conditionType.StatusEffect):
                        GambitTargetter (i, 2);
                        break;  

                    case(conditionType.Present):
                        GambitTargetter (i, 3);
                        break;  

                    default:
                        break;    
                }
                
            }
        }
        //fetch line i of gambits, for (i) gambitsList[i]
        //if line1 has all components X != null && Y != null, etc.
            //check the if condition (is there an enemy below 20%?, is there an ally below 40%?)
                //run target command
                //return and end turn if ran
            //if not then check next (i)
        //backup action if at last (i)
    }

    public void GambitTargetter (int line, int HM)
    {
        // if target is self then just pick self and skip ahead.
        if (gambitArray[line].target == targetType.Self)
        {
            //battle controller target = self
            battleController.target = tempCharHolder;
            //RUN NEXT BIT AND RETURN
            return;
        }
        List<BattleCharacter> tList = new List<BattleCharacter>();
        if (gambitArray[line].target == targetType.Ally)
            {
            tList = battleController.allyList;
            }
        if (gambitArray[line].target == targetType.Foe)
        {
            tList = battleController.enemyList;
        }
        //if health/mana
        if (HM <= 1)
        //if HM = 0, health, if HM = 1, mana
        {switch(gambitArray[line].conditionQ){

            case(conditionQualifier.Higher):
                //check for target in gambitArray[line].target with health higher than condition float
                for (int i = 0; i < tList.Count; i++)
                {
                if (HM == 0)    
                    percent = tList[i].currentHP/tList[i].maxHP;
                if (HM == 1)
                    percent = tList[i].currentMana/tList[i].maxMana;
                if (percent > gambitArray[line].conditionFloat) //battleController.allyList[i].currentHP/battleController.allyList[i].maxHP 
                    {
                        //set battle controller target to that person
                        gambitTarget = tList[i];
                        GambitAction(line);
                        return;
                    }
                }
                
                break;  

            case(conditionQualifier.Lower):
                for (int i = 0; i < tList.Count; i++)
                {
                if (HM == 0)    
                    percent = tList[i].currentHP/tList[i].maxHP;
                if (HM == 1)
                    percent = tList[i].currentMana/tList[i].maxMana;
                if (percent < gambitArray[line].conditionFloat) //battleController.allyList[i].currentHP/battleController.allyList[i].maxHP 
                    {
                        //set battle controller target to that person
                        gambitTarget = tList[i];
                        GambitAction(line);
                        return;
                    }
                }
                break;  

            case(conditionQualifier.Equal):
                for (int i = 0; i < tList.Count; i++)
                {
                if (HM == 0)    
                    percent = tList[i].currentHP/tList[i].maxHP;
                if (HM == 1)
                    percent = tList[i].currentMana/tList[i].maxMana;
                if (percent == gambitArray[line].conditionFloat) //battleController.allyList[i].currentHP/battleController.allyList[i].maxHP 
                    {
                        //set battle controller target to that person
                        gambitTarget = tList[i];
                        GambitAction(line);
                        return;
                    }
                }
                break;  

            case(conditionQualifier.Highest):
                int highest = 0;
                if (HM == 0) 
                {
                for (int i = 0; i < tList.Count; i++)
                {
                    highest = (int)Mathf.Max(highest, tList[i].currentHP);
                    if (highest == tList[i].currentHP)
                        gambitTarget = tList[i];
                }
                GambitAction(line);
                return;
                }        

                if (HM == 1) 
                {
                for (int i = 0; i < tList.Count; i++)
                {
                    highest = (int)Mathf.Max(highest, tList[i].currentMana);
                    if (highest == tList[i].currentMana)
                        gambitTarget = tList[i];
                }
                GambitAction(line);
                return;
                }            
                break;  

            case(conditionQualifier.Lowest):
                int lowest = 2147483647;
                if (HM == 0) 
                {
                for (int i = 0; i < tList.Count; i++)
                {
                    lowest = (int)Mathf.Min(lowest, tList[i].currentHP);
                    if (lowest == tList[i].currentHP)
                        gambitTarget = tList[i];
                }
                GambitAction(line);
                return;
                }        

                if (HM == 1) 
                {
                for (int i = 0; i < tList.Count; i++)
                {
                    lowest = (int)Mathf.Min(lowest, tList[i].currentMana);
                    if (lowest == tList[i].currentMana)
                        gambitTarget = tList[i];
                }
                GambitAction(line);
                return;
                }   
                break;  

            default:
                break;    

        }}


        //if status effect
        if (HM == 2)
        {
            //check Tlist for status effect 
            //for i in tList
            //for X in tList.statusEffectsList
            //if tList.statusEffectsList[X] = gambits status effect
            //thats the target, proceed
        }
        //if present
        if (HM == 3)
        {
            //check foe/ally/all depending on picked list
        }
    }

    private void GambitAction (int line)
    {
        //do action on target
        battleController.abilityChoice = gambitArray[line].action;
        //attack
        
        //cast
        StartCoroutine(battleController.Cast(gambitTarget));
        //defend
    }

}
