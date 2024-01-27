//This script contains the meat of the functionality. It has a long list of responsilities which fall within managing the main tilemap.


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameOfLifeTileMap : MonoBehaviour
{
    /// <summary>
    /// Use this custom rules creator to create your own cellular automata rules in the inspector.
    /// </summary>
    [Serializable]
    public struct customRules
    {
        public string RuleName;
        public List<int> customBirthRule; // Hey! You can edit your own birth rule on start if you'd like!
        public List<int> customSurviveRule;
    }

    public List<customRules> customRulesList = new List<customRules>();
    int customRulesIndex;

    //Recover objects in Unity to interact with

    public GameObject BirthRulesetField;
    public GameObject SurviveRulesetField;
    public Tilemap tilemap;
    public Tile aliveTile;
    public int neighborSurvive;
    public List<int> BirthRule = new List<int>(); // Hey! You can edit your own birth rule on start if you'd like!
    public List<int> SurviveRule = new List<int>(); // Give it a try by modifying this and the above list in the API

    /// <summary>
    /// These rulesets are provided as examples. Each has a descriptor. 
    /// The birth ruleset denotes the required active tiles around a tile to allow burth. 
    /// Each number in the birth list is an acceptable number of neighboring tiles to allow a birth.
    /// The same logic applies for the survival list.
    /// The first example is Conways game of life. The most famous example of cellular automata.
    /// </summary>
    //Existing Rulesets  (   In the future, it would be excellent to make a feature to create list pairs for new rulesets   )
    public List<int> ConwaysLifeBirth = new List<int>();
    public List<int> ConwaysLifeSurvive = new List<int>();

    /// <summary>
    /// The following is my favourite example of C.A.
    /// Day and night is a unique ruleset in which patterns are interchangeable in behavior regardless of their color.
    /// A dark pattern on white will act in the exact same way as its identical white counterpart on black.
    /// This results from the ruleset being a mirror of itself.
    /// </summary>
    public List<int> DaynNightBirth = new List<int>();
    public List<int> DaynNightSurvive = new List<int>();
    /// <summary>
    /// !!!WARNING!!! This rulset is slightly unstable. You can get great explosive visuals using this ruleset.
    /// </summary>
    public List<int> ExplodeBirth = new List<int>();
    public List<int> ExplodeSurvive = new List<int>();
    /// <summary>
    /// This ruleset is a tamed version of the above. You will get post modern fractal effects as a result of running this C.A.
    /// </summary>
    public List<int> CyberPunkBirth = new List<int>();
    public List<int> CyberPunkSurvive = new List<int>();


    //Failed rate of update button, instead I use a repeat script
    public float rate;

    //This was meant to go in conjuction with the rate button, however instead repeat script manages this.
    public bool updateButton = false;

    public void Start()
    {
        //The only start up operation is to reset the text to correspond to the rules that you start with
        ResetRuleText();
        if (customRulesList.Count > 0 ) 
        {
            for (int i = 0; i < customRulesList.Count; i++)
            {
                //customRulesList[i].customBirthRule = new List<customRules>();
                //customRulesList[i].customSurviveRule = new List<customRules>();
            }
        }
    }
    public void ResetRuleText()
    {
        //Display current ruleset to the User
        BirthRulesetField.GetComponent<Text>().text = "BirthRuleset:";
        SurviveRulesetField.GetComponent<Text>().text = "SurviveRuleset:";
        //Display Birth rules as numbers in an text field
        foreach (int item in BirthRule)
        {
            BirthRulesetField.GetComponent<Text>().text += item.ToString() + ", ";
        }
        //Display Survive rules as numbers in an text field
        foreach (int item in SurviveRule)
        {
            SurviveRulesetField.GetComponent<Text>().text += item.ToString() + ", ";
        }

    }
    void ClearRules()
    {
        ResetRuleText();
        BirthRule.Clear();
        SurviveRule.Clear();

    }

    //There is  DRY problem for rulesetting, however, the way I prefer to work with the API, this solution is more initally time effective.
    //Unity Events does not allow a list as a parameter in API
    //(   When a list pair system is implemented, I can have a single function which is SetRules which takes a list pair parameter   )
    public void SetConwayRules()
    {
        ClearRules();
        BirthRule.AddRange(ConwaysLifeBirth);
        SurviveRule.AddRange(ConwaysLifeSurvive);
        ResetRuleText();

    }
    public void SetDaynNightRules()
    {
        ClearRules();
        BirthRule.AddRange(DaynNightBirth);
        SurviveRule.AddRange(DaynNightSurvive);
        ResetRuleText();

    }
    public void SetExplodeRules()
    {
        ClearRules();
        BirthRule.AddRange(ExplodeBirth);
        SurviveRule.AddRange(ExplodeSurvive);
        ResetRuleText();

    }
    public void SetCyberpunkRules()
    {
        ClearRules();
        BirthRule.AddRange(CyberPunkBirth);
        SurviveRule.AddRange(CyberPunkSurvive);
        ResetRuleText();

    }

    public void SetCustomRules()
    {
        if(customRulesIndex > customRulesList.Count - 1)
        {
            customRulesIndex = 0;
        }

        ClearRules();
        BirthRule.AddRange(customRulesList[customRulesIndex].customBirthRule);
        SurviveRule.AddRange(customRulesList[customRulesIndex].customSurviveRule);
        ResetRuleText();

        customRulesIndex++;

    }

    //On update, check if the update life button is hit.
    public void Update()
    {
        if (updateButton)
        {
            UpdateLife();
            updateButton= false;
        }
        if (Input.GetKeyDown(KeyCode.N)) 
        {
            SetCustomRules();
        }

    }
    public void ButtonTrue()
    {
        updateButton= true;
    }
    //public int neighborBorn;

    //On update life, create a dictionary which manages tile positions and neighbor count
    public void UpdateLife()
    {
        //currently this is unused
        TileBase[] cells = tilemap.GetTilesBlock(tilemap.cellBounds);
        //currently this is unused
        Dictionary<Vector3Int, int> Creator = new Dictionary<Vector3Int, int>();
        //Creator Dictionary
        //Vector3Int is used because as of now, when I use Vector 2 variables they are refused by the tilebase
        List<Vector3Int> kill_list = new List<Vector3Int>();

        //Check all positions within the tilemap bounds
        foreach (var cell in tilemap.cellBounds.allPositionsWithin)
        {
            //initalize tile variables
            var neighbor_count = 0;
            var not_killed = true;
            //If there is a tile at the position, run a nested loop for a grid to check neighbors 
            //(   This can be exploited in the future to create Automata with larger Grids   )
            if (tilemap.HasTile(cell))
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        var neighbor = cell + new Vector3Int(x - 1, y - 1, 0);
                        if (neighbor == cell)
                        { continue; }
                        //If there are neighbors, add to neighbor count
                        if (tilemap.GetTile(neighbor) != null)
                        { neighbor_count++; }
                        else
                        {
                            if (Creator.ContainsKey(neighbor))
                                Creator[neighbor] += 1;
                            else
                                Creator[neighbor] = 1;
                        }
                    }
                }
                if (!SurviveRule.Contains(neighbor_count) & not_killed)
                {
                    not_killed = false;
                    kill_list.Add(cell);
                }


            }
        }
        foreach (var kill in kill_list)
        {
            tilemap.SetTile(kill, null);
        }
        foreach (var create in Creator)
        {
            if (BirthRule.Contains(create.Value))
            {
                tilemap.SetTile(create.Key, aliveTile);
            }
        }
    }

    public void ClearAll()
    {
            tilemap.ClearAllTiles();
    }
}


        
/* GD SCRIPT. I used this code to get an idea of how to make some optimizations, and learn to implement my idea in the first place.
var cells = get_used_cells()
var creat_dict := {}
var kill_arr := []
for cell in cells:
    var neighbor_count = 0
    var not_killed := true
    for y in 3:
        for x in 3:
            var neighbor = cell + Vector2(x-1, y-1)
            if neighbor == cell:
                continue
            if get_cellv(neighbor) != INVALID_CELL:
                neighbor_count += 1
            else:
                if creat_dict.has(neighbor):
                    creat_dict[neighbor] += 1
                else:
                    creat_dict[neighbor] = 1
            if neighbor_count >= 4 and not_killed:
                not_killed = false
                #set_cellv(cell, -1)
                kill_arr.push_back(cell)
            if neighbor_count <= 1:
                #set_cellv(cell, -1)
                kill_arr.push_back(cell)
for kill in kill_arr:
    set_cellv(kill, -1)
for create in creat_dict:
    if creat_dict[create] == 3:
        set_cellv(create, 0)
*/