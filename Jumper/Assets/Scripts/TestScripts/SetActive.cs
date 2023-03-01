using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    [SerializeField] private GameObject _huj;

    // Start is called before the first frame update
    void Start()
    {
        _huj.gameObject.SetActive(true);

        //StartCoroutine(Coroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
        }
        
    }
}
