using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TowerType
{
    None,
    WarriorTower,
    ArcheryTower,
    MagicTower,
    Catapult
}

public class Tower : MonoBehaviour
{
    public TowerType Type;
    public bool IsBought = false;

    [Header("Tower Options")]
    [SerializeField] public int TowerDamage;
    [SerializeField] private float _towerRange;
    [SerializeField] private float _towerAttackSpeed;
    [SerializeField] private int _towerBuildPrice;
    [SerializeField] private bool _isTowerFiring;

    [SerializeField] private Transform m_target;
    [SerializeField] private Transform m_weapon;

    [Header("Projectile Options")]
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private float m_projectileSpeed = 10f;
    [SerializeField] private float m_projectileLifetime = 1f;

    public void SetTower(TowerType type)
    {
        if (type == TowerType.None)
        {
            TowerDamage = 0;
            _towerRange = 0;
            _towerAttackSpeed = 0;
            _towerBuildPrice = 0;
            m_weapon = gameObject.transform.Find("EmptyWeapon");
        }
        if (Type == TowerType.WarriorTower)
        {
            TowerDamage = GameManager.Instance.WarriorTower.Damage;
            _towerRange = GameManager.Instance.WarriorTower.Range;
            _towerAttackSpeed = GameManager.Instance.WarriorTower.ShootInterval;
            _towerBuildPrice = GameManager.Instance.WarriorTower.BuildPrice;
            m_weapon = gameObject.transform.Find("Warrior Tower/Sword");
        }
        else if (Type == TowerType.ArcheryTower)
        {
            TowerDamage = GameManager.Instance.ArcheryTower.Damage;
            _towerRange = GameManager.Instance.ArcheryTower.Range;
            _towerAttackSpeed = GameManager.Instance.ArcheryTower.ShootInterval;
            _towerBuildPrice = GameManager.Instance.ArcheryTower.BuildPrice;
            m_weapon = gameObject.transform.Find("Archery Tower/Bow");
        }
        else if (Type == TowerType.MagicTower)
        {
            TowerDamage = GameManager.Instance.MagicTower.Damage;
            _towerRange = GameManager.Instance.MagicTower.Range;
            _towerAttackSpeed = GameManager.Instance.MagicTower.ShootInterval;
            _towerBuildPrice = GameManager.Instance.MagicTower.BuildPrice;
            m_weapon = gameObject.transform.Find("Magic Tower/Staff");
        }
        else if (Type == TowerType.Catapult)
        {
            TowerDamage = GameManager.Instance.Catapult.Damage;
            _towerRange = GameManager.Instance.Catapult.Range;
            _towerAttackSpeed = GameManager.Instance.Catapult.ShootInterval;
            _towerBuildPrice = GameManager.Instance.Catapult.BuildPrice;
            m_weapon = gameObject.transform.Find("Catapult/Stone");
        }
    }

    private void Start()
    {
        if (m_weapon != null)
        {
            m_target = FindObjectOfType<Enemy>().transform;
            m_weapon = transform.Find("EmptyWeapon");
        }
    }

    private void Update()
    {
        FindClosestEnemy();
    }

    private void FixedUpdate()
    {
        AimWeapon();
    }

    private void AimWeapon()
    {
        if (m_target != null && m_weapon != null)
        {
            float targetDistance = Vector2.Distance(transform.position, m_target.position);

            if (targetDistance < _towerRange)
            {
                m_weapon.right = m_target.position - m_weapon.position;

                FireProjectile(true);
            }
            else 
            {
                FireProjectile(false);
            }
        }
    }

    private void FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = _towerRange;

        foreach (Enemy enemy in enemies)
        {
            float targetDistance = Vector2.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        m_target = closestTarget;
    }

    private void FireProjectile(bool isActive)
    {
        StartCoroutine(OnFire(_towerAttackSpeed));
    }

    IEnumerator OnFire(float delay)
    {
        yield return new WaitForSeconds(delay);

        _isTowerFiring = true;

        if (_isTowerFiring) 
        {
            GameObject projectileInstance = Instantiate(m_projectile, m_weapon.position, Quaternion.identity, gameObject.transform).gameObject;
            Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = m_weapon.right * m_projectileSpeed;
            }

            Debug.Log("Fired");


            Destroy(projectileInstance, m_projectileLifetime);
        }
        
        _isTowerFiring = false;
    }
}
