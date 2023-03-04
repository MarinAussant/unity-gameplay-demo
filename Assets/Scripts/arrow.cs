using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
        Vector2 arrowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - arrowPosition;
        transform.right = direction;

    }
}
