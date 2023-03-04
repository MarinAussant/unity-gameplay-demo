using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterGround : MonoBehaviour
{

    private bool onGround;
    
    private characterJump scriptJump;
    private characterMovement scriptMovement;

    [Header("Collider Settings")]
    [SerializeField] private float groundLength = 0.95f;
    [SerializeField] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;

    private void Awake(){

        scriptJump = GetComponent<characterJump>();
        scriptMovement = GetComponent<characterMovement>();

    }

    // Update is called once per frame
    private void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if(onGround){
            scriptJump.setPropolsing(false);
            scriptMovement.setMaxSpeed(9);
        }

    }

    private void OnDrawGizmos() {
        //Draw the ground colliders on screen for debug purposes
        if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    //Send ground detection to other scripts
    public bool GetOnGround() { return onGround; }

}
