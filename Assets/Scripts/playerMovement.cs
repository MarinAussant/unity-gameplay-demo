using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{   

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask jumpableGround;


    private float dirX = 0f;
    private Vector2 desiredVelocity;
    private float maxSpeedChange;

    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float acceleration = 100f;
    [SerializeField] private float deceleration = 100f;
    [SerializeField] private float turnSpeed = 100f;



    private bool desiredJump = false;
    [SerializeField] private float maxJump = 7f;
    

    private enum MovementState {idle, running, jumping};

    [SerializeField] private GameObject propulse;
    private enum PropulseDirection {rien, gauche, gaucheBas, bas, droiteBas, droite, droiteHaut, haut, gaucheHaut};
    PropulseDirection propulseDirection;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void OnMovement()
    {
        //This is called when you input a direction on a valid input type, such as arrow keys or analogue stick
        //The value will read -1 when pressing left, 0 when idle, and 1 when pressing right.
        dirX = UnityEngine.Input.GetAxisRaw("Horizontal");

    }

    // Update is called once per frame
    private void Update()
    {
        
        OnMovement();

        if (Input.GetButton("Propulse")){

            rb.velocity = new Vector2(0,0);
            propulseDirection = ShowDirection();
        }
        else {
            
            if (Input.GetButtonUp("Propulse")){
                ThrowPropulse(propulseDirection);
            }

            desiredVelocity = new Vector2(dirX, 0f) * maxSpeed;

            if (Input.GetButtonDown("Jump") && IsOnGround()){
                rb.velocity = new Vector2(rb.velocity.x,maxJump);
            }



            /*
            if (Input.GetButtonUp("Jump") && rb.velocity.y > .1f){
                rb.velocity = new Vector2(rb.velocity.x,0);
            }
            */

            UpdateAnimationState();
        }
        

        if (Input.GetButtonDown("Restart")){
            RestartLevel();
        }
        
    }

    private void FixedUpdate() {
        
        if (dirX != 0){
            if (Mathf.Sign(dirX) != Mathf.Sign(rb.velocity.x)){
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else {
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else {
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, desiredVelocity.x, maxSpeedChange),rb.velocity.y);

    }

    private void UpdateAnimationState(){

        MovementState value;

        if (dirX > 0f){
            value = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f){
            value = MovementState.running;
            sprite.flipX = true;
        }
        else {
            value = MovementState.idle;
        }

        if (rb.velocity.y > .1f){
            value = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f){
            value = MovementState.idle;
        }

        anim.SetInteger("state",(int)value);
    }

    private bool IsOnGround(){

        return Physics2D.BoxCast(coll.bounds.center,coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

    }

    private void RestartLevel(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private PropulseDirection ShowDirection(){

        PropulseDirection direction = PropulseDirection.rien;

        if ((Input.GetAxisRaw("Horizontal") > 0f && Input.GetButton("Horizontal")) && !Input.GetButton("Vertical")){
            direction = PropulseDirection.droite;
        }
        if ((Input.GetAxisRaw("Horizontal") < 0f && Input.GetButton("Horizontal")) && !Input.GetButton("Vertical")){
            direction = PropulseDirection.gauche;
        }
        if (!Input.GetButton("Horizontal") && (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0f)){
            direction = PropulseDirection.haut;
        }
        if (!Input.GetButton("Horizontal") && (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0f)){
            direction = PropulseDirection.bas;
        }
        if ((Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0f) && (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0f)){
            direction = PropulseDirection.droiteHaut;
        }
        if ((Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0f) && (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0f)){
            direction = PropulseDirection.gaucheHaut;
        }
        if ((Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0f) && (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0f)){
            direction = PropulseDirection.droiteBas;
        }
        if ((Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0f) && (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0f)){
            direction = PropulseDirection.gaucheBas;
        }

        return direction; // CA CA MARCHE

    }

    private void ThrowPropulse(PropulseDirection direction){

        GameObject tempPropulse = Instantiate(propulse);
        tempPropulse.transform.position = new Vector2(transform.position.x,transform.position.y);

        Rigidbody2D propulseRB = tempPropulse.GetComponent<Rigidbody2D>();

        Vector2 vectorDirection = new Vector2(0f,0f);

        if ((int)direction == 1){
            vectorDirection = new Vector2(-6f, 9f);
        }
        if ((int)direction == 5){
            vectorDirection = new Vector2(6f, 9f);
        }
        if ((int)direction == 7){
            vectorDirection = new Vector2(0f, 12f);
        }
        if ((int)direction == 3){
            vectorDirection = new Vector2(0f,-9f);
        }
        if ((int)direction == 6){
            vectorDirection = new Vector2(3f, 11f);
        }
        if ((int)direction == 4){
            vectorDirection = new Vector2(3f,5f);
        }
        if ((int)direction == 8){
            vectorDirection = new Vector2(-3f, 11f);
        }
        if ((int)direction == 2){
            vectorDirection = new Vector2(-3f,5f);
        }

        /*
        if (sprite.flipX){
            propulseRB.velocity = new Vector2(-2f + (rb.velocity.x / 3),5f + (rb.velocity.y / 2));
        }
        else {
            propulseRB.velocity = new Vector2(2f + (rb.velocity.x / 3),5f + (rb.velocity.y / 2));
        }
        */

        propulseRB.velocity = vectorDirection;
        Destroy(tempPropulse,2.5f);

    }
}
