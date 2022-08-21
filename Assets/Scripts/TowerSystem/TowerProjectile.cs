using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public int ProjectileDamage;

    private void Update()
    {
        ProjectileDamage = gameObject.GetComponentInParent<Tower>().TowerDamage;
    }
}
