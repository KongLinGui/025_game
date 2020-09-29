using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	[RequireComponent (typeof (ObjectPooler))]
	public class Spawner : MonoBehaviour 
	{

		[Header("Size")]
		/// the minimum size of a spawned object
		public Vector3 MinimumSize ;
		/// the maximum size of a spawned object
		public Vector3 MaximumSize ;	
		[Space(10)]	
		[Header("Rotation")]
		/// the minimum size of a spawned object
		public Vector3 MinimumRotation ;
		/// the maximum size of a spawned object
		public Vector3 MaximumRotation ;
		[Space(10)]	
		[Header("Vertical Clamp")]
		/// the minimum Y position we can spawn the object at
		public float MinimumYClamp;
		/// the maximum Y position we can spawn the object at
		public float MaximumYClamp;
	    /// if true, only spawn objects while the game is in progress
	    public bool OnlySpawnWhileGameInProgress = true;

	    protected ObjectPooler _objectPooler;

		/// <summary>
		/// On awake, we get the objectPooler component
		/// </summary>
	    protected virtual void Awake()
	    {
	        _objectPooler = GetComponent<ObjectPooler>();
	    }
			
		/// <summary>
		/// Spawns a new object and positions/resizes it
		/// </summary>
		public virtual GameObject Spawn(Vector3 spawnPosition)
		{
	        if (OnlySpawnWhileGameInProgress)
	        {
	            if (GameManager.Instance.Status!=GameManager.GameStatus.GameInProgress)
	            {
	                return null;
	            }
	        }
	        /// we get the next object in the pool and make sure it's not null
	        GameObject nextGameObject = _objectPooler.GetPooledGameObject();
			if (nextGameObject==null)	{ return null; }

	        /// we rescale the object
	        Vector3 scale = new Vector3 (Random.Range (MinimumSize.x, MaximumSize.x), Random.Range (MinimumSize.y, MaximumSize.y), Random.Range (MinimumSize.z, MaximumSize.z));
			nextGameObject.transform.localScale = scale;		
			
			// we adjust the object's position based on its renderer's size
			spawnPosition.x +=   nextGameObject.GetComponent<Renderer> ().bounds.size.x/2;
			spawnPosition.y +=   nextGameObject.GetComponent<Renderer> ().bounds.size.y/2;
			spawnPosition.y = Mathf.Clamp (spawnPosition.y, MinimumYClamp, MaximumYClamp);
			nextGameObject.transform.position =spawnPosition;
			
			// we set the object's rotation
			nextGameObject.transform.eulerAngles = new Vector3 (
				Random.Range (MinimumRotation.x, MaximumRotation.x), 
				Random.Range (MinimumRotation.y, MaximumRotation.y), 
				Random.Range (MinimumRotation.z, MaximumRotation.z)
				);

			// we activate the object
	        nextGameObject.SetActive(true);

	        return (nextGameObject);

	    }

		/// <summary>
		/// When the object is selected in scene view, we draw the min and max Y clamps
		/// </summary>
	    protected virtual void OnDrawGizmosSelected()
	    {
	        DrawClamps();
	    }

		/// <summary>
		/// Draws the position clamps, called when the object is selected in scene view
		/// </summary>
	    protected virtual void DrawClamps()
	    {
	        Gizmos.color = Color.cyan;
	        // draws a line for the minimum Y clamp
	        Gizmos.DrawLine(new Vector2(transform.position.x - 5, MinimumYClamp), new Vector2(transform.position.x + 5, MinimumYClamp));
	        // draws a line for the maximum Y clamp
	        Gizmos.DrawLine(new Vector2(transform.position.x - 5, MaximumYClamp), new Vector2(transform.position.x + 5, MaximumYClamp));
	    }
	}
}