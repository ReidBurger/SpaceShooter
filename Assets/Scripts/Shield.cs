using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.GetComponentInParent<Transform>().position;
    }
}
