using UnityEngine;

public class RopeMover : MonoBehaviour
{
    public float speed = 3f;

    private void Update()
    {
        float direction = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            direction -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += 1f;
        }

        transform.position +=
            Vector3.right * direction * speed * Time.deltaTime;
    }
}