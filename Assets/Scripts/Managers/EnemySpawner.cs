using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Spawner Options")]
    public bool IsSpawnerActive = false;
    public Waypoint TileSpawnPosition;
    public GameObject SpawnPosition;
    public float SpawnDelay = 2f;
    public int SpawnWaves = 5;
    public float WaveDuration = 30;

    public int CurrentWave = 0;

    [Header("Enemy List")]
    public List<GameObject> Enemies = new List<GameObject>();

    [Header("Timer Options")]
    public float CurrentTimer;

    private List<GameObject> randomEnemies = new List<GameObject>();
    private Vector3 _spawnVector;
    private IEnumerator SpawnerCoroutine;

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

        CurrentTimer = WaveDuration;
        _spawnVector = TileSpawnPosition.transform.position;
        randomEnemies = GetRandomEnemyFromList<GameObject>(Enemies, 4);
        SpawnerCoroutine = SpawnHanlder();
    }

    IEnumerator Start() 
    {
        if (IsSpawnerActive)
        {
            // where i is current wave
            while(CurrentWave < SpawnWaves) 
            {
                CurrentWave++;

                StartCoroutine(SpawnerCoroutine);

                yield return new WaitForSeconds(WaveDuration);

                
            }

            if (CurrentWave == SpawnWaves)
            {
                IsSpawnerActive = false;

                SpawnerCoroutine = null;

                yield break;
            }

            //for(int i = CurrentWave; i <= SpawnWaves; i++) 
            //{
            //    CurrentWave = i;

            //    if(CurrentWave != SpawnWaves) 
            //    {
            //        StartCoroutine(SpawnHanlder());

            //        yield return new WaitForSeconds(WaveDuration);
            //    }
            //    else if(CurrentWave == SpawnWaves)
            //    {
            //        StopCoroutine(SpawnHanlder());

            //        yield return new WaitForEndOfFrame();
            //    }

            //}
        }
        else 
        {
            StopCoroutine(SpawnerCoroutine);
        }
    }

    private void Update()
    {
        CurrentTimer -= Time.deltaTime;

        if(CurrentTimer <= 0) 
        {
            CurrentTimer = WaveDuration;
        }
    }

    //IEnumerator Start() 
    //{
    //    if (IsSpawnerActive) 
    //    {
    //        while (CurrentWave <= SpawnWaves)
    //        {
    //            StartCoroutine(SpawnHanlder());

    //            yield return new WaitForSeconds(WaveDuration);
    //        }
    //    }
    //}

    public static List<T> GetRandomEnemyFromList<T>(List<T> list, int number)
    {
        List<T> tmpList = new List<T>(list);

        List<T> newList = new List<T>();

        while (newList.Count < number && tmpList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, tmpList.Count);
            newList.Add(tmpList[index]);
            tmpList.RemoveAt(index);
        }

        return newList;
    }

    IEnumerator SpawnHanlder()
    {
        while (Time.deltaTime < WaveDuration)
        {
            for (int i = 0; i < randomEnemies.Count; i++)
            {
                Instantiate(randomEnemies[i], _spawnVector, Quaternion.identity, SpawnPosition.transform);
            }
            yield return new WaitForSeconds(SpawnDelay);
        }
    }
}
