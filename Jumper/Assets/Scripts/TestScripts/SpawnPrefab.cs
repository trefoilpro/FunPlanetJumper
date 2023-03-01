using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    [SerializeField] private GameObject _trianglePrefab;
    [SerializeField] private List<GameObject> _triangleList;

    public void SpawnTriangle()
    {
        GameObject triangle = Instantiate(_trianglePrefab, transform.position, Quaternion.identity, transform);
        _triangleList.Add(triangle);
    }
}
