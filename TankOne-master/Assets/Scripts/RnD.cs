using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RnD : MonoBehaviour
{
    public Transform target;
    public GameObject buletPref;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(buletPref, transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().isKinematic = false;
            go.GetComponent<Rigidbody2D>().velocity = Calculate(target);
        }
    }


    private Vector3 Calculate(Transform target)
    {
        float gravity = -9.8f;
        float h = 4;
        float displacementY = target.position.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - transform.position.x,0, 0);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;
        return velocityXZ + velocityY;
    }
}
