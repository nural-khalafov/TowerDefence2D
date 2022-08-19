using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TowerDefence/Create Enemy", fileName = "New Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] public int _healthAmount;
    [SerializeField] public int _damage;
    [SerializeField] public float _speed;
}
