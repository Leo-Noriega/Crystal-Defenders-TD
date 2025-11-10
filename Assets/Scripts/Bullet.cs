using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 20f;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }
    }
}
