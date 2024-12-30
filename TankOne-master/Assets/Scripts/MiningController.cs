using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiningController : MonoBehaviour
{


    public float castDistance = 3.0f; //How far we will cast
    public Transform raycastPoint; //The origin point of our cast
    public LayerMask layer; //The layer we want to cast onto

    float blockDestroyTime = 1.0f; //How long to delay between tile destructions

    Vector3 direction;
    RaycastHit2D hit;
    bool destroyingBlock = false;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            RaycastDirection();
        }
    }

    void RaycastDirection()
    {
        //Only update our direction if we have new inputs
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            direction.x = Input.GetAxis("Horizontal");
            direction.y = Input.GetAxis("Vertical");
        }

        //Raycast in the direction we are facing
        hit = Physics2D.Raycast(raycastPoint.position, direction, castDistance, layer.value);

        //Calculate our end position
        Vector2 endpos = raycastPoint.position + direction;

        //Draw a line so we can see the direction
        Debug.DrawLine(raycastPoint.position, endpos, Color.red);

        //Check if we are colliding with something
        if (hit.collider && !destroyingBlock)
        {
            //Destroy the tile at the endpos, on the tilemap that we collided with
            destroyingBlock = true;
            StartCoroutine(DestroyBlock(hit.collider.gameObject.GetComponent<Tilemap>(), endpos));
        }

    }

    IEnumerator DestroyBlock(Tilemap map, Vector2 pos)
    {
        //Wait a second or two, so that we aren't constantly destroying blocks
        yield return new WaitForSeconds(blockDestroyTime);

        //Floor the floats so that we dont destroy the wrong tile
        pos.y = Mathf.Floor(pos.y);
        pos.x = Mathf.Floor(pos.x);

        //Set the tile to null
        map.SetTile(new Vector3Int((int)pos.x, (int)pos.y, 0), null);

        //We are no longer destroying a block
        destroyingBlock = false;
    }
}