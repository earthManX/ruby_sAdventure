using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
	public int maxHealth = 5 ; 
	// Shortcut for get function
	public int health{ get { return currentHealth; } }
	[SerializeField]
	private int currentHealth; 
	
	Rigidbody2D rigidBody2D;
	Animator animator;
	Vector2 lookDIrection = new Vector2(1,0);
	
	private float invincibleTimer;
	private float invincibleTime = 2f;
	[SerializeField]
	private bool isInvincible = false ;
	
	public GameObject projectilePrefab;
    
	void Start()
    {
     rigidBody2D = GetComponent<Rigidbody2D>();
	 animator = GetComponent<Animator>();
     currentHealth = maxHealth ; 
	}

    
    void Update()
    {
        float sideMovement = Input.GetAxis("Horizontal");
		float verticalMovement = Input.GetAxis("Vertical");
		
		if( Input.GetKeyDown( KeyCode.X )){
			RaycastHit2D hit = Physics2D.Raycast( rigidBody2D.position + Vector2.up *  0.2f , lookDIrection , 1.5f , LayerMask.GetMask("NPC"));
			Debug.Log("W");
			if( hit.collider != null) {
				NonPlayableCharacter npc = hit.collider.GetComponent<NonPlayableCharacter>();
				Debug.Log("Waz");
				if( npc != null ){
					
					npc.displayDialog();
					Debug.Log("Wazzup");
				}
			}				
		}
		
		
		Vector2 move = new Vector2( sideMovement , verticalMovement );
		Vector2 position = rigidBody2D.position;
		
		// Checking if idle approximately
	if( !Mathf.Approximately( move.x , 0.0f ) || !Mathf.Approximately( move.y , 0.0f ) ){
		lookDIrection.Set( move.x , move.y );
		lookDIrection.Normalize(); // Normalize makes length = 1 , thus effectively storing direction
	}
	
		animator.SetFloat("Look X", lookDIrection.x );
		animator.SetFloat("Look Y" , lookDIrection.y );
		animator.SetFloat("Speed" , move.magnitude ); // Boom, vector magnitude. Speed here is the length of the move vector 
		
		position = position + 2f * move * Time.deltaTime;
		// Here we are using rigidbody rather than transform to make the Physics system track the rigidBosy rather than the transform
		// to make charater collisions look smooth
		rigidBody2D.MovePosition(position);
		
		if( isInvincible ){
				invincibleTimer -= Time.deltaTime;
				if( invincibleTimer < 0 ){
					isInvincible = false; 
				}
		}
		
		// Launch projectile 
		if(Input.GetKeyDown( KeyCode.C)){
			Launch();
		}
    }
	
	public void updateHealth( int amount ){
		// Basically setting limits;
		
		UIHealthBar.instance.SetValue( currentHealth / (float) maxHealth );
		if( amount < 0 ){
			
			if(isInvincible){
				return;
			}
			
			animator.SetTrigger("Hit");
			isInvincible = true;
			invincibleTimer = invincibleTime;
			
			}
			currentHealth = Mathf.Clamp( currentHealth + amount, 0 , maxHealth );
		}
		
	void Launch(){
		GameObject projectileObject = Instantiate( projectilePrefab  , rigidBody2D.position + Vector2.up * 0.5f , Quaternion.identity );
		
		Projectile projectile = projectileObject.GetComponent<Projectile>();
		projectile.Launch( lookDIrection , 300 );
		
		animator.SetTrigger("Launch");
		
	}
		
	}

