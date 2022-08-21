using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemyOptions")]
    [SerializeField] private int m_health;
    [SerializeField] private int m_damage;
    [SerializeField] private float m_speed;
    [SerializeField] private int _minReward; 
    [SerializeField] private int _maxReward; 

    [SerializeField] private EnemySO m_enemyType;

    public int Health 
    {
        get { return m_health; }
        set 
        { 
            m_health = value;
            if(value <= 0) 
            {
                m_health = 0;
                Destroy(gameObject);
            }
        } 
    }

    private void Start()
    {
        Health = m_enemyType._healthAmount;
        m_damage = m_enemyType._damage;
        m_speed = m_enemyType._speed;
        gameObject.GetComponent<EnemyHandler>()._speed = m_speed;
    }

    //private void OnParticleCollision(GameObject other)
    //{
    //    if (other.gameObject.GetComponentInParent<Tower>()) 
    //    {
    //        Health -= other.gameObject.GetComponentInParent<Tower>()._towerDamage;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Castle")
        {
            GameManager.Instance.DurabilityAmount -= m_damage;
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "TowerProjectile") 
        {
            Health -= collision.gameObject.GetComponent<TowerProjectile>().ProjectileDamage;

            GameManager.Instance.GoldAmount += Random.RandomRange(_minReward , _maxReward);
        }
    }
}
