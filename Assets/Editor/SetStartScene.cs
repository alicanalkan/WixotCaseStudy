#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Wixot.Editor
{
    [InitializeOnLoad]
    public class SetStartScene 
    {
        static SetStartScene()
        {
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/LoadingScene.unity");
        }
    }
}

#endif