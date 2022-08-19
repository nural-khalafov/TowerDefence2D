using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _firstPath;
    //[SerializeField] private List<Waypoint> _secondPath = new List<Waypoint>();
    [SerializeField] [Range(0f, 5f)] public float _speed = 1f;

    private void Start()
    {
        _firstPath = GameManager.Instance._firstPath;
        StartCoroutine(FollowWaypoints());
    }

    IEnumerator FollowWaypoints() 
    {
        foreach(Waypoint w in _firstPath) 
        {
            Vector2 startPosition = transform.position;
            Vector3 endPosition = w.transform.position;
            float travelPercent = 0f;

            transform.right = endPosition - transform.position;

            while (travelPercent < 1f)
            {
                travelPercent += _speed * Time.deltaTime;
                transform.position = Vector2.Lerp(startPosition, endPosition, travelPercent);

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
