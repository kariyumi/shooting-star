using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float Speed = 6f;

    private void Update()
    {
        transform.Translate(Speed * Time.deltaTime * Vector3.up);

        if (transform.position.y >= Camera.main.orthographicSize)
        {
            Destroy(gameObject);
        }
    }
}
