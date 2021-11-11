using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BattleController : MonoBehaviour
{
    [SerializeField] private BattleGUI battleUI;
    public static int dungeonDifficulty;
    [SerializeField] private DungeonTheme theme;   
    [SerializeField] private GameObject combatText;   
    [SerializeField] private List<TeamCharacterSO> characterReferences = new List<TeamCharacterSO>(); 
    
    private enum battleStates{ 
        Start,
        CheckTurn,
        StatusEffects,
        PlayerChoice,
        EnemyChoice,
        Lose,
        Win
    };
    public enum actionChoice{ 
        UI,
        Choosing,
        Attack,
        Cast,
        Defend
    };
    [Header("Turn details")]
    [SerializeField] private battleStates currentState;
    public actionChoice currentAction;
    public AbilitySO abilityChoice;
    public bool waiting = false;
    private bool waitingTurnCalc = false;
    [Header("Character details")]
    public BattleCharacter characterTurn;
    public BattleCharacter target;
    public List<BattleCharacter> allyList = new List<BattleCharacter>();
    public List<BattleCharacter> enemyList = new List<BattleCharacter>();
    private BattleCharacter eA;

    
    [SerializeField] public List<Transform> allySpawns = new List<Transform>(); //these are just transforms not gameobjects! <-
    [SerializeField] public List<Transform> enemySpawns = new List<Transform>();

//    private bool playersSpawned = false;  might need something like this to prevent characters respawning on next fight


    //private GameObject currentUnitsTurn; //for the gameobject of the current character, probably better to access it through characterTurn.theirGO
    // Start is called before the first frame update
    void Start()
    {
        //pullplayers
        for (int i = 0; i < characterReferences.Count; i++)
        {
            BattleCharacter pA = ScriptableObject.CreateInstance<BattleCharacter>();
            pA.PullPlayer(characterReferences[i]);
            allyList.Add(pA);
            
        }
        
        currentState = battleStates.Start;
        //set characters in order
    }


    // Update is called once per frame
    void Update()
    {


        //Debug.Log(currentState);
        switch(currentState){ //instead of using a switch i could just have everything flow and use bools to check where to progress to next, seems this works well tho

        case (battleStates.Start):
            //Set up battle -> prepare battle script
            //generate enemies
            GenEnemies(false);
            //set turns to default
            Debug.Log(allyList.Count + " " + enemyList.Count);

            SpawnEnemies();

            target = enemyList[0];
            battleUI.BuildTargetLists();
            currentState = battleStates.CheckTurn; //progress to calculate next turn
            break;


        case (battleStates.CheckTurn):
            if (waitingTurnCalc == false && waiting == false) //so it doesn't double up on turn calcs
            {   waitingTurnCalc = true;
                Debug.Log("calculating Turn");
                CheckWinLoss();
                CalculateTurn();}
            
            if (characterTurn.enemyOrAlly == BattleCharacter.enemyAlly.Enemy)  // could get rid of this variable and just see if they exist in the ally or enemy list
                {
                    currentState = battleStates.EnemyChoice;
                }
            if (characterTurn.enemyOrAlly == BattleCharacter.enemyAlly.Ally)
                {
                    currentAction = actionChoice.UI;  
                    currentState = battleStates.PlayerChoice;
                }              
            break;       


        case (battleStates.PlayerChoice):
            //go to UI 
            switch(currentAction){
            case(actionChoice.UI):
                battleUI.BuildSpellList(characterTurn.abilities);
                battleUI.PlayerTurn();
                currentAction = actionChoice.Choosing;
                Debug.Log("player turn started");  
                break;
            case(actionChoice.Attack):
                if ((battleUI.targetGO.activeSelf != true) && (waiting == false))
                    {
                        waiting = true;
                        Debug.Log("Starting attack routine on " + target);
                        StartCoroutine(PlayerAttack(target));
                    }
                //case(currentAction.Cast):
                break;
            case(actionChoice.Cast):
                if ((battleUI.targetGO.activeSelf != true) && (waiting == false)) //if not currently picking a target and not waiting for another action to complete
                    {
                        waiting = true;
                        Debug.Log("Starting cast routine on" + target);
                        StartCoroutine(Cast(target));
                    }
                //case(currentAction.Cast):
                break;
            case(actionChoice.Defend):
                if ((battleUI.targetGO.activeSelf != true) && (waiting == false)) //if not currently picking a target and not waiting for another action to complete
                    {
                        waiting = true;
                        StartCoroutine(Defend());
                    }
                break;
            }
            break;


        case (battleStates.EnemyChoice):
            //calculate enemies turn
            if (waiting == false)
            {   waiting = true;
                StartCoroutine(EnemyTurnAI());}
            //add a brief delay to drag it out a bit
            break;

            
        default:
            break;
        }
    }

    private void StartBattle(bool isBoss)
    {
        //load in mobs GenEnemies
        //all character next turn timer set to default
        //for
        //pick turn
        //currentState = battleStates.CheckTurn;
    }
    private void CalculateTurn()
    {
        //find shortest next turn duration out of pA/pB/pC/eA/eB/eC
        //float lowestTimer = Mathf.Min(pA.nextTurn, pB.nextTurn, pC.nextTurn, eA.nextTurn, eB.nextTurn, eC.nextTurn);
        battleUI.BuildTargetLists();
        List<BattleCharacter> dicTurns = new List<BattleCharacter>(); // new list of all characters
        dicTurns = battleUI.allyListAlive.Union<BattleCharacter>(battleUI.enemyListAlive).ToList<BattleCharacter>(); //add ally and enemy list to that new all list
        Debug.Log("calculating turn");

        float value = dicTurns[0].nextTurn + 1; //define and reset value from last usage so it's definitely above the lowest turn

        for(int i = 0; i < dicTurns.Count; i++)
        {
            if (dicTurns[i].nextTurn < value)
            {
                value = dicTurns[i].nextTurn;
                Debug.Log(value + "is the lowest next turn value on row" + i);
            }
            // float Rul = dicTurns[i].MinValue();
            // if (Rul == dicTurns[i])
            //     Debug.Log(Rul);            
        }
        for(int i = 0; i < dicTurns.Count; i++)
        {
            dicTurns[i].nextTurn = dicTurns[i].nextTurn - value; //remove remaining duration of shortest from them
            Debug.Log(dicTurns[i].nameSO + "'s next turn value set to" +  dicTurns[i].nextTurn);
        }
        for(int i = 0; i < dicTurns.Count; i++)
        {
            if (dicTurns[i].nextTurn == 0)
            {
                characterTurn = dicTurns[i]; 
                characterTurn.nextTurn = characterTurn.actSpeed;
                Debug.Log(characterTurn.nameSO + "'s turn");
                battleUI.turnInd.UpdateTurnFrames();
                battleUI.turnInd.CreateTurnFrames();
                waitingTurnCalc = false;
                return;  //get out before we accidentally skip a turn when two values are equal
            }
        }
        //reset chosen to default length
        
        //update the UI
        //start the turn by changing battlestate

    }
    
    private void GenEnemies(bool isBoss)
    {
        //clear the enemies list
        enemyList.Clear();
        if(dungeonDifficulty <= 0)
            dungeonDifficulty = 1;
        Debug.Log("Dungeon Difficulty: " + dungeonDifficulty);
        float modifier = (float)(dungeonDifficulty*0.5);
        float rFactor = (Random.Range(0.8f, 1.2f));
        Debug.Log("Modifier: " + modifier);
        //could use an enemies to spawn variable and count down through it
        //use dungeon difficulty rating to spawn enemies with stats blah blah blah
        //add weightings to certain mobs later on down the line
        //pick a template from the dungeon theme template list

        

        int noToSpawn;
        noToSpawn = 3;
        Debug.Log("starting enemy generation");
        for (int i = 0; i < noToSpawn; i++)
        {
            EnemyTemplateSO temp = theme.enemyTheme[Random.Range(0,theme.enemyTheme.Length)];     

            if ((isBoss == true) && (i == noToSpawn-1))
            {
                //times all stats by 6, add spells, add tags, etc.
                modifier = (modifier*6); 
            }
        //shell scriptable object
            eA = ScriptableObject.CreateInstance<BattleCharacter>();
            eA.nameSO = temp.enemyName;
            eA.alive = true;    
            eA.enemyOrAlly = BattleCharacter.enemyAlly.Enemy;
            eA.charPrefab = temp.enemyPrefab; //this'll hold the sprite and animations n shit
        //stats
            eA.maxHP = (int)((temp.hpWeight*100*modifier)*(Random.Range(0.8f, 1.2f)));
            Debug.Log("HP weighting" + temp.hpWeight + "modifier:" + modifier);
            eA.currentHP = eA.maxHP;
            eA.maxMana = (int)(temp.manaWeight*100*modifier*(Random.Range(0.8f, 1.2f)));
            eA.currentMana = eA.maxMana;
            eA.str = (int)(temp.strWeight*10*modifier*(Random.Range(0.8f, 1.2f)));
            eA.intelligence = (int)(temp.intWeight*10*modifier*(Random.Range(0.8f, 1.2f)));
            eA.spr = (int)(temp.sprWeight*10*modifier*(Random.Range(0.8f, 1.2f)));
            eA.def = (int)(temp.defWeight*10*modifier*(Random.Range(0.8f, 1.2f)));   
            eA.mDef = (int)(temp.mDefWeight*10*modifier*(Random.Range(0.8f, 1.2f)));
            eA.spd = (int)(temp.spdWeight*10*modifier*(Random.Range(0.8f, 1.2f)));
            eA.turnNumber = 0;
        //give em skills
        //give em a chance to spawn with tags
        //now add them to the list!
            enemyList.Add(eA);
            Debug.Log("Enemy generated " + i);
        }

    }
    private void SpawnEnemies()
    {
            for (int i = 0; i < allyList.Count; i++)
            {
                Debug.Log("spawned: " + allyList[i].nameSO);
                if (allyList[i].charPrefab && (i < allySpawns.Count))
                    {
                    GameObject rij = Instantiate(allyList[i].charPrefab, allySpawns[i]);  //spawn allies
                    Spawner tempSpawner =  allySpawns[i].GetComponent<Spawner>(); //set the spawner to hold the char SO for targetting 
                    tempSpawner.placedCharacter = allyList[i];
                    Debug.Log(allySpawns[i].position.x);
                    allyList[i].assignedSpawn = allySpawns[i];
                    allyList[i].combatText = combatText;
                    allyList[i].charPrefabSpawned = rij;
                    //set their timers to default
                    allyList[i].CalculateActSpeed();
                    allyList[i].ResetTurnTimer();
                    Debug.Log("Spawn completed");
                    }

            }
            for (int i = 0; i < enemyList.Count; i++)
            {
                Debug.Log("spawned: " + enemyList[i].nameSO);

                if (enemyList[i].charPrefab && (enemySpawns.Count > i))
                {
                    GameObject rij = Instantiate(enemyList[i].charPrefab, enemySpawns[i]); //spawn enemies
                    Spawner tempSpawner =  enemySpawns[i].GetComponent<Spawner>();
                    tempSpawner.placedCharacter = enemyList[i];
                    enemyList[i].assignedSpawn = enemySpawns[i];
                    enemyList[i].combatText = combatText;
                    enemyList[i].charPrefabSpawned = rij;
                    enemyList[i].CalculateActSpeed();
                    enemyList[i].ResetTurnTimer();
                }
            }

    }


        
    
    IEnumerator PlayerAttack(BattleCharacter attackTarget)
    {
        //damage animation
        //Damage script on target
        //wait 
        //do all other neccessary calculations, checks, updates, etc.
        int damage = (int)characterTurn.str;
        

        Debug.Log(characterTurn.nameSO + " attacked " + attackTarget.nameSO);
        characterTurn.AnimateMe(2);
        
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        attackTarget.Hurt(damage);
        EndOfTurn();
    }

    public IEnumerator Cast(BattleCharacter attackTarget) //the ability selected should be held on each character for memory selection convenience         <--------------
    {
        //damage animation
        //Damage script on target
        //wait 
        //do all other neccessary calculations, checks, updates, etc.
        Debug.Log(characterTurn.nameSO + " used: " + abilityChoice.abilityName +" on " + attackTarget.nameSO);
        //caseSwitch for base damage modifier
        int baseDamage;
        switch(abilityChoice.statModifier){ //instead of using a switch i could just have everything flow and use bools to check where to progress to next, seems this works well tho
        
        case (AbilitySO.Stat.maxHP):
            baseDamage = (int)(characterTurn.maxHP*abilityChoice.abilityPower);
            break;
        case (AbilitySO.Stat.Mana):
            baseDamage = (int)(characterTurn.maxMana*abilityChoice.abilityPower);
            break;    
        case (AbilitySO.Stat.Str):
            baseDamage = (int)(characterTurn.str*abilityChoice.abilityPower);
            break;   
        case (AbilitySO.Stat.Def):
            baseDamage = (int)(characterTurn.def*abilityChoice.abilityPower);
            break;   
        case (AbilitySO.Stat.MDef):
            baseDamage = (int)(characterTurn.mDef*abilityChoice.abilityPower);
            break;   
        case (AbilitySO.Stat.Intelligence):
            baseDamage = (int)(characterTurn.intelligence*abilityChoice.abilityPower);
            break;   
        case (AbilitySO.Stat.Spr):
            baseDamage = (int)(characterTurn.spr*abilityChoice.abilityPower);
            break;    
        case (AbilitySO.Stat.Spd):
            baseDamage = (int)(characterTurn.spd*abilityChoice.abilityPower);
            break;        
        default:
            baseDamage = (int)abilityChoice.abilityPower;
            break;
        }

        //then check elements and morph baseDamage to damage 
        int damage = baseDamage;

        if (abilityChoice.hurtHeal == AbilitySO.HurtOrHeal.Hurt) //if it's a damage ability
        {
            if (abilityChoice.targettingType == AbilitySO.Targetting.Single)  //single target choice
            {
                attackTarget.Hurt(damage);                                  
            }
            if (abilityChoice.targettingType == AbilitySO.Targetting.All)  //multi target choice
            {
                if (characterTurn.enemyOrAlly == BattleCharacter.enemyAlly.Enemy)   //if enemy, hit all allies
                {
                    for (int i = 0; i < battleUI.allyListAlive.Count; i++)
                    {
                        battleUI.allyListAlive[i].Hurt(damage); 
                    }
                }
                if (characterTurn.enemyOrAlly == BattleCharacter.enemyAlly.Ally) //if ally, hit all enemies
                {
                    for (int i = 0; i < battleUI.enemyListAlive.Count; i++)
                    {
                        battleUI.enemyListAlive[i].Hurt(damage); 
                    }
                    
                }
            }  
        }

        if (abilityChoice.hurtHeal == AbilitySO.HurtOrHeal.Heal) //if it's a healing ability
        {
            attackTarget.Heal(damage);
        }
        //then run through applying status elements
        

        characterTurn.AnimateMe(2);
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        EndOfTurn();
    }

    IEnumerator Defend()
    {
        //Do defend stuff
        characterTurn.AnimateMe(0);
        yield return new WaitForSeconds(1);
        EndOfTurn();
        
    }

    IEnumerator EnemyTurnAI()
    {
        battleUI.BuildTargetLists();
        yield return new WaitForSeconds(3);
        //characterTurn
        int choice = Random.Range(0, 6);
        int AItargetNo = Random.Range(0, battleUI.allyListAlive.Count-1);
        BattleCharacter AItarget = battleUI.allyListAlive[AItargetNo];
        if (choice <= 6)
        {
            
            StartCoroutine(PlayerAttack(AItarget));
        }
    }

    private void CheckWinLoss()
    {
        Debug.Log("checking for a win or loss");
        //if all allies dead, gamestate = lose
        int alDead = 0;
        for (int i = 0; i < allyList.Count; i++)
        {
            if (allyList[i].alive == false)
                {alDead = alDead + 1;}
                
        }
        if (alDead == allyList.Count)
        {
            currentState = battleStates.Lose;
            return;
        }   
        //if all enemies dead, gamestate = won
        int enDead = 0;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].alive == false)
                {enDead = enDead + 1;}
        }
        
        if (enDead == enemyList.Count)
        {
            Debug.Log("win battle");
            currentState = battleStates.Win;
            return;
        }
        
     
    }

    private void EndOfTurn()
    {
        battleUI.BuildTargetLists();
        if (target.alive == false && battleUI.enemyListAlive.Count != 0)  
        {
            target = battleUI.enemyListAlive[0];  //change target if i killed mine
        }
        Debug.Log("doing end of turn calculations");
        CheckWinLoss();
        if (currentState != battleStates.Win && currentState != battleStates.Lose)
        {characterTurn.turnNumber = characterTurn.turnNumber + 1;
        currentState = battleStates.CheckTurn; //progress to calculate next turn
        Debug.Log(currentState);
        currentAction = actionChoice.UI;  
        waiting = false;}
    }
}
