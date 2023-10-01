using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowBehavior : MonoBehaviour
{

    [SerializeField] private GameObject chest;

    private Transform _objTransform;

    // Start is called before the first frame update
    void Start()
    {
        _objTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        PointToObject(chest);
    }

    private void PointToObject(GameObject targetGameObject)
    {
        var targetVect = targetGameObject.transform.position - _objTransform.position;
        _objTransform.up = -targetVect;
    }
    
    
}
