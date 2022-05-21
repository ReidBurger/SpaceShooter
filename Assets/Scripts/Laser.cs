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
            transform.Translate(new Vector3(0, 1, 0) * velocity * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
