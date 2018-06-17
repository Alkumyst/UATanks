using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup {											//powerup class

	public string displayName;									//powerup display name
	public float lifespan = 1.5f;

	public virtual void ApplyPowerup (TankData data) {			//parent ApplyPowerup function
		Debug.Log (displayName + " powerup applied!");			//debug log notification
	}

	public virtual void RemovePowerup (TankData data) {			//parent RemovePowerup function
		Debug.Log (displayName + " powerup removed!");			//debug log notification
	}

}

[System.Serializable]
public class HealthPowerup : Powerup {							//health powerup class
	public int healthToAdd;										//health to add
	public bool clampToMax;										//max health clamp
	public override void ApplyPowerup (TankData data) {					//health powerup child ApplyPowerup function
		base.ApplyPowerup(data);										//run function in parent class
		data.health += healthToAdd;										//add to health
		if (clampToMax) {												
			data.health = Mathf.Clamp (data.health, 0, data.maxHealth);	//clamp to max health
		}
	}
	public override void RemovePowerup (TankData data) {		//health powerup child RemovePowerup function
		base.RemovePowerup(data);								//run function in parent class
	}
}

[System.Serializable]
public class DefencePowerup : Powerup {							//defence powerup class
	public int defenceBoost;									//defence boost
	public override void ApplyPowerup (TankData data) {			//defence powerup child ApplyPowerup function
		if (data.damageToTake >0) {								//if damageToTake is not 0 or under	
			base.ApplyPowerup(data);							//run function in parent class
			data.damageToTake -= defenceBoost;					//decrease damageToTake by defenceBoost
		}
	}
	public override void RemovePowerup (TankData data) {		//defence powerup child RemovePowerup function
		base.RemovePowerup(data);								//run function in parent class
		data.damageToTake += defenceBoost;						//increase damageToTake by defenceBoost
	}
}

[System.Serializable]
public class SpeedPowerup : Powerup {							//seed powerup class
	public int speedBoost;										//speed boost to add
	public override void ApplyPowerup (TankData data) {			//speed powerup child ApplyPowerup function
		base.ApplyPowerup (data);								//run function in parent class
		data.forwardSpeed += speedBoost;						//add speed boost to forwardSpeed
		data.reverseSpeed += speedBoost;						//add speed boost to reverseSpeed
		//data.turnSpeed += speedBoost;							//may re-add turnspeed boost
	}
	public override void RemovePowerup (TankData data) {		//speed powerup child RemovePowerup function
		base.RemovePowerup (data);								//run function in parent class
		data.forwardSpeed -= speedBoost;						//remove speed boost to forwardSpeed
		data.reverseSpeed -= speedBoost;						//remove speed boost to reverseSpeed
		//data.turnSpeed -= speedBoost;							//may re-add turnspeed boost
	}
}
