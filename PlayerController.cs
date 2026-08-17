


using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    public static float BonusDMG=0f;
    public static float CritDMG=1.5f;
    public static float CritChance=0.1f;
    [SerializeField] private ParticleSystem muzzleParticles;

    
    public CharacterController controller;
    public static float speed=1.0f;
    public float gravity=-9.81f;

    public static bool isRunning=false;

    public static float StaminaLimit=100f;

    public static float stamina=100f;

    public static bool isShooting;

    private float reload=0f;

    public static bool shot=false;

    public static bool inMotion=false;

    

    float mouseX,mouseY;

    private float xRotation = 0f;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float lookSmoothTime = 0.05f;
    [SerializeField] private float maxLookAngle = 90f;

    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    [Header("Movement Feel")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float moveAcceleration = 10f;
    private Vector3 currentMoveVelocity;
    private float currentSpeedBlend; // 0 = walk, 1 = run, used for bob intensity

    [Header("Head Bob")]
    [SerializeField] private float bobFrequency = 10f;
    [SerializeField] private float bobHorizontalAmount = 0.05f;
    [SerializeField] private float bobVerticalAmount = 0.05f;
    [SerializeField] private float bobSmooth = 10f;
    private float bobTimer = 0f;
    private Vector3 cameraStartLocalPos;

    [Header("Weapon Sway")]
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float swaySmooth = 6f;
    private Vector3 weaponStartLocalPos;

    [Header("Weapon Bob")]
    [SerializeField] private float weaponBobFrequency = 10f;
    [SerializeField] private float weaponBobHorizontalAmount = 0.015f;
    [SerializeField] private float weaponBobVerticalAmount = 0.03f;
    [SerializeField] private float weaponBobSmooth = 10f;
    private float weaponBobTimer = 0f;

    [Header("Recoil")]
    public static float recoilKickback;
    [SerializeField] private float recoilRotationKick = 5f;
    [SerializeField] private float recoilSnappiness = 12f;
    [SerializeField] private float recoilReturnSpeed = 6f;
    private Vector3 currentRecoilOffset;
    private Vector3 targetRecoilOffset;
    private float currentRecoilTilt;
    private float targetRecoilTilt;
    private bool lastShotState = false;

    [Header("Weapon Recoil Kickback")]
    [SerializeField] private float weaponRecoilKickback = 0.08f;   // how far the gun kicks back on shot
    [SerializeField] private float weaponRecoilRotationKick = 8f;  // how much the gun tips up on shot
    [SerializeField] private float weaponRecoilSnappiness = 14f;
    [SerializeField] private float weaponRecoilReturnSpeed = 7f;
    private Vector3 currentWeaponRecoilOffset;
    private Vector3 targetWeaponRecoilOffset;
    private float currentWeaponRecoilTilt;
    private float targetWeaponRecoilTilt;

    [Header("Intro Camera")]
    [SerializeField] private float introStartPitch = 60f;   // positive = looking down
    [SerializeField] private float introDuration = 1.2f;
    [SerializeField] private AnimationCurve introEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private bool lockCursorAfterIntro = true;
    private bool introPlaying = true;

    [Header("Teleport")]
    [SerializeField] private Transform teleportTarget; // assign a GameObject in the Inspector

    [Header("Death Animation")]
    [SerializeField] private float deathKneesDuration = 0.6f;   // time to buckle to knees
    [SerializeField] private float deathGroundDuration = 0.8f;  // time to collapse from knees to the ground
    [SerializeField] private float deathKneeDrop = 0.5f;        // how far the camera drops when hitting knees
    [SerializeField] private float deathGroundDrop = 1.2f;      // how far the camera drops when fully on the ground
    [SerializeField] private float deathPitchKnees = 15f;       // forward tilt while on knees
    [SerializeField] private float deathPitchGround = 45f;      // forward tilt once collapsed
    [SerializeField] private float deathRoll = 30f;             // sideways roll as the body slumps
    [SerializeField] private float deathHoldBeforeSceneLoad = 0.4f; // pause on the ground before loading DeathScreen
    private bool isDead = false;

    private static PlayerController instance;

    

    private Vector3 velocity;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!lockCursorAfterIntro)
            Cursor.lockState = CursorLockMode.Locked;

        if (cameraTransform != null)
            cameraStartLocalPos = cameraTransform.localPosition;

        if (weaponTransform != null)
            weaponStartLocalPos = weaponTransform.localPosition;

        xRotation = 0f; // resting pitch the intro will animate toward

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(introStartPitch, 0f, 0f);
            StartCoroutine(PlayIntroCamera());
        }
        else
        {
            introPlaying = false;
        }
    }

    private IEnumerator PlayIntroCamera()
    {
        float t = 0f;
        while (t < introDuration)
        {
            t += Time.deltaTime;
            float normalized = introEase.Evaluate(Mathf.Clamp01(t / introDuration));
            float pitch = Mathf.Lerp(introStartPitch, xRotation, normalized);
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            yield return null;
        }

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        introPlaying = false;

        if (lockCursorAfterIntro)
            Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (introPlaying)
            return; // hold off on normal control/camera logic until the intro finishes

        if (isDead)
            return; // hold off on normal control/camera logic while the death animation plays

        HandleMouseLook();
        handlemovement();
        shootcheck();

        HandleHeadBob();
        HandleWeaponSway();
        HandleRecoil();
    }

    void handlemovement()
    {
        float x= Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(x, 0f, y);
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);

        isRunning=Input.GetKey(KeyCode.LeftShift);
        if (isRunning==true && stamina>10)
        {
            speed=runSpeed;
            stamina-=0.1f;
        }
        else
        {
            speed=walkSpeed;
            if(stamina<=StaminaLimit-1.0f)
            {
                stamina+=0.01f;
            }
            
        }

        Vector3 targetVelocity = (transform.right * inputDir.x + transform.forward * inputDir.z) * speed;

        // smooth acceleration instead of snapping straight to target speed,
        // this is what makes the resulting head bob / feel less jerky
        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, targetVelocity, Time.deltaTime * moveAcceleration);
        controller.Move(currentMoveVelocity * Time.deltaTime);

        currentSpeedBlend = Mathf.Lerp(currentSpeedBlend, isRunning ? 1f : 0f, Time.deltaTime * moveAcceleration);

        velocity.y +=gravity *Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);

        inMotion = inputDir.sqrMagnitude > 0.01f;

        if (controller.isGrounded && velocity.y<0)
        {
            velocity.y=-2f;
        }

        
    }

    void shootcheck()
{
    shot = false;   // <-- add this line
    reload+=Time.deltaTime;
    isShooting=Input.GetMouseButton(0);

    if ( isShooting==true && reload>GunManager.CurrentLoad)
    {
        shot=true;
        
        // Debug.Log("shooting");
        muzzleParticles.Play();
       
        AudioManager.GunAudio=true;
        reload=0f;

    
    }
}
    private void HandleMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, lookSmoothTime);

        mouseX = currentMouseDelta.x * mouseSensitivity;
        mouseY = currentMouseDelta.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleHeadBob()
    {
        if (cameraTransform == null) return;

        if (inMotion && controller.isGrounded)
        {
            float freq = bobFrequency * (1f + currentSpeedBlend * 0.6f);
            bobTimer += Time.deltaTime * freq;

            float bobX = Mathf.Cos(bobTimer) * bobHorizontalAmount;
            float bobY = Mathf.Abs(Mathf.Sin(bobTimer)) * bobVerticalAmount;

            Vector3 targetPos = cameraStartLocalPos + new Vector3(bobX, bobY, 0f) + currentRecoilOffset;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetPos, Time.deltaTime * bobSmooth);
        }
        else
        {
            bobTimer = 0f;
            Vector3 targetPos = cameraStartLocalPos + currentRecoilOffset;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetPos, Time.deltaTime * bobSmooth);
        }
    }

    private void HandleWeaponSway()
    {
        if (weaponTransform == null) return;

        float swayX = -mouseX * swayAmount;
        float swayY = -mouseY * swayAmount;

        Vector3 bobOffset = Vector3.zero;
        if (inMotion && controller.isGrounded)
        {
            float freq = weaponBobFrequency * (1f + currentSpeedBlend * 0.6f);
            weaponBobTimer += Time.deltaTime * freq;

            float bobX = Mathf.Cos(weaponBobTimer) * weaponBobHorizontalAmount;
            float bobY = Mathf.Abs(Mathf.Sin(weaponBobTimer)) * weaponBobVerticalAmount;
            bobOffset = new Vector3(bobX, bobY, 0f);
        }
        else
        {
            weaponBobTimer = 0f;
        }

        Vector3 targetPos = weaponStartLocalPos + new Vector3(swayX, swayY, 0f) + bobOffset + currentWeaponRecoilOffset;
        weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, targetPos, Time.deltaTime * (swaySmooth + weaponBobSmooth) * 0.5f);

        Quaternion weaponRecoilRot = Quaternion.Euler(-currentWeaponRecoilTilt, 0f, 0f);
        weaponTransform.localRotation = Quaternion.Slerp(weaponTransform.localRotation, weaponRecoilRot, Time.deltaTime * weaponRecoilSnappiness);
    }

    private void HandleRecoil()
{
    if (shot == true && lastShotState == false)
    {
        targetRecoilOffset += new Vector3(0f, 0f, -recoilKickback);
        targetRecoilTilt -= recoilRotationKick;

        targetWeaponRecoilOffset += new Vector3(0f, 0f, -weaponRecoilKickback);
        targetWeaponRecoilTilt += weaponRecoilRotationKick;
    }
    lastShotState = shot;

    currentRecoilOffset = Vector3.Lerp(currentRecoilOffset, targetRecoilOffset, Time.deltaTime * recoilSnappiness);
    targetRecoilOffset = Vector3.Lerp(targetRecoilOffset, Vector3.zero, Time.deltaTime * recoilReturnSpeed);

    currentRecoilTilt = Mathf.Lerp(currentRecoilTilt, targetRecoilTilt, Time.deltaTime * recoilSnappiness);
    targetRecoilTilt = Mathf.Lerp(targetRecoilTilt, 0f, Time.deltaTime * recoilReturnSpeed);

    currentWeaponRecoilOffset = Vector3.Lerp(currentWeaponRecoilOffset, targetWeaponRecoilOffset, Time.deltaTime * weaponRecoilSnappiness);
    targetWeaponRecoilOffset = Vector3.Lerp(targetWeaponRecoilOffset, Vector3.zero, Time.deltaTime * weaponRecoilReturnSpeed);

    currentWeaponRecoilTilt = Mathf.Lerp(currentWeaponRecoilTilt, targetWeaponRecoilTilt, Time.deltaTime * weaponRecoilSnappiness);
    targetWeaponRecoilTilt = Mathf.Lerp(targetWeaponRecoilTilt, 0f, Time.deltaTime * weaponRecoilReturnSpeed);

    if (cameraTransform != null)
    {
        Quaternion baseRot = Quaternion.Euler(xRotation, 0f, 0f);
        Quaternion recoilRot = Quaternion.Euler(currentRecoilTilt, 0f, 0f);
        cameraTransform.localRotation = baseRot * recoilRot;
    }



}

    // ---------------- Teleport System ----------------

    // Teleports the player to the Transform assigned in the Inspector (teleportTarget field)
    public static void TeleportToTarget()
    {
        if (instance == null || instance.teleportTarget == null)
        {
            Debug.LogWarning("TeleportToTarget: missing instance or teleportTarget reference.");
            return;
        }

        instance.TeleportInternal(instance.teleportTarget.position);
    }

    // Teleports the player to any arbitrary world position, callable from other scripts
    public static void TeleportTo(Vector3 destination)
    {
        if (instance == null)
        {
            Debug.LogWarning("TeleportTo: PlayerController instance not found.");
            return;
        }

        instance.TeleportInternal(destination);
    }

    // Handles the actual safe teleport (disables CharacterController to avoid collision glitches)
    private void TeleportInternal(Vector3 destination)
    {
        if (controller != null)
            controller.enabled = false;

        transform.position = destination;

        // reset vertical velocity so gravity doesn't fling the player on landing
        velocity.y = 0f;

        if (controller != null)
            controller.enabled = true;
    }

    // ---------------- Death Sequence ----------------

    public static void HandlePlayerDeath()
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.DeathFallSequence());
        }
        else
        {
            // fallback in case there's no instance to run the coroutine on
            NewWave.CleanVars();
            SceneManager.LoadScene("DeathScreen");
        }
    }

    // Animates the camera as if the player is buckling to their knees, then collapsing fully to the ground,
    // then loads the death screen once the fall finishes.
    private IEnumerator DeathFallSequence()
    {
        isDead = true;

        if (cameraTransform == null)
        {
            // no camera to animate, just proceed straight to the death screen
            yield return new WaitForSeconds(deathHoldBeforeSceneLoad);
            NewWave.CleanVars();
            SceneManager.LoadScene("DeathScreen");
            yield break;
        }

        Vector3 startCamPos = cameraTransform.localPosition;
        Quaternion startCamRot = cameraTransform.localRotation;

        // Phase 1: buckle to knees
        Vector3 kneesTargetPos = cameraStartLocalPos + new Vector3(0f, -deathKneeDrop, 0f);
        Quaternion kneesTargetRot = Quaternion.Euler(xRotation + deathPitchKnees, 0f, deathRoll * 0.4f);

        float t = 0f;
        while (t < deathKneesDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.SmoothStep(0f, 1f, t / deathKneesDuration);

            cameraTransform.localPosition = Vector3.Lerp(startCamPos, kneesTargetPos, n);
            cameraTransform.localRotation = Quaternion.Slerp(startCamRot, kneesTargetRot, n);

            yield return null;
        }

        // Phase 2: collapse from knees to the ground
        Vector3 groundTargetPos = cameraStartLocalPos + new Vector3(0f, -deathGroundDrop, 0f);
        Quaternion groundTargetRot = Quaternion.Euler(xRotation + deathPitchGround, 0f, deathRoll);

        float t2 = 0f;
        while (t2 < deathGroundDuration)
        {
            t2 += Time.deltaTime;
            float n = Mathf.SmoothStep(0f, 1f, t2 / deathGroundDuration);

            cameraTransform.localPosition = Vector3.Lerp(kneesTargetPos, groundTargetPos, n);
            cameraTransform.localRotation = Quaternion.Slerp(kneesTargetRot, groundTargetRot, n);

            yield return null;
        }

        yield return new WaitForSeconds(deathHoldBeforeSceneLoad);

        NewWave.CleanVars();
        SceneManager.LoadScene("DeathScreen");
    }

}
