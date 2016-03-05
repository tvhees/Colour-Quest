using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class ManaCollection<T> : MonoBehaviour where T : MonoBehaviour
{

    public RectTransform rTransform;
    public float horizGap, scale;
    public List<GameObject> manaList;

    protected Rect rect;
    protected int size;

    protected void Start() {
        rect = rTransform.rect;
    }

    public void AddMana(GameObject mana) {
        // Add to container
        manaList.Add(mana);

        // Assign local position to the right of existing mana
        Vector3 localPoint = new Vector3(rect.x + (size + 1.0f) * horizGap, rect.center.y, 0f);
        mana.transform.parent = transform;
        mana.transform.localScale = scale * Vector3.one;
        mana.transform.localPosition = localPoint;

        // Track mana in container
        size++;
    }

    public void RemoveMana(GameObject mana) {
        if (manaList.Contains(mana)) {
            manaList.Remove(mana);
            size--;
        }
    }



    // Singleton Code - DON'T ALTER
    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                 "' already destroyed on application quit." +
                                 " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                                  " is needed in the scene, so '" + singleton +
                                  "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                                  _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}