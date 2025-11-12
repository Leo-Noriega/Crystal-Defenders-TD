using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 20f;
    private float timer;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        timer = 0f;
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * speed;
        }
    }
    private void OnDisable()
    {
        timer = 0f;
        if (rb != null) rb.linearVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null)
        {
            transform.position += direction.normalized * (speed * Time.deltaTime);
        }
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }
    }
}
