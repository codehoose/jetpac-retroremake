using UnityEngine;

public class FollowAlien : MonoBehaviour
{
    public GameObject explosion;

    private void Update()
    {
        if (RocketManager.Instance.State == RocketState.TakeOff) return;

        Transform player = GameManager.Instance.ThePlayer.transform;
        Vector3 diff = (player.transform.position - transform.position).normalized;
        transform.position += diff * 16f * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            ScoreManager.Instance.KillAlien();
            var copy = Instantiate(explosion);
            copy.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var copy = Instantiate(explosion);
            copy.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    internal void Init()
    {
        bool isRight = Random.Range(0f, 1f) > .5f;
        float x = isRight ? 180 : -180;
        float y = -88 + Random.Range(0f, 176);

        transform.position = new Vector3(x, y, 0);
        GetComponent<AlienInit>().flipAlien = isRight;

        //xDirection = isRight ? -1 : 1;
        //yGradient = -0.25f + Random.Range(0f, 0.5f);
        //speed = 16 + Random.Range(0, 4f);
    }
}
