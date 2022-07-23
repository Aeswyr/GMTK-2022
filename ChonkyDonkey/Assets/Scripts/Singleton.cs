using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Component {
    protected static T instance = null;

    public static T Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<T>();
            if (instance == null)
                instance = FindInScene();
            return instance;
        }
    }

    private static T FindInScene()
    {
        var objs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var gameObject in objs)
        {
            T found = gameObject.transform.GetComponentInChildren(typeof(T)) as T;
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

}