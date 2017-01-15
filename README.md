# U-EAT
Unity Events, Actions and Triggers Library

Download the all-in-one .UnityPackage here: https://mega.nz/#!l9wgUSoQ!XWuAoBS2bjVSfydrsFh1ZE0Hhz17L5Ghf6c-bfjBkGo

If you get a lot of 'null-object reference' errors, playing the project should get rid of them.


Below are the prefabs and the components which are supposed to be on them, in case they break from importing into a vrsion of Unity before 5.5.
GameSession
{
	Game
}

The GameSession prefab should never be instatiated. You cn use Game.GameSession to listen to events dispatched to this prefab.

GameEventHandler
{
	GameEventHandler
	Reactive
	{
		UpdateEvents = true;
		InstatiationEvents = true;
		GameEvents = true;
	}
	DontDestroyOnLoad
}
The GameEventHandler is what actually dispatches events to the gamesession. Only 1 should ever be instatiated throughout the entire game.

Space
{
	Reactive
	{
		UpdateEvents = true;
		InstatiationEvents = true;
	}
}
Ideally, there should be one of this object in every scene. You can have other objects listen to this object for events in order to reduce uneeded function calls.