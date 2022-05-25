using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float velocity = 30;
    private float upBound = 10;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < upBound)
        {
            transform.Translate(Vector3.up * velocity * Time.deltaTime);
        }
        else
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
