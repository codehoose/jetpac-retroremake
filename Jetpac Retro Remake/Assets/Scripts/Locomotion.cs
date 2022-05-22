using UnityEngine;

public class Locomotion : MonoBehaviour
{
    private static readonly float DROP_ZONE_X = 44f;

    private Rigidbody2D _rb;

    private float _verticalMultiplier = 5f;
    private float _horizontalMuliplier = 5f;

    private int _nextPartId = 1;
    private GameObject _currentPart;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ResetPartRequired() => _nextPartId = 1;

    private void Update()
    {
        var horiz = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        vert = vert > 0 ? vert : 0f;

        _rb.AddForce(new Vector2(horiz * _horizontalMuliplier, vert * _verticalMultiplier));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "RocketPart")
        {
            PickupRocketPart(collision.gameObject);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "DropZone"
            && _currentPart != null
            && Mathf.Abs(transform.position.x - collision.transform.position.x) < 2)
        {
            _currentPart.transform.SetParent(null);
            _currentPart.transform.position = new Vector3(DROP_ZONE_X, _currentPart.transform.position.y);
            RocketManager.Instance.DropPart(_currentPart,
                                            _currentPart.GetComponent<RocketPart>().partId);

            _currentPart = null;
            _nextPartId--;
        }
    }

    private void PickupRocketPart(GameObject rocketPart)
    {
        if (rocketPart.GetComponent<RocketPart>().partId != _nextPartId) return;

        rocketPart.transform.SetParent(transform);
        rocketPart.transform.localPosition = Vector3.zero;
        rocketPart.GetComponent<BoxCollider2D>().enabled = false;
        _currentPart = rocketPart;
    }
}
