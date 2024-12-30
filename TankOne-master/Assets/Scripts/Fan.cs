using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float speed = 20f;

    private float angle;

    private void Update()
    {
        angle += speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, -angle);
    }
}
