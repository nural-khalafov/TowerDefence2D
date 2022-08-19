using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Spawner Options")]
    public Waypoint TileSpawnPosition;
    public GameObject SpawnPosition;
    public float SpawnDelay = 2f;
    public List<GameObject> Enemies = new List<GameObject>();

    private List<GameObject> randomEnemies = new List<GameObject>();
    private Vector3 _spawnVector;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _spawnVector = TileSpawnPosition.transform.position;
        randomEnemies = GetRandomEnemyFromList<GameObject>(Enemies, 4);
    }

    public void SpawnEnemies() 
    {
        StartCoroutine(SpawnHanlder());
    }

    public static List<T> GetRandomEnemyFromList<T>(List<T> list, int number) 
    {
        List<T> tmpList = new List<T>(list);

        List<T> newList = new List<T>();

        while(newList.Count < number && tmpList.Count > 0) 
        {
            int index = Random.Range(0, tmpList.Count);
            newList.Add(tmpList[index]);
            tmpList.RemoveAt(index);
        }

        return newList;
    }

    IEnumerator SpawnHanlder() 
    {
        while (Time.time < 30) 
        {
            for(int i = 0; i < randomEnemies.Count; i++) 
            {
                //Instantiate(randomEnemies[i], SpawnPosition.transform);
                Instantiate(randomEnemies[i], _spawnVector, Quaternion.identity, SpawnPosition.transform);
            }
            yield return new WaitForSeconds(SpawnDelay);
        }
    }
}
