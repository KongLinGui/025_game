using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Extend this class to make your own pickable objects. Look at Coin.cs for an example.
	/// Note that you need a boxcollider or boxcollider2D for this component to work. 
	/// </summary>
	public class PickableObject : MonoBehaviour 
	{
		/// The effect to instantiate when the coin is hit
		public GameObject PickEffect;
		
		/// <summary>
		/// Triggered when something collides with the coin
		/// </summary>
		/// <param name="collider">Other.</param>
		public virtual void OnTriggerEnter2D (Collider2D collider) 
		{
			// if what's colliding with the coin ain't a characterBehavior, we do nothing and exit
			if (collider.GetComponent<PlayableCharacter>() == null)
				return;
			
			// adds an instance of the effect at the coin's position
			if (PickEffect!=null)
			{
				Instantiate(PickEffect,transform.position,transform.rotation);
			}
			ObjectPicked();
			// we desactivate the gameobject
			gameObject.SetActive(false);
		}
		
		/// <summary>
		/// Override this to describe what happens when that object gets picked.
		/// </summary>
		protected virtual void ObjectPicked()
		{
		
		}
	}
}