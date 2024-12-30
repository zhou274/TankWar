using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject explosionParticle;
    private Rigidbody2D rb;
    private bool isHit;
    private int demageAmount;
    [SerializeField]
    private Type type;
    public enum Type { ROCKET,DRONE,BIG,AIR_STRIKE}


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Rotate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isHit)
        {
            return;
        }
        
        if (collision.gameObject.tag == "Border")
        {
            isHit = true;
            GameController.GetInstance().ChangeTurn();
            Destroy(gameObject);
            return;
        }

        if (type == Type.ROCKET)
        {
            SoundManager.Instance.PlayGunBlust();
            CameraController.Instance.Shake();
            GameObject go = Instantiate(explosionParticle, collision.GetContact(0).point, Quaternion.identity);
            Destroy(go, 2);
            GameController.GetInstance().ChangeTurn();
            if (collision.gameObject.tag == "Tank")
            {

                Player player = collision.gameObject.GetComponent<Player>();
                player.HitMe(demageAmount);
            }
            else if (collision.gameObject.tag == "Wheel")
            {

                Player player = collision.gameObject.GetComponentInParent<Player>();
                if (player != null)
                    player.HitMe(demageAmount/2.0f);
            }
            isHit = true;
            
        }
        else if (type == Type.DRONE)
        {
            GameObject go = Instantiate(explosionParticle, collision.GetContact(0).point, Quaternion.identity);
            Destroy(go, 4);
            Drone.Instance.Fire(collision.GetContact(0).point);
        }
        else if (type == Type.AIR_STRIKE)
        {
            GameObject go = Instantiate(explosionParticle, collision.GetContact(0).point, Quaternion.identity);
            Destroy(go, 4);
            Drone.Instance.FireAirStrike(collision.GetContact(0).point);
        }
        else if (type == Type.BIG)
        {
            GameObject go = Instantiate(explosionParticle, collision.GetContact(0).point, Quaternion.identity);
            SoundManager.Instance.PlayBig();
            CameraController.Instance.Shake(2);
            GameController.Instance.ChangeTurn(2);
            if (collision.gameObject.tag == "Tank")
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.HitMe(demageAmount);
            }
        }
        Destroy(gameObject);
    }

    private void ChangeTurn()
    {
        GameController.Instance.ChangeTurn();
    }
    private void Rotate()
    {
        if (rb.velocity.magnitude > 0)
        {
            float angleZ = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;            

            transform.eulerAngles = new Vector3(0, 0, angleZ);
        }
    }
   
    public void Fire(Vector2 velocity, int demageAmount)
    {
        
        this.demageAmount = demageAmount+(GameData.playerState.powerMultiplier-1)*10;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }
}
