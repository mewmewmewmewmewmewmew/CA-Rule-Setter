using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int level;

    public int basicEnemyNumber;
    public int smallEnemyNumber;
    public int mediumEnemyNumber;
    public int largeEnemyNumber;
}
