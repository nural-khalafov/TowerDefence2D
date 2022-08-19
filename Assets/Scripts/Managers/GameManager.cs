using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Data")]
    [SerializeField] private int _goldAmount;
    [SerializeField] private int _durabilityAmount = 100;

    private Camera m_Camera;
    private TextMeshProUGUI _durabilityAmountText;
    private TextMeshProUGUI _goldAmountText;
    private TextMeshProUGUI _wavesAmountText;

    private RaycastHit2D _hit;
    private Collider2D _collider;

    [Header("Tower SOs")]
    public TowerSO WarriorTower;
    public TowerSO ArcheryTower;
    public TowerSO MagicTower;
    public TowerSO Catapult;

    [Header("Enemy SOs")]
    public EnemySO Snark;
    public EnemySO Zerg;
    public EnemySO GiantInsect;
    public EnemySO Dragon;

    [Header("Level Waypoints")]
    public List<Waypoint> _firstPath = new List<Waypoint>();

    private Vector2 worldPosition;

    private const string _towerSlotString = "TowerSlot";

    public int DurabilityAmount
    {
        get
        {
            return _durabilityAmount;
        }
        set
        {
            _durabilityAmount = value;

            if (value <= 0)
            {
                _durabilityAmount = 0;

                Time.timeScale = 0;
            }
        }
    }

    public int GoldAmount
    {
        get
        {
            return _goldAmount;
        }
        set
        {
            _goldAmount = value;

            if (value <= 0)
            {
                _goldAmount = 0;
            }
        }
    }


    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        m_Camera = Camera.main;

        _durabilityAmountText = GameObject.Find("Durability").GetComponentInChildren<TextMeshProUGUI>();
        _goldAmountText = GameObject.Find("Coins").GetComponentInChildren<TextMeshProUGUI>();
        _wavesAmountText = GameObject.Find("EnemyWaves").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        EnemySpawner.Instance.SpawnEnemies();
    }

    private void Update()
    {
        _durabilityAmountText.text = Convert.ToString(DurabilityAmount);
        _goldAmountText.text = Convert.ToString(GoldAmount);

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            return;
        }

        //Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 tapPosition = Touchscreen.current.primaryTouch.startPosition.ReadValue();
        worldPosition = m_Camera.ScreenToWorldPoint(tapPosition);
    }

    private void FixedUpdate()
    {
        _hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        _collider = _hit.collider;

        if (_collider != null)
        {
            if (_collider.tag == _towerSlotString && !_collider.transform.gameObject.GetComponent<Tower>().IsBought)
            {
                _collider.gameObject.transform.Find("TowerBuy_menu").gameObject.SetActive(true);
                _collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
            if (_collider.transform.name == "WarTower_buy")
            {
                StartCoroutine(BuyTower(TowerType.WarriorTower));
            }
            if (_collider.transform.name == "ArchTower_buy")
            {
                StartCoroutine(BuyTower(TowerType.ArcheryTower));
            }
            if (_collider.transform.name == "MagTower_buy")
            {
                StartCoroutine(BuyTower(TowerType.MagicTower));
            }
            if (_collider.transform.name == "Catapult_buy")
            {
                StartCoroutine(BuyTower(TowerType.Catapult));
            }

            if (_collider.tag == _towerSlotString && _collider.transform.gameObject.GetComponent<Tower>().IsBought) 
            {
                _collider.gameObject.transform.Find("TowerSell_menu").gameObject.SetActive(true);
                _collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
            if (_collider.transform.name == "Tower_sell")
            {
                StartCoroutine(SellTower(_collider.transform.gameObject.GetComponentInParent<Tower>().Type));
            }

        }
    }


    IEnumerator BuyTower(TowerType type)
    {
        while (!_collider.transform.gameObject.GetComponentInParent<Tower>().IsBought)
        {
            yield return new WaitForEndOfFrame();

            if (type == TowerType.WarriorTower)
            {
                if (GoldAmount >= WarriorTower.BuildPrice)
                {
                    GoldAmount -= WarriorTower.BuildPrice;
                    _collider.transform.gameObject.GetComponentInParent<Tower>().Type = TowerType.WarriorTower;
                    _collider.transform.gameObject.GetComponentInParent<Tower>().SetTower(TowerType.WarriorTower);
                    _collider.transform.parent.parent.gameObject.transform.Find("Warrior Tower").gameObject.SetActive(true);
                    _collider.transform.parent.parent.Find("Slot_img").gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Not enough gold for setting the Warrior Tower");
                    _collider.transform.parent.gameObject.SetActive(false);

                }
            }
            else if (type == TowerType.ArcheryTower)
            {
                if (GoldAmount >= ArcheryTower.BuildPrice)
                {
                    GoldAmount -= ArcheryTower.BuildPrice;
                    _collider.transform.gameObject.GetComponentInParent<Tower>().Type = TowerType.ArcheryTower;
                    _collider.transform.gameObject.GetComponentInParent<Tower>().SetTower(TowerType.ArcheryTower);
                    _collider.transform.parent.parent.gameObject.transform.Find("Archery Tower").gameObject.SetActive(true);
                    _collider.transform.parent.parent.Find("Slot_img").gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Not enough gold for setting the Archery Tower");
                    _collider.transform.parent.gameObject.SetActive(false);
                }
            }
            else if (type == TowerType.MagicTower)
            {
                if (GoldAmount >= MagicTower.BuildPrice)
                {
                    GoldAmount -= MagicTower.BuildPrice;
                    _collider.transform.gameObject.GetComponentInParent<Tower>().Type = TowerType.MagicTower;
                    _collider.transform.gameObject.GetComponentInParent<Tower>().SetTower(TowerType.MagicTower);
                    _collider.transform.parent.parent.gameObject.transform.Find("Magic Tower").gameObject.SetActive(true);
                    _collider.transform.parent.parent.Find("Slot_img").gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Not enough gold for setting the Magic Tower");
                    _collider.transform.parent.gameObject.SetActive(false);
                }
            }
            else if (type == TowerType.Catapult)
            {
                if (GoldAmount >= Catapult.BuildPrice)
                {
                    GoldAmount -= Catapult.BuildPrice;
                    _collider.transform.gameObject.GetComponentInParent<Tower>().Type = TowerType.Catapult;
                    _collider.transform.gameObject.GetComponentInParent<Tower>().SetTower(TowerType.Catapult);
                    _collider.transform.parent.parent.gameObject.transform.Find("Catapult").gameObject.SetActive(true);
                    _collider.transform.parent.parent.Find("Slot_img").gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Not enough gold for setting the Catapult");
                    _collider.transform.parent.gameObject.SetActive(false);
                }
            }
            _collider.transform.gameObject.GetComponentInParent<Tower>().IsBought = true;
            _collider.transform.parent.gameObject.SetActive(false);
        }
        _collider.transform.gameObject.GetComponentInParent<CircleCollider2D>().enabled = true;
    }

    IEnumerator SellTower(TowerType type)  
    {
        while (_collider.transform.gameObject.GetComponentInParent<Tower>().IsBought) 
        {
            yield return new WaitForEndOfFrame();

            if(type == TowerType.WarriorTower) 
            {
                GoldAmount += WarriorTower.BuildPrice;
                _collider.transform.gameObject.GetComponentInParent<Tower>().Type = TowerType.None;
                _collider.transform.parent.parent.gameObject.transform.Find("Warrior Tower").gameObject.SetActive(false);
            }
            else if(type == TowerType.ArcheryTower) 
            {
                GoldAmount += ArcheryTower.BuildPrice;
                _collider.transform.gameObject.GetComponentInParent<Tower>().Type = TowerType.None;
                _collider.transform.parent.parent.gameObject.transform.Find("Archery Tower").gameObject.SetActive(false);
            }
            else if(type == TowerType.MagicTower) 
            {
                GoldAmount += MagicTower.BuildPrice;
                _collider.transform.gameObject.GetComponentInParent<Tower>().Type = TowerType.None;
                _collider.transform.parent.parent.gameObject.transform.Find("Magic Tower").gameObject.SetActive(false);
            }
            else if(type == TowerType.Catapult) 
            {
                GoldAmount += Catapult.BuildPrice;
                _collider.transform.gameObject.GetComponentInParent<Tower>().Type = TowerType.None;
                _collider.transform.parent.parent.gameObject.transform.Find("Catapult").gameObject.SetActive(false);
            }
            _collider.transform.gameObject.GetComponentInParent<Tower>().SetTower(TowerType.None);
            _collider.transform.gameObject.GetComponentInParent<Tower>().IsBought = false;
            _collider.transform.parent.gameObject.SetActive(false);
        }
        _collider.transform.parent.parent.Find("Slot_img").gameObject.SetActive(true);
        _collider.transform.gameObject.GetComponentInParent<CircleCollider2D>().enabled = true;
    }
}
