using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Ship : MonoBehaviour
{
    public enum EOT { Stop, FullAhead, HalfAhead, FullAstern, HalfAstern, SlowAhead, SlowAstern }
    public static float MaxRudderAngle = 35f;

    public UnityAction OnCrash;
    public UnityAction OnDestroy;
    public UnityAction<float> UpdateHitpointRatio;

    [Range(1f,50f)]
    public float MaxSpeed = 1f;
    public float SlowSpeed = 3f;
    public float rudderDegTick = 0.5f;
    public float engineResponseTime = 30f;
    public int hitpoints = 1000;

    public WeaponsSys.WeaponsRack weapons;

    [Header("temporary")]
    public GameObject explodingObject;

    private float rudderAngle = 0f;
    private float targetSpeed = 0;

    private int MaxHitpoints;

    private float changeSpeedtimeLapse = 0f;
    private float initialSpeed = 0f;
    private int crashToLayers;


    public Vector2 Heading { get; private set; }
    public Vector2 Position { get => transform.position; }
    public Quaternion Rotation { get => transform.rotation; }
    public float rudderAngleAdjustment { get; private set; }
    public float Speed { get; private set; }
    public bool CrashRecovering { get; private set; }

    private void Awake()
    {
        weapons = GetComponentInChildren<WeaponsSys.WeaponsRack>();
    }

    void Start()
    {
        crashToLayers |= (1 << 8); //Player Layer
        crashToLayers |= (1 << 11); //Terrain

        MaxHitpoints = hitpoints;
        Heading = Vector2.up;
        OnCrash += Crashed;

        OnDestroy += Explode;
    }

    void Update()
    {
        UpdatePosition();
        UpdateHeading();
        UpdateRotation();
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        if (Mathf.Abs(Mathf.Abs(targetSpeed) - Mathf.Abs(Speed)) > 0)
        {
            changeSpeedtimeLapse += Time.deltaTime;
            Speed = Mathf.Lerp(initialSpeed, targetSpeed, changeSpeedtimeLapse / engineResponseTime);

            if (changeSpeedtimeLapse > engineResponseTime)
                Speed = targetSpeed;
        }
    }

    private void UpdateHeading()
    {
        rudderAngle += rudderAngleAdjustment * Time.deltaTime * Speed;
        Heading = new Vector2(Mathf.Cos(rudderAngle * Mathf.Deg2Rad), Mathf.Sin(rudderAngle * Mathf.Deg2Rad));
    }

    private void UpdatePosition()
    {
        transform.position += (Vector3)(Heading * Speed * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, rudderAngle);
    }

    public void SetSpeed(EOT engineOrder)
    {
        SetEngineOrder(engineOrder);
    }

    private void SetEngineOrder(EOT engineOrder)
    {
        initialSpeed = Speed;
        changeSpeedtimeLapse = 0;

        switch (engineOrder)
        {
            case EOT.Stop: targetSpeed = 0; break;
            case EOT.FullAhead: targetSpeed = MaxSpeed; break;
            case EOT.HalfAhead: targetSpeed = MaxSpeed / 2; break;
            case EOT.FullAstern: targetSpeed = -MaxSpeed / 3; break;
            case EOT.HalfAstern: targetSpeed = -MaxSpeed / 6; break;
            case EOT.SlowAhead: targetSpeed = SlowSpeed; break;
            case EOT.SlowAstern: targetSpeed = -SlowSpeed; break;
            default: Speed = 0; break;
        }
    }

    public void SteerTickCounterClockWise()
    {
        rudderAngleAdjustment = Mathf.Clamp(rudderAngleAdjustment + rudderDegTick, -MaxRudderAngle, +MaxRudderAngle);
    }

    public void SteerTickClockWise()
    {
        rudderAngleAdjustment = Mathf.Clamp(rudderAngleAdjustment - rudderDegTick, -MaxRudderAngle, +MaxRudderAngle);
    }

    public void ResetSteer()
    {
        rudderAngleAdjustment = 0;
    }

    private void Crashed()
    {
        TakeDamage((int)(Mathf.Abs(Speed / 10) * MaxHitpoints));
        StartCoroutine(CrashRecovery(2f));

        Speed = -Speed * 0.1f + (Mathf.Sign(-Speed) * 0.02f);
        SetEngineOrder(EOT.Stop);
    }

    public void TakeDamage(int damage)
    {
        hitpoints = Mathf.Clamp(hitpoints - damage, 0, int.MaxValue);
        UpdateHitpointRatio?.Invoke((float)hitpoints/MaxHitpoints);

        if (hitpoints == 0)
            OnDestroy?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OnCrashLayers(other) && !CrashRecovering)
        {
            OnCrash?.Invoke();
            //TODO Remove when many ships are controlled
            //EZCameraShake.CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1.4f);
        }
    }

    private bool OnCrashLayers(Collider2D other)
    {
        return (crashToLayers == (crashToLayers | (1 << other.gameObject.layer)));
    }

    private IEnumerator CrashRecovery(float time)
    {
        CrashRecovering = true;
        yield return new WaitForSeconds(time);
        CrashRecovering = false;
    }

    private void Explode()
    {
        GameObject explosion = null;

        if (explodingObject != null)
        {
            explosion = Instantiate(explodingObject, transform.position, Quaternion.identity);
            explosion.transform.localScale *= 10;
            Destroy(explosion, 0.5f);
        }

        Destroy(gameObject, 0.1f);
    }
}
