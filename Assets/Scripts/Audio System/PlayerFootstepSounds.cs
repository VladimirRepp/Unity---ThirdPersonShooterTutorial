using StarterAssets;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController), typeof(AudioSource), typeof(SurfaceDetector))]
public class PlayerFootstepSounds : MonoBehaviour
{
    [System.Serializable]
    public class MovementSettings
    {
        public float walkStepInterval = 0.5f;
        public float sprintStepInterval = 0.3f;
        public float crouchStepInterval = 0.7f;
        public float velocityThreshold = 0.1f;
        public float landSoundVelocityThreshold = 3f;
    }

    [Header("Component Settings")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SurfaceDetector surfaceDetector;

    [Header("Movement Settings")]
    public MovementSettings movementSettings = new MovementSettings();

    [Header("Timing Settings")]
    [SerializeField] private float stepTimer = 0f;
    [SerializeField] private bool wasGroundedLastFrame = true;
    [SerializeField] private float currentStepInterval;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    private StarterAssetsInputs _input;

    private void Awake()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (surfaceDetector == null)
            surfaceDetector = GetComponent<SurfaceDetector>();

        audioSource.spatialBlend = 1f; // 3D звук
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 20f;

        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "PlayerFootstepSounds using the new Input System, but ENABLE_INPUT_SYSTEM is not defined. Please enable the Input System package and define ENABLE_INPUT_SYSTEM in Player Settings." );
#endif
    }

    private void Update()
    {
        if (!characterController.isGrounded ||
            characterController.velocity.magnitude < movementSettings.velocityThreshold)
        {
            stepTimer = 0f;
            return;
        }

        HandleFootsteps();
        HandleLandingSound();

        wasGroundedLastFrame = characterController.isGrounded;
    }

    private void HandleFootsteps()
    {
        // Определяем интервал шагов в зависимости от состояния
        if (_input.sprint && characterController.velocity.magnitude > 3f)
        {
            currentStepInterval = movementSettings.sprintStepInterval;
        }
        else if (_input.crouch)
        {
            currentStepInterval = movementSettings.crouchStepInterval;
        }
        else
        {
            currentStepInterval = movementSettings.walkStepInterval;
        }

        // Таймер шагов
        stepTimer += Time.deltaTime;

        if (stepTimer >= currentStepInterval)
        {
            PlayFootstepSound();
            stepTimer = 0f;
        }
    }

    private void HandleLandingSound()
    {
        if (characterController.isGrounded && !wasGroundedLastFrame)
        {
            // Рассчитываем скорость падения (примерно)
            float fallVelocity = Mathf.Abs(characterController.velocity.y);

            if (fallVelocity > movementSettings.landSoundVelocityThreshold)
            {
                PlayLandSound();
            }
        }
    }

    private void PlayFootstepSound()
    {
        var (surfaceTag, physicMaterial) = surfaceDetector.GetCurrentSurfaceInfo();
        var surfaceSet = AudioManager.Instance.GetSurfaceSoundSet(surfaceTag, physicMaterial);

        AudioClip[] clipsToUse;

        // Выбираем набор звуков в зависимости от действия
        if (Input.GetKey(KeyCode.LeftShift) && characterController.velocity.magnitude > 3f)
        {
            clipsToUse = surfaceSet.sprintSounds.Length > 0 ?
                         surfaceSet.sprintSounds : surfaceSet.footstepSounds;
        }
        else
        {
            clipsToUse = surfaceSet.footstepSounds;
        }

        AudioClip clip = AudioManager.Instance.GetRandomClip(clipsToUse);

        if (clip != null)
        {
            audioSource.pitch = AudioManager.Instance.GetRandomizedPitch();
            audioSource.volume = AudioManager.Instance.footstepVolume * surfaceSet.volumeMultiplier;
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayJumpSound()
    {
        var (surfaceTag, physicMaterial) = surfaceDetector.GetCurrentSurfaceInfo();
        var surfaceSet = AudioManager.Instance.GetSurfaceSoundSet(surfaceTag, physicMaterial);

        AudioClip clip = AudioManager.Instance.GetRandomClip(surfaceSet.jumpSounds);

        if (clip != null)
        {
            audioSource.pitch = AudioManager.Instance.GetRandomizedPitch();
            audioSource.volume = AudioManager.Instance.jumpVolume * surfaceSet.volumeMultiplier;
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayLandSound()
    {
        var (surfaceTag, physicMaterial) = surfaceDetector.GetCurrentSurfaceInfo();
        var surfaceSet = AudioManager.Instance.GetSurfaceSoundSet(surfaceTag, physicMaterial);

        AudioClip clip = AudioManager.Instance.GetRandomClip(surfaceSet.landSounds);

        if (clip != null)
        {
            audioSource.pitch = AudioManager.Instance.GetRandomizedPitch();
            audioSource.volume = AudioManager.Instance.landVolume * surfaceSet.volumeMultiplier;
            audioSource.PlayOneShot(clip);
        }
    }

    // Метод для вызова из системы анимаций
    public void AnimationEventFootstep()
    {
        if (characterController.isGrounded)
        {
            PlayFootstepSound();
        }
    }

    // Метод для вызова из системы анимаций при прыжке
    public void AnimationEventJump()
    {
        PlayJumpSound();
    }
}