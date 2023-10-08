using UnityEngine;

public class ResourceLoader : MonoBehaviour
{
    public static T Load<T>(string dataFileName) where T : ScriptableObject
    {
        string folderName = "ScriptableObjects";
        string path = folderName + "/" + dataFileName;

        T loadedResource = Resources.Load<T>(path);
        if (loadedResource == null)
        {
            Debug.LogError($"Resource not found at path: {path}");
        }
        return loadedResource;
    }
}
