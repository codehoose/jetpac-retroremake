using UnityEngine;

public class Locomotion : MonoBehaviour
{
    private static readonly float DROP_ZONE_X = 44f;

    private Rigidbody2D _rb;

    private float _maxXSpeed = 32f;
    private float _maxXAirSpeed = 64f;
    private float _maxYSpeed = 64f;
    private float _verticalMultiplier = 16f;
    private float _horizontalMuliplier = 16f;

    private int _nextPartId = 1;
    private GameObject _currentPart;
    private GameObject _currentFuelCell;

    public GameObject cloud;
    public GameObject shape;
    public JetmanAnimation jetmanShape;
    public JetmanShadow shadow;

    private Vector2 _velocity;
    private float _gravity = 0f;
    private bool _applyGravity = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        shape.SetActive(false);
    }

    public void ResetPartRequired() => _nextPartId = 1;

    private void Update()
    {
        if (RocketManager.Instance.State == RocketState.Landing)
        {
            return;
        }

        if (!shape.activeInHierarchy) shape.SetActive(true);

        var horiz = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        vert = vert > 0 ? vert : 0f;

        if (vert != 0)
            _gravity = 0f;

        if (horiz == 0 && vert == 0)
        {
            jetmanShape.Idle();
        }
        else
        {
            jetmanShape.FlipHorizontal = horiz < 0;
            jetmanShape._isAnimating = true;
        }

        _velocity += new Vector2(horiz * _horizontalMuliplier, vert * _verticalMultiplier);

        var maxXSpeed = jetmanShape._isInflight ? _maxXAirSpeed : _maxXSpeed;
        if (Mathf.Abs( _velocity.x) > maxXSpeed)
            _velocity.x = maxXSpeed * Mathf.Sign(_velocity.x);

        if (Mathf.Abs(_velocity.y) > _maxYSpeed)
            _velocity.y = _maxYSpeed * Mathf.Sign(_velocity.y);

        if (horiz == 0f)
            _velocity.x = 0f;

        if (vert == 0f)
            _velocity.y = 0f;

        if (_velocity.y == 0 && _applyGravity)
            _gravity += -_maxXAirSpeed * Time.deltaTime;

        _velocity.y += _gravity;
        _rb.velocity = _velocity;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Alien")
        {
            GameManager.Instance.PlayerDied();
        }
        else if (collision.gameObject.tag == "Platform")
        {
            if (gameObject.transform.position.y > collision.gameObject.transform.position.y)
            {
                jetmanShape.Idle();
                jetmanShape._isInflight = false;
                _applyGravity = false;
                _gravity = 0;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            var copy = Instantiate(cloud);
            copy.transform.position = gameObject.transform.position - new Vector3(0, 8, 0);
            jetmanShape._isInflight = true;
            _applyGravity = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "RocketPart")
        {
            PickupRocketPart(collision.gameObject);
        }
        else if (collision.tag == "Pickup")
        {
            var root = collision.gameObject.transform.root.gameObject;
            var pickup = root.GetComponent<Pickup>();
            pickup._pickedUp = true;
            DropManager.Instance.PickupObject(root);
            ScoreManager.Instance.PickUpGem();
        }
        else if (collision.tag == "Fuel")
        {
            var root = collision.gameObject.transform.root.gameObject;
            PickupFuel(root);
            ScoreManager.Instance.PickUpFuel();
        }
        else if (collision.tag == "LeftSideBlock")
        {
            shadow.SetPlayer(this, new Vector3(256, 0));
        }
        else if (collision.tag == "RightSideBlock")
        {
            shadow.SetPlayer(this, new Vector3(-256, 0));
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "RightSideBlock")
        {
            if (transform.position.x > collision.gameObject.transform.position.x)
            {
                var worldPosition = shadow.transform.position;
                transform.position = worldPosition;
                shadow.SetPlayer(null, Vector3.zero);
            }
            else
            {
                shadow.SetPlayer(null, Vector3.zero);
            }
        }
        else if (collision.tag == "LeftSideBlock")
        {
            if (transform.position.x < collision.gameObject.transform.position.x)
            {
                var worldPosition = shadow.transform.position;
                transform.position = worldPosition;
                shadow.SetPlayer(null, Vector3.zero);
            }
            else
            {
                shadow.SetPlayer(null, Vector3.zero);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "DropZone"
            && Mathf.Abs(transform.position.x - collision.transform.position.x) < 2)
        {
            if (_currentPart != null)
            {
                DropItem(_currentPart);
                RocketManager.Instance.DropPart(_currentPart,
                                                _currentPart.GetComponent<RocketPart>().partId);

                _currentPart = null;
                _nextPartId--;
            } else if (_currentFuelCell != null)
            {
                DropItem(_currentFuelCell);

                _currentFuelCell.GetComponent<BoxCollider2D>().tag = "Dropped Fuel";
                _currentFuelCell.GetComponent<BoxCollider2D>().enabled = true;
                _currentFuelCell.GetComponent<Rigidbody2D>().isKinematic = false;
                _currentFuelCell = null;
            }
        }
    }

    private void DropItem(GameObject go)
    {
        go.transform.SetParent(null);
        go.transform.position = new Vector3(DROP_ZONE_X, go.transform.position.y);
    }

    private void PickupRocketPart(GameObject rocketPart)
    {
        if (rocketPart.GetComponent<RocketPart>().partId != _nextPartId) return;

        rocketPart.transform.SetParent(transform);
        rocketPart.transform.localPosition = Vector3.zero;
        rocketPart.GetComponent<BoxCollider2D>().enabled = false;
        _currentPart = rocketPart;
    }

    private void PickupFuel(GameObject fuel)
    {
        if (_currentFuelCell != null) return;

        fuel.transform.SetParent(transform);
        fuel.transform.localPosition = Vector3.zero;
        fuel.GetComponent<BoxCollider2D>().enabled = false;
        fuel.GetComponent<Rigidbody2D>().isKinematic = true;
        _currentFuelCell = fuel;
    }
}
