using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Various static methods used throughout the Infinite Runner Engine and the Corgi Engine.
	/// </summary>

	public static class CorgiTools 
	{

		/// <summary>
		/// Draws a debug ray and does the actual raycast
		/// </summary>
		/// <returns>The cast.</returns>
		/// <param name="rayOriginPoint">Ray origin point.</param>
		/// <param name="rayDirection">Ray direction.</param>
		/// <param name="rayDistance">Ray distance.</param>
		/// <param name="mask">Mask.</param>
		/// <param name="debug">If set to <c>true</c> debug.</param>
		/// <param name="color">Color.</param>
		public static RaycastHit2D CorgiRayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask,bool debug,Color color)
		{			
			Debug.DrawRay( rayOriginPoint, rayDirection*rayDistance, color );
			return Physics2D.Raycast(rayOriginPoint,rayDirection,rayDistance,mask);		
		}

		/// <summary>
		/// Outputs the message object to the console, prefixed with the current timestamp
		/// </summary>
		/// <param name="message">Message.</param>
		public static void DebugLogTime(object message)
		{
			Debug.Log (Time.time + " " + message);

		}
		
		/// <summary>
		/// Fades the specified image to the target opacity and duration.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="opacity">Opacity.</param>
		/// <param name="duration">Duration.</param>
		public static IEnumerator FadeImage(Image target, float duration, Color color)
		{		
			if (target==null)
				yield break;
				
			float alpha = target.color.a;
			
			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
			{
				if (target==null)
					yield break;
				Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha,color.a,t));
				target.color=newColor;
				yield return null;
			}
		}
		/// <summary>
		/// Fades the specified image to the target opacity and duration.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="opacity">Opacity.</param>
		/// <param name="duration">Duration.</param>
		public static IEnumerator FadeText(Text target, float duration, Color color)
		{
			if (target==null)
				yield break;
				
			float alpha = target.color.a;
			
			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
			{
				if (target==null)
					yield break;
				Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha,color.a,t));
				target.color=newColor;
				yield return null;
			}
		}
		/// <summary>
		/// Fades the specified image to the target opacity and duration.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="opacity">Opacity.</param>
		/// <param name="duration">Duration.</param>
		public static IEnumerator FadeSprite(SpriteRenderer target, float duration, Color color)
		{
			if (target==null)
				yield break;
		
			float alpha = target.material.color.a;		
			
			float t=0f;
			while (t<1.0f)
			{
				if (target==null)
					yield break;
									
				Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha,color.a,t));
				target.material.color=newColor;
				
				t += Time.deltaTime / duration;
				
				yield return null;
					
			}
			Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha,color.a,t));
			target.material.color=finalColor;
		}
		
		/// <summary>
		/// Updates the animator bool.
		/// </summary>
		/// <param name="animator">Animator.</param>
		/// <param name="parameterName">Parameter name.</param>
		/// <param name="value">If set to <c>true</c> value.</param>
		public static void UpdateAnimatorBool(Animator animator, string parameterName,bool value)
		{
			if (animator.HasParameterOfType (parameterName, AnimatorControllerParameterType.Bool))
				animator.SetBool(parameterName,value);
		}
		
		/// <summary>
		/// Updates the animator float.
		/// </summary>
		/// <param name="animator">Animator.</param>
		/// <param name="parameterName">Parameter name.</param>
		/// <param name="value">Value.</param>
		public static void UpdateAnimatorFloat(Animator animator, string parameterName,float value)
		{
			if (animator.HasParameterOfType (parameterName, AnimatorControllerParameterType.Float))
				animator.SetFloat(parameterName,value);
		}
		
		/// <summary>
		/// Updates the animator integer.
		/// </summary>
		/// <param name="animator">Animator.</param>
		/// <param name="parameterName">Parameter name.</param>
		/// <param name="value">Value.</param>
		public static void UpdateAnimatorInteger(Animator animator, string parameterName,int value)
		{
			if (animator.HasParameterOfType (parameterName, AnimatorControllerParameterType.Int))
				animator.SetInteger(parameterName,value);
		}

	    /// <summary>
	    /// Coroutine used to make the character's sprite flicker (when hurt for example).
	    /// </summary>
	    public static IEnumerator Flicker(Renderer renderer, Color flickerColor, float flickerSpeed, float flickerDuration)
	    {
	        Color initialColor = renderer.material.color;
	        float flickerStop = Time.time + flickerDuration;

	        while (Time.time<flickerStop)
	        {
	            renderer.material.color = initialColor;
	            yield return new WaitForSeconds(flickerSpeed);
	            renderer.material.color = flickerColor;
	            yield return new WaitForSeconds(flickerSpeed);
	        }

	        renderer.material.color = initialColor;        
	    }
	}
}
