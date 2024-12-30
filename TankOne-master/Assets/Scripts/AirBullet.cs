using UnityEngine;

public class AirBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject particle;

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
            SoundManager.Instance.PlayAirBullet();
            Destroy(gameObject);
            Instantiate(particle,collision.GetContact(0).point,Quaternion.identity);

            if(collision.gameObject.tag == "Tank")
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.HitMe(15);
                CameraController.Instance.Shake();
            }

        }
    }
}
