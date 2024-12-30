using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public static Drone Instance;
    public GameObject prefBullet,airBullet;
    public float speed = 10f;
    public Transform target;

    private Vector2 hitPos;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireAirStrike(target.position);
        }
    }

    public void Fire(Vector2 hitPos)
    {
        this.hitPos = hitPos;
        StartCoroutine(IFire());
    }
    public void FireAirStrike(Vector2 hitPos)
    {
        this.hitPos = hitPos;
        StartCoroutine(IAirStrike());
    }

    private IEnumerator IFire()
    {

        transform.position = new Vector2(hitPos.x,transform.position.y);
        SoundManager.Instance.DroneShot();
        float speed = 10f;
        yield return new WaitForSeconds(0.5f);        
        for(int i = 0; i < 15; i++)
        {
            DroneBullet bullet = Instantiate(prefBullet, transform.position, Quaternion.identity).GetComponent<DroneBullet>();
            Vector2 targetPos = new Vector2(hitPos.x-5+0.8f*i, hitPos.y);            
            Vector2 dir = (targetPos-(Vector2)transform.position).normalized;
            Vector2 velocity = dir * speed;
            bullet.Fire(velocity);
            yield return new WaitForSeconds(0.2f);

        }
        yield return new WaitForSeconds(3);
        GameController.GetInstance().ChangeTurn();
    }

    private IEnumerator IAirStrike()
    {
        transform.position = new Vector2(hitPos.x, transform.position.y);
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < 8; i++)
        {
            Vector2 pos = transform.position;
            pos.x += -3 + i * 1;

            AirBullet bullet = Instantiate(airBullet, pos, Quaternion.identity).GetComponent<AirBullet>();
            bullet.Fire(Vector2.down*speed);
        }
        yield return new WaitForSeconds(3);
        GameController.GetInstance().ChangeTurn();
    }
}
