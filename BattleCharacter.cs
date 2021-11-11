using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleCharacter", menuName = "BattleCharacterSO")]
public class BattleCharacter : ScriptableObject
{
    public string nameSO;
    public Sprite icon;
    [SerializeField]
    public GameObject charPrefab; //this'll hold the sprite and animations n shit
    public GameObject charPrefabSpawned;
    public Animator anim;
    public enum enemyAlly
    {
        Enemy,
        Ally
    }
    public enemyAlly enemyOrAlly;
    public bool alive = true;
    public Transform assignedSpawn;

    public GameObject combatText;

    [Header("Turn info")]
    //next turn info
    public float actSpeed; //the default gap between turns
    public float nextTurn; //the actual gap until next turn
    public int turnNumber; //what turn the character is on right now (starts at 0)
    [Header("Stats")]
    //current stats
    public int maxHP;
    public float currentHP;
    public float maxMana;
    public float currentMana;
    public float str;
    public float intelligence;
    public float spr;
    public float def;
    public float mDef;
    public float spd;
    // type resistances                                                                                                                          <------

    //assign a loot table that's a seperate scriptable object or something like that                                                             <------

    public List<AbilitySO> abilities;

    
    public Gambit[] gambits = new Gambit[12];  //12 gambit slots

    // Start is called before the first frame update
    void Start()
    {
        //load in stats from the SO

        CalculateActSpeed();
        //set the Combat text                                                                                                                   <--------------

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTurnTimer()
    {
        nextTurn = actSpeed;
        //start of turn effects like take 1 turn off buff/debuff durations, apply burns n shit                                                  <--------------

        //add turn speed change effects here                                                                                                    <-----------
    }

    public void CalculateActSpeed() 
    {
        actSpeed = (float)System.Math.Pow((spd+5), -0.6); //this could return a value instead of setting actspeed if i want to later implement previews of changes
    }

    //a damage dealing calculation
    public void Hurt(int amount)
    {
        AnimateMe(1);
        int damageAmount = (int)(amount * (100 / (100 + def)));
        currentHP = Mathf.Max(currentHP - damageAmount, 0);
        //Damage text
        TextPopup(damageAmount);
        

        Debug.Log(this.nameSO + " took " + damageAmount + "damage");
        //play UI damage animation and update
        if (currentHP == 0)
        {
            Die();
        }
    }
    //a heal calculation
    public void Heal(int amount)
    {
        int healAmount = amount;
        currentHP = Mathf.Min(currentHP + healAmount, maxHP);
        //play UI damage animation and update
        TextPopup(healAmount);
        Debug.Log(this.nameSO + " was healed for " + healAmount);
    }    
    //a shield calculation
    // public void Shield(int amount)
    // {
    //     int healAmount = amount;
    //     health = Mathf.Min(health + healAmount, maxHealth);
    // }

    public void Defend()
    {
        //defencePower += (int)(defencePower * .33); //change the defence modifier
        Debug.Log("Defending");
        
    }
    
    public virtual void Die()
    {
        //death animation
        alive = false;
        // Destroy(this.gameObject);
        Debug.LogFormat("{0} has died!", nameSO);

        AnimateMe(3);
    }


    public void PullPlayer(TeamCharacterSO charSO)
    {
        //name and def
        charPrefab = charSO.charPrefab;
        icon = charSO.icon;
        nameSO = charSO.charName;
        maxHP = charSO.healthPoints;
        currentHP = maxHP;
        maxMana = charSO.mana;
        currentMana = maxMana;
        str = charSO.strength;
        intelligence = charSO.strength;
        spr = charSO.spirit;
        def = charSO.defence;
        mDef = charSO.magicDefence;
        spd = charSO.speed;
        enemyOrAlly = enemyAlly.Ally;
        abilities = charSO.abilities;
        turnNumber = 0;
    }

    private void TextPopup(int amount)
    {
        Vector3 dtextSpawn = charPrefabSpawned.transform.position + new Vector3(0, 2, 0);
        GameObject damagePopup = Instantiate(combatText, dtextSpawn, Quaternion.identity); //charPrefabSpawned.transform.position
        CombatText dpct = damagePopup.GetComponent<CombatText>();
        dpct.Setup(amount);
    }

    public void AnimateMe(int aniChoice)
    {
        if (anim == null)
            anim = charPrefabSpawned.GetComponent<Animator>();
        if (anim != null)
        {
            if (aniChoice == 0)
                anim.SetTrigger("Defend");
            if (aniChoice == 1)
                {anim.SetTrigger("Hurt"); 
                Debug.Log("Hurt triggered animation");}
            if (aniChoice == 2)
                anim.SetTrigger("Attack");
            if (aniChoice == 3)
                anim.SetBool("Alive", false);  
        }        
    }
}
