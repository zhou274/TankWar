using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject particle;

    public void Fire(Vector2 velocity)
    {
        rb.isKinematic = false;        
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward * angle;
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision != null)
        {
            Destroy(gameObject);
            Instantiate(particle,collision.GetContact(0).point,Quaternion.identity);
            if (collision.gameObject.tag == "Tank")
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.HitMe(15);
                CameraController.Instance.Shake();
            }
            else if (collision.gameObject.tag == "Wheel")
            {

                Player player = collision.gameObject.GetComponentInParent<Player>();
                if (player != null)
                    player.HitMe(10);
            }

            
        }
    }
}
