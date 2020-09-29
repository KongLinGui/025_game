namespace MoreMountains.InfiniteRunnerEngine
{	

	/// <summary>
	/// Manages the various game events
	/// Trigger them to broadcast them to other classes that have registered to it
	/// </summary>

	public static class EventManager 
	{
		public delegate void GameEvent();
		/// List of the game's events
		public static event GameEvent BeforeGameStart, WorldReset, GameStart, GameOver;

		/// Triggered at game initialization, before the actual start of the game
		public static void TriggerBeforeGameStart(){
			if(BeforeGameStart != null){
				BeforeGameStart();
			}
		}
		
		/// Triggered when the world needs position reset (to avoid high float coordinates)
		public static void TriggerWorldReset(){
			if(WorldReset != null){
				WorldReset();
			}
		}

		/// Triggered when the game starts (TODO)
		public static void TriggerGameStart(){
			if(GameStart != null){
				GameStart();
			}
		}

		/// Triggered when the player loses (TODO)
		public static void TriggerGameOver(){
			if(GameOver != null){
				GameOver();
			}
		}
	}
}