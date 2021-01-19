using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    private AudioSource audioSource;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogWarning("Multple SoundManagers on Scene");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    public void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetInt("sfxOn", 1);

    }

    public void PlaySound(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }
}