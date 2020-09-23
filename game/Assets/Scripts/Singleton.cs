using UnityEngine;

public class Singleton : MonoBehaviour
{
    public int number;
    private static Singleton instance;

    public static Singleton Instance
    {
        get
        {
            if (instance == null)
                instance = new GameObject("Singleton").AddComponent<Singleton>();
            return instance;
        }
    }
}