using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleGUI : MonoBehaviour
{
    public BattleController battleController;
    enum enemyAlly
    {
        Enemy,
        Ally
    }
    [SerializeField] private GameObject spellPanel;
    void Start () {
        spellPanel.SetActive(false);
	}    

    [SerializeField] private GameObject Actions;
    [SerializeField] private Button button;
    public GameObject targetGO;
    private Vector3 arrowPos;
    public TurnIndicator turnInd;
    private bool axisWait = false;
    private bool inputWait = false;
    //could put this bit onto each character panel and call it from here with SetPlayerInfo(chardeets)
    //first get it to instantiate a player panel per character on the field
    //get it to grey out if they dead
    public List<GameObject> charInfoPanels = new List<GameObject>();
    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    
    public List<BattleCharacter> allyListAlive = new List<BattleCharacter>();
    public List<BattleCharacter> allyListDead = new List<BattleCharacter>();
    public List<BattleCharacter> allytargetsList = new List<BattleCharacter>();
    public List<BattleCharacter> enemyListAlive = new List<BattleCharacter>();
    public List<BattleCharacter> enemyListDead = new List<BattleCharacter>();
    public List<BattleCharacter> enemytargetsList = new List<BattleCharacter>();

    private void Update()
    {
        SetPlayerInfo();  //move this to the end of damage taking sequences                 <-------------- move me
        if (targetGO.activeSelf == true)
        {
            //if attack spell, use alive list, 
            allytargetsList = allyListAlive;
            enemytargetsList = enemyListAlive;
            //if rez spell use dead list                                                    <-------------- add me when you add rez pls
        //Debug.Log(battleController.enemyList[0].assignedSpawn.position.x);
            if (Input.GetAxisRaw("Submit") > 0 && inputWait == false)
                targetGO.SetActive(false);
                //give indication to execute next order 
            if (Input.GetAxisRaw("Submit") == 0) 
                inputWait = false;         //wait for input key to be let go before we let it press again    

            arrowPos = Camera.main.WorldToScreenPoint(battleController.target.assignedSpawn.position); //worldtoscreenpoint converts world position to screen position
            targetGO.transform.position = arrowPos; //then set the overlay position to that position

        // ^can't get these to work

            //cycle through enemies list to see if current target is on there
            //for loop, if target = list[i] then if (i+1) <= list.Count then target = list[i+1], else target = list[0]
            if (Input.GetAxisRaw("Vertical") == 1 && axisWait != true)
                {
                    axisWait = true;
                    if (battleController.target.enemyOrAlly == BattleCharacter.enemyAlly.Ally && allytargetsList.Count != 0)  //if target is an ally
                    {
                    for (int i = 0; i < allytargetsList.Count; i++)               //go through the ally list 
                    {
                        if (battleController.target == allytargetsList[i])         //until we get to the right i of that ally
                            if ((i+1) >= allytargetsList.Count)                    //if i+1 doesn't exist in the list
                            {
                                battleController.target = allytargetsList[0];     //then reset the target back to 0 in that list
                                return;
                            }
                            else                                                            //otherwise
                            {
                                battleController.target = allytargetsList[i+1];   //target = i+1
                                return;
                            }
                    }
                    }
                    if (battleController.target.enemyOrAlly == BattleCharacter.enemyAlly.Enemy && enemytargetsList.Count != 0)
                    {
                        //Debug.Log("going to Vertical + enemy for targetting loop ");
                    for (int i = 0; i < enemytargetsList.Count; i++) //again for enemies
                    {
                        //Debug.Log("i is : " + i + " max count: " + battleController.enemyList.Count);
                        if (battleController.target == enemytargetsList[i])
                        {
                            //Debug.Log("targetting target is #: " + i);
                            if ((i+1) >= enemytargetsList.Count)
                            {
                                //Debug.Log("did the if: " + i);
                                battleController.target = enemytargetsList[0];
                                return;
                            }
                            if ((i+1) < enemytargetsList.Count)
                            {
                                //Debug.Log("did the else: " + i + " to set enemy to enemylist " + (i+1) );
                                battleController.target = enemytargetsList[i+1];  //this bit isn't working
                                return;
                            }
                        }
                    }    
                    }                
                    
                }
            if (Input.GetAxisRaw("Vertical") == -1 && axisWait != true)
                {
                    axisWait = true;
                    if (battleController.target.enemyOrAlly == BattleCharacter.enemyAlly.Ally && allytargetsList.Count != 0)  //if target is an ally
                    {
                    for (int i = 0; i < allytargetsList.Count; i++)
                    {
                        if (battleController.target == allytargetsList[i])
                            if ((i-1) < 0)
                            {
                                battleController.target = allytargetsList[allytargetsList.Count - 1];
                                return;
                            }
                            else
                            {
                                battleController.target = allytargetsList[i-1];
                                return;
                            }
                    }
                    }
                    if (battleController.target.enemyOrAlly == BattleCharacter.enemyAlly.Enemy && enemytargetsList.Count != 0)
                    {
                        //Debug.Log("going to Vertical - enemy for targetting loop ");
                    for (int i = 0; i < enemytargetsList.Count; i++) //again for enemies
                    {
                        //Debug.Log("target for : " + i);
                        if (battleController.target == enemytargetsList[i])
                        {
                            if ((i-1) < 0)
                            {
                                //Debug.Log("got to if: " + i);
                                battleController.target = enemytargetsList[enemytargetsList.Count - 1];
                                return;
                            }
                            else
                            {
                                //Debug.Log("did the else: " + i + " to set enemy to enemylist " + (i-1) );
                                battleController.target = enemytargetsList[i-1];
                                return;
                            }
                        }
                    } 
                    }                   
                    
                }
            if (Input.GetAxisRaw("Horizontal") != 0 && axisWait != true)
            {
                axisWait = true;
                //if in ally list, set to enemy[0]
                if (battleController.target.enemyOrAlly == BattleCharacter.enemyAlly.Enemy)
                    {
                    battleController.target = allytargetsList[0];
                    return;
                    }
                if (battleController.target.enemyOrAlly == BattleCharacter.enemyAlly.Ally)
                    {
                    battleController.target = enemytargetsList[0];
                    return;
                    }
                // for (int i = 0; i < battleController.allyList.Count; i++)
                // {
                //     if (battleController.target = battleController.allyList[i])
                //         battleController.target = battleController.enemyList[0];
                // }
                // for (int i = 0; i < battleController.enemyList.Count; i++)
                // {
                //     if (battleController.target = battleController.enemyList[i])
                //         battleController.target = battleController.allyList[0];
                // }
                
            }
            if (Input.GetAxisRaw("Vertical") == 0)
                axisWait = false;
        }
            
    }



    public void SetPlayerInfo()
    {
        for (int i = 0; i < battleController.allyList.Count; i++)
        {
            hpSlider = charInfoPanels[i].GetComponentInChildren<Slider>();
            //Debug.Log(hpSlider);
            hpSlider.maxValue = battleController.allyList[i].maxHP;
            hpSlider.value = battleController.allyList[i].currentHP;

            nameText = charInfoPanels[i].GetComponentInChildren<TextMeshProUGUI>();
            nameText.text = battleController.allyList[i].nameSO;

        }

        for (int i = 0; i < charInfoPanels.Count; i++)
        {
            if (i >= battleController.allyList.Count)
            {
                //Debug.Log("set inactive panel: " + i);
                charInfoPanels[i].SetActive(false);
            }
        }
        
        //nameText.text = Reference.unitName;
        
    }

    // Update is called once per frame
    public void PlayerTurn()
    {
        Actions.SetActive(true);
    }

    public void ToggleSpellPanel()
    {
        Debug.Log("Spell Panel Toggle");
        if (spellPanel.activeInHierarchy == true)
            spellPanel.SetActive(false);
        else
        {
            if (spellPanel.activeInHierarchy == false)
                spellPanel.SetActive(true);  
        }
                  
    }

    public void BuildSpellList(List<AbilitySO> spells)
    {
        GameObject content = spellPanel.transform.GetChild(0).GetChild(0).gameObject; //pick out the content window of the scrollview
        if (content.transform.childCount > 0)
        {
            foreach(Button button in content.transform.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }

        foreach(AbilitySO spell in spells)
        {
            Button spellButton = Instantiate<Button>(button, content.transform);
            spellButton.GetComponentInChildren<Text>().text = spell.abilityName;
            spellButton.onClick.AddListener(() => OnCastButton(spell));               //gotta add the on click function here
        }
    }

    public void ChangeTarget()
    {
        Actions.SetActive(false);  //turn off the players action choice UI
        
        BuildTargetLists();

        targetGO.SetActive(true);  //stick the targetter on, this'll let the update function track targetting
        //ADD A WAIT FOR CONFIRM BUTTON TO RESET SO IT DOESNT DOUBLE HIT STRAIGHT INTO YOUR TARGET
        
        //update function above makes movement commands switch targets whilst this is active

    }

    public void BuildTargetLists()
    {
        enemyListAlive = new List<BattleCharacter>();
        enemyListDead = new List<BattleCharacter>();
        allyListAlive = new List<BattleCharacter>();
        allyListDead = new List<BattleCharacter>();
        for (int i = 0; i < battleController.allyList.Count; i++)  //update alive/dead targets list
        {
            if (battleController.allyList[i].alive == true)
            {
                allyListAlive.Add(battleController.allyList[i]); 
            }
            if (battleController.allyList[i].alive == false)
            {
                allyListDead.Add(battleController.allyList[i]); 
            }
        }
        for (int i = 0; i < battleController.enemyList.Count; i++)  //update alive/dead targets list
        {
            if (battleController.enemyList[i].alive == true)
            {
                enemyListAlive.Add(battleController.enemyList[i]); 
            }
            if (battleController.enemyList[i].alive == false)
            {
                enemyListDead.Add(battleController.enemyList[i]); 
            }
        }
    }

    public void OnAttackButton()
    {
        //add inputwait
        inputWait = true;
        battleController.currentAction = BattleController.actionChoice.Attack;
        ChangeTarget();
    }


    public void OnCastButton(AbilitySO ability)
    {
        inputWait = true;
        battleController.currentAction = BattleController.actionChoice.Cast;
        battleController.abilityChoice = ability;
        //if single target then do this
        ChangeTarget();
        //otherwise do the finish lines of changing target command cause no target required for multi target spells
    }

    public void OnDefButton()
    {
        battleController.currentAction = BattleController.actionChoice.Defend;        

    }
}

