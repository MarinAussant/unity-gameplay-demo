using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterPropulse : MonoBehaviour
{

    [Header("Propusle fields")]

    [SerializeField][Tooltip("Put what prefab you want your character throw")] public GameObject propulseObject;
    [SerializeField][Tooltip("Arrow direction object")] public GameObject arrow;

    [SerializeField, Range(0f, 30f)][Tooltip("Force of thrown")] public float throwForce;
    [SerializeField, Range(0f, 60f)][Tooltip("Force of propulsing")] public float propulseForce;

    private SpriteRenderer arrowRenderer;
    private Rigidbody2D rb;

    private characterJump scriptJump;
    private characterMovement scriptMovement;

    [Header("Information")]

    public bool isThrowing;
    public bool isPropulsing;

    private void Awake(){

        arrowRenderer = arrow.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        scriptJump = GetComponent<characterJump>();
        scriptMovement = GetComponent<characterMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !isPropulsing && !isThrowing){
            
            isThrowing = true;
            Time.timeScale = 0.1f;
            arrowRenderer.enabled = true;

        }

        if (Input.GetMouseButtonUp(0) && isThrowing && !isPropulsing){
            
            isThrowing = false;
            Time.timeScale = 1f;
            arrowRenderer.enabled = false;
            ThrowPropulse();

        }

        if (!isThrowing && !isPropulsing && Input.GetMouseButtonDown(1)){

            isPropulsing = true;
            Time.timeScale = 0.1f;
            arrowRenderer.enabled = true;

        }

        if (!isThrowing && isPropulsing && Input.GetMouseButtonUp(1)){

            isPropulsing = false;
            Time.timeScale = 1f;
            arrowRenderer.enabled = false;
            Propulsing();

        }

    }

    private void ThrowPropulse(){

        GameObject newPropulse = Instantiate(propulseObject, arrow.transform.position, arrow.transform.rotation);
        newPropulse.GetComponent<Rigidbody2D>().velocity = arrow.transform.right * throwForce;

        Destroy(newPropulse,8f);

    }

    private void Propulsing(){

        Vector2 newGravity = new Vector2(0, (-2 * scriptJump.getJumpHeight()) / (scriptJump.getTimeToJumpApex() * scriptJump.getTimeToJumpApex()));
        rb.gravityScale = (newGravity.y / Physics2D.gravity.y);
        scriptJump.setPropolsing(true);
        //scriptMovement.setMaxSpeed(propulseForc);
        rb.velocity = arrow.transform.right * propulseForce;
        Debug.Log(arrow.transform.right * propulseForce);

    }
}
