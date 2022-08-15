using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateHandler : MonoBehaviour
{
    [SerializeField] private TextMeshPro _label;

    private Vector2Int _coordinates;

    private void Awake()
    {
        _label = GetComponentInChildren<TextMeshPro>();
        DisplayCoordinates();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
        }
    }

    private void DisplayCoordinates() 
    {
        _coordinates.x = Mathf.RoundToInt(transform.position.x / 2);
        _coordinates.y = Mathf.RoundToInt(transform.position.y / 2);

        _label.text = $"{_coordinates.x}, {_coordinates.y}";
    }
}
