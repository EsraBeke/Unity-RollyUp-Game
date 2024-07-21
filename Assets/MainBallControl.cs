using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBallControl : MonoBehaviour
{
    [SerializeField] float movementLimit;
    private void FixedUpdate()
    {
        if (GameManager.Instance.hasTheGameStart)
        {
            transform.Translate(Vector3.left * 5 * Time.fixedDeltaTime); // duz gidisati saglar

            // sadece saga sola gidisi dokunarak kontrol etmemiz lazim
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    float newDeltaPos = Mathf.Clamp(Input.touches[0].deltaPosition.x, -1, 1);
                    float newPos = transform.position.x + newDeltaPos * Time.fixedDeltaTime * 6;

                    newPos = Mathf.Clamp(newPos, -movementLimit, movementLimit);
                    transform.position = Vector3.Lerp(transform.position, new Vector3(newPos, transform.position.y, transform.position.z), 100f * Time.fixedDeltaTime);
                }
            }

        }
    }
}
