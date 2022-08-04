using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandomProject
{
    public class LevelManager : NetworkSceneManagerBase
	{
		public SceneReference gameplayScene;

		public static LevelManager Instance => Singleton<LevelManager>.Instance;

        public void LoadGameplay()
		{
			if (Launcher.ConnectionStatus != ConnectionStatus.Connected) return;

			Runner.SetActiveScene(gameplayScene.ScenePath);
		}
		
		protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
		{
			Debug.Log($"Loading scene {newScene}");
			List<NetworkObject> sceneObjects = new List<NetworkObject>();
			yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
			Scene loadedScene = SceneManager.GetSceneByBuildIndex(newScene);
			Debug.Log($"Loaded scene {newScene}: {loadedScene}");
			sceneObjects = FindNetworkObjects(loadedScene, disable: false);

			// Delay one frame, so we're sure level objects has spawned locally
			yield return null;
			finished(sceneObjects);
		}
    }
}
