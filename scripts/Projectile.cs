using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
	Vector2 direction;
	float force; 
	
	// This is because Unity doesn’t run Start when you create the object, but on the next frame. So when you call Launch on your projectile, just Instantiate and don’t call Start, so your Rigidbody2d is still empty. To fix that, rename the void Start() function in the Projectile script to void Awake().
	
	//Contrary to Start, Awake is called immediately when the object is created (when Instantiate is called)
    
	void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
     Destroy( this.gameObject , 1f );   
    }
	
	public void Launch( Vector2 direction , float force ){
		rigidBody2D.AddForce( direction * force );
	}
	
	void OnCollisionEnter2D( Collision2D other ){
		Debug.Log(" Projectile hit -" + other.gameObject);
		EnemyController enC = other.collider.GetComponent<EnemyController>();
		
		if( enC != null ){
			enC.Fix();
		}
		Destroy(gameObject);
	}
}
