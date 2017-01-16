These prefabs were created with Unity v5.5. You will run into unexpected behavior when trying to load them in Unity v5.4 or earlier.

While 5.4 is not officially supported by this project, You should be able to re-make the prefabs if necessary.

The Prefabs should be defined as follows:

-------

## GameEventHandler
The GameEventHandler is what actually dispatches events to the gamesession. Only one should ever be instatiated throughout the entire game.

#### Prefab Components
  - *GameEventHandler* (default settings)
  - *DontDestroyOnLoad* (default settings)
  - *Reactive*
    - `Mouse Events` : TRUE
    - `Instantiation Events` : TRUE
    - `Update Events` : TRUE
    - `Collision Events` : FALSE
    - `Game Events` : FALSE

-------

## GameSession
The GameSession should never be instatiated. It provides the interface for sending and receiving global events. For example:
- `Game.GameSession.Dispatch("SomeEventName", null);` to send an event with no data.
- `Game.GameSession.Connect("SomeEventName", SomeEventCallbackFunc);` to register a callback for an event.

#### Prefab Components  
  - *Game* (default settings)
 
 -------

## Space
Ideally, there should be one Space object in every scene. You can have other objects listen to this object for events in order to reduce unnecessary function calls.

#### Prefab Components
  - *Reactive*
    - `Mouse Events` : FALSE
    - `Instantiation Events` : TRUE
    - `Update Events` : TRUE
    - `Collision Events` : TRUE
    - `Game Events` : TRUE
  
