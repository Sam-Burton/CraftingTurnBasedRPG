using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    public List<TagObject> matTags = new List<TagObject>();
    public List<TagObject> otherTags = new List<TagObject>();

    private void GenerateLoot()
    {
        MaterialItem loot;
        loot = ScriptableObject.CreateInstance<MaterialItem>();

        //rare loot indicator variable

        /////
        // Material
        /////

        //look through a potential drop list for that dungeon theme X Scrap that, let's start simple and have all themes drop the same type of loot
        //roll for material, random number
        int matRoll = Random.Range(0, matTags.Count);
        loot.matTags.Add(matTags[matRoll]);


        //use an rng function to set the material level based off of the dungeon difficulty
        float matLevelRoll = Random.Range(0.8f, 1f);
        loot.matLevel = (int)(matLevelRoll * BattleController.dungeonDifficulty);

        //use a random function to pick a rarity (rarer is less likely)
        //pick a random number, use it as X coordinates in an exponential function of 0 - 1
        float rarityRoll = Random.Range(0f, 1f);
        loot.quality = (int)(100f * (System.Math.Pow(rarityRoll, 2.5f))); //quality is decided by 100x^2.5 when x is a random roll of 0-1                                       <-------- to decrease luck, increase power

        //pick a potential max taglist count based off of rarity, dungeon difficulty                                                                                            <---- add if including multiple mat tag rolls

        //randomly pick a material tag                                                                                                                                          <--- add weightings later
        //assign a name based on material tag (handle can be wood,blah,blah,blah)
        int matNameRoll = Random.Range(0, loot.matTags.Count);
        loot.matTags[matNameRoll].title = loot.title;
        //find a prefix and suffix

        /////
        // Tags
        /////

        //roll for each tier of tag rarity
        //if rarity roll is a yes
        //pick random from that tag tier 
        //add it to the item 
        //return out

        //repeat until taglist.count = 1 if tag guaranteed

        //repeat for max potential taglist count

        //add it to an inventory
    }
}
