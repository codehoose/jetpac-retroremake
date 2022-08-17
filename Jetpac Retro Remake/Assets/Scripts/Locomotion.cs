using UnityEngine;

public class Locomotion : MonoBehaviour
{
    private static readonly float DROP_ZONE_X = 44f;

    private Rigidbody2D _rb;

    private float _verticalMultiplier = 5f;
    private float _horizontalMuliplier = 5f;

    private int _nextPartId = 1;
    private GameObject _currentPart;
    private GameObject _currentFuelCell;

    public GameObject shape;

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

        _rb.AddForce(new Vector2(horiz * _horizontalMuliplier, vert * _verticalMultiplier));
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
