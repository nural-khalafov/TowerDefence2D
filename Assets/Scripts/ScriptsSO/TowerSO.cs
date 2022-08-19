using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TowerDefence/Create Tower", fileName = "New Tower")]
public class TowerSO : ScriptableObject
{
    [SerializeField] public int Damage;
    [SerializeField] public float Range;
    [SerializeField] public float ShootInterval;
    [SerializeField] public int BuildPrice;
}