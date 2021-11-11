using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "playable Character")]
public class TeamCharacterSO : ScriptableObject
{
    //name
    public string charName;
    public GameObject charPrefab;
    public Sprite icon;
    

    //stats
    public int healthPoints;
    public float mana;
    public float strength;
    public float intelligence;
    public float spirit;
    public float defence;
    public float magicDefence;
    public float speed;

    //type resistances

    public EquipmentObj mainHand;
    public EquipmentObj offHand;
    public EquipmentObj armour;

    public List<AbilitySO> abilities;

    public Gambit[] gambits = new Gambit[12];  //12 gambit slots

    public void EquipItem(EquipmentObj _item, string slot)
    {
        if (slot == "mainHand")
            {
                // if slot not empty, add old equipment item to inventory
                // check slot empty, add the new equipment to the slot
                // calculate stats
            }
            if (slot == "offHand")
            {
                // if slot not empty, add old equipment item to inventory
                // check slot empty, add the new equipment to the slot
                //calculate stats
            }
            if (slot == "armour")
            {
                // if slot not empty, add old equipment item to inventory
                // check slot empty, add the new equipment to the slot
                //calculate stats
            }
      
    }
    private void CalculateStats()
    {
        //add up stats from equipment objects
    }

}
