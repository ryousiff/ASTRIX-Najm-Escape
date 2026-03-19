using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
public AudioSource audioSource;
    private static AudioManager instance;

    void Start()
    {
          if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void Awake()
    {
        // Prevent duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Keep this object across scenes
        DontDestroyOnLoad(gameObject);
    }
}
