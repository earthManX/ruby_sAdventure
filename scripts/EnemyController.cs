using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	Rigidbody2D rigidBody2D;
    Vector2 position ;
	Animator animator;
	
	[SerializeField]
	private float speed;
	[SerializeField]
	private float timer ;
	private float changeTime = 1.5f ; 
	int patrolDirection ;
	
	float animDirection;
	
    private int direction = 1 ; 
	private Vector2 tempPos;
	private int count = 0;
	
	public ParticleSystem smokeEffect;
	public ParticleSystem hitEffect;
	
	bool isBroken;
	
	void Start()
    {
		rigidBody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		isBroken = true ;
		position = rigidBody2D.position;
		speed = 2f;
		timer = changeTime ;
		patrolDirection = 0 ;
		//direction =  1;
		tempPos = position; 		
    }

    // Update is called once per frame
    void Update()
    {
		if(!isBroken){
			return;
		}
		if( count == 2 ){
			patrolDirection = Random.Range( -2  , 3 );
			animDirection = patrolDirection;
			//if( patrolDirection == 0){
				//patrolDirection = 1 ; 
			//}
			count = 0 ;
		}
		
		timer -= Time.deltaTime;
        
		if( timer > 0 ){
			enemyMovement( patrolDirection );
		}else{
			// Here we are just chnging direction as we want to take the enemy back to starting point
			//enemyMovement(-1);
			direction = -direction;
			animDirection = -patrolDirection;
			animator.SetFloat("patrolDirection", animDirection  );
			//enemyMovement(patrolDirection);
			//patrolDirection = Random.Range( 0 , 4 ) ;
			timer = changeTime ;
			count++;
			//tempPos = position;
		}
    }
	
	void enemyMovement( int patrolDirection ){
		
		switch( patrolDirection ){
			case 1 :
				tempPos += new Vector2( 0 , speed * Time.deltaTime * direction);
				rigidBody2D.MovePosition( tempPos);
				//rigidBody2D.MovePosition( position - new Vector2( 0 , speed* Time.deltaTime ) );
				break;
			case -1 :
				tempPos +=  new Vector2( 0 , speed * Time.deltaTime  * -1 * direction );
				rigidBody2D.MovePosition( tempPos );
				//rigidBody2D.MovePosition( position - new Vector2( 0 , speed* Time.deltaTime  * -1 )  );
				break;
			case 2 :
				tempPos += new Vector2( speed* Time.deltaTime * direction  , 0 );
				rigidBody2D.MovePosition( tempPos );
				//rigidBody2D.MovePosition( position - new Vector2( speed* Time.deltaTime , 0 )  );
				break;
			case -2 :
				tempPos += new Vector2( speed* Time.deltaTime * -1 * direction , 0);
				rigidBody2D.MovePosition( tempPos );
				//rigidBody2D.MovePosition( position - new Vector2( speed* Time.deltaTime * 2f * -1 , 0  )  );
				break;
			//default :
				//rigidBody2D.MovePosition( position );
				//break;
		}
		animator.SetFloat("patrolDirection", animDirection  );
	}
	
	void OnCollisionEnter2D( Collision2D other){ // Collision2D is used, not Collider2D
		RubyController rController = other.gameObject.GetComponent<RubyController>();
		
		if( rController != null ){
			rController.updateHealth(-1);
		}
	}
	
	public void Fix(){
		isBroken = false;
		rigidBody2D.simulated = false ;
		animator.enabled = false ;
		smokeEffect.Stop();
		Instantiate(hitEffect , rigidBody2D.position , Quaternion.identity);
		
		
	}
}
