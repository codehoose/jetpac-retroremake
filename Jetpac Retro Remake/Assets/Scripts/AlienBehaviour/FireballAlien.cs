using UnityEngine;

public class FireballAlien : MonoBehaviour
{
    public float yGradient = 1f;
    public float xDirection = 1f;
    public float speed = 16;
    public GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Player")
        {
            var copy = Instantiate(explosion);
            copy.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position += new Vector3(xDirection, yGradient) * speed * Time.deltaTime;

        if (Mathf.Abs(transform.position.x) > 185)
            Destroy(gameObject);
    }
}
