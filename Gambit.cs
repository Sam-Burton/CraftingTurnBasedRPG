using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum targetType{
    Ally,
    Foe,
    Self
};

public enum conditionType{
    Health,
    Mana,
    StatusEffect,
    Present //if 2+ allies present, if 2+ enemies present, if 1 enemy left
};

public enum conditionQualifier{
    Higher,
    Lower,
    Equal,
    Highest,
    Lowest,
    Has,
    Hasnt
};
[CreateAssetMenu(fileName = "Gambit", menuName = "Gambit")]
public class Gambit : ScriptableObject
{
    //
    //target type   (Ally/Foe/Self)
    public targetType target;
    //condition type (HP, status effect, mana, etc.)
    public conditionType condition;
    //higher or lower than
    public conditionQualifier conditionQ;
    //condition float (for % health or mana or for #enemies/allies)
    public float conditionFloat;
    //condition SO (for status effects)
    //public StatusEffect conditionSE;
    //spell/action  //consider making attack into a SO skill, makes it customizable and easy to select here
    public AbilitySO action;
    //to run:
        //fetch line i of gambits, for (i) gambitsList[i]
        //if line1 has all components X != null && Y != null, etc.
            //check the if condition (is there an enemy below 20%?, is there an ally below 40%?)
                //run target command
                //return and end turn if ran
            //if not then check next (i)
        //backup action if at last (i)
    

    //char gambits could be an array of these, each portion of each line is editable and assignable to a list of figures
}
