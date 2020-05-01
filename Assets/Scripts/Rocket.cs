using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private AudioSource source;

    private int currentLevel;

    [SerializeField] private float thrustForce = 120f;
    [SerializeField] private float rotationTorque = 25f;

    [SerializeField] AudioClip thrustSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem deathParticles;

    float camOffsetZ;

    [SerializeField] float loadDelay;

    private bool collisionDisabled;
    private bool cameraFollow;
    private Vector3 camStartPos;

    enum State { Alive, Dying, Transitioning };
    State state = State.Alive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        camStartPos = Camera.main.transform.position;
        camOffsetZ = Camera.main.transform.position.z - transform.position.z;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive && cameraFollow)
        {
          Camera.main.transform.position = transform.position + new Vector3(0, 0, camOffsetZ);
        }
        else if (state == State.Alive)
        {
            Camera.main.transform.position = camStartPos;
        }
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadNextLevel();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                collisionDisabled = !collisionDisabled;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            cameraFollow = !cameraFollow;
        }
        
    }

    void FixedUpdate()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }

    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrustForce);
            if (!source.isPlaying)
            {
                source.PlayOneShot(thrustSound);
            }
            thrustParticles.Play();
        }
        else
        {
            source.Stop();
            thrustParticles.Stop();
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeTorque(0, 0, rotationTorque);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeTorque(0, 0, -rotationTorque);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionDisabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transitioning;
                WinSequence();
                break;
            default:
                state = State.Dying;
                DeathSequence();
                break;
        }
    }

    private void WinSequence()
    {
        source.Stop();
        source.PlayOneShot(winSound);
        winParticles.Play();
        Invoke("LoadNextLevel", loadDelay);
    }

    private void DeathSequence()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddRelativeForce(new Vector3(Random.Range(-20, 20), Random.Range(-2, -20), Random.Range(-20, 20)) * thrustForce);
        rb.AddRelativeTorque(Random.Range(-50, 50), Random.Range(-20, 20), Random.Range(-50, 50) * rotationTorque);
        source.Stop();
        source.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("ReloadLevel", loadDelay);
    }

    private void LoadNextLevel()
    {
        currentLevel += 1;
        if (SceneManager.sceneCountInBuildSettings < currentLevel + 1)
        {
            currentLevel = 0;
        }
        SceneManager.LoadScene(currentLevel);
        
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }
}
