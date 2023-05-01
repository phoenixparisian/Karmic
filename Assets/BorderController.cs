using UnityEngine;

public class BorderController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerPosition = other.transform.position;

            float x = Mathf.Clamp(playerPosition.x, GetLeftBoundary(), GetRightBoundary());
            float y = Mathf.Clamp(playerPosition.y, GetBottomBoundary(), GetTopBoundary());

            other.transform.position = new Vector3(x, y, playerPosition.z);
        }
    }

    private float GetLeftBoundary()
    {
        float leftBoundary = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        return leftBoundary;
    }

    private float GetRightBoundary()
    {
        float rightBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        return rightBoundary;
    }

    private float GetTopBoundary()
    {
        float topBoundary = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        return topBoundary;
    }

    private float GetBottomBoundary()
    {
        float bottomBoundary = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        return bottomBoundary;
    }
}