using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Stay used to keed damaging as long as she's in the zone
	void OnTriggerStay2D( Collider2D other  ){
		
		RubyController rController = other.GetComponent<RubyController>();
		if( null != rController ){
			if( rController.health != 0 ){
				rController.updateHealth(-1);
				//Destroy(gameObject);
			}
		}
		
	}
}
