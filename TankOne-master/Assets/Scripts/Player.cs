using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Player : MonoBehaviour
{
    public int id;
    public int playerType = 1;
    public int selectedWeaponId = 1;
    public RowWeapon selectedWeapon;
    public GameObject gameoverParticle;

    public bool isAI;

    public Transform body, shell, turret;
    public Vector3 turretAdjtPos, shellAdjPos;
    public Vector2 maxAngle = new Vector2(0, 180);
    public AudioSource audioSource;

    private FixedJoystick moveJoystic;
    private FixedJoystick rotateJoystic;

    public Transform canvas;
    public Transform projectileHolder;
    public WheelJoint2D frontWheel, backWheel, midleWheel;

    public GameObject dotPref;
    public GameObject[] bulletPrefs;
    public Transform spwan;

    public Transform dot;

    private Coroutine corAI;
    private Healthbar healthbar;
    [SerializeField] private TextMeshProUGUI textAngle, textPower;
    [SerializeField]
    private float fuelAmount = 100;


    private float MAX_SPEED = 20;
    private float carSpeed = 500;
    private float bulletSpeed = 5;

    private Vector2 LAUNCH_VELOCITY;
    private Vector2 GRAVITY = new Vector2(0f, -9.8f);
    private float DOT_TIME_STEP = 0.05f;
    private List<Transform> projetileDots = new List<Transform>();
    private float projectileLifeTime;
    private float projectileDist;
    [SerializeField]
    private bool isActive, fireEnabled;

    private float health = 100f;

    private Rigidbody2D rb;

    private List<int> usedWeaponIds;
    private int weaponIndex = -1;
    private List<RowWeapon> activeWeapons;


    void Start()
    {
        print("Player start");
        Init();

    }


    void Update()
    {
        if (!isAI)
            Move();

        if (!isActive) return;
        Rotate();
        if (!isAI)
            DrawProjectile();

    }

    private void Init()
    {
        moveJoystic = GameController.GetInstance().moveJoystick;
        rotateJoystic = GameController.GetInstance().rotateJoystic;
        rb = GetComponent<Rigidbody2D>();

        Physics2D.gravity = Vector2.zero;

        if (playerType == 2)
        {

            body.eulerAngles = new Vector3(0, 180, 0);
            shell.eulerAngles = new Vector3(0, 180, 0);
            turret.eulerAngles = new Vector3(0, 180, 0);
            if (turretAdjtPos != Vector3.zero)
                turret.localPosition = turretAdjtPos;
            if (shellAdjPos != Vector3.zero)
                shell.localPosition = shellAdjPos;
            //transform.eulerAngles = new Vector3(0,180,0);
            //canvas.eulerAngles = new Vector3(0, 180, 0);

            healthbar = GameController.GetInstance().health2;
            healthbar.Init();

            canvas.gameObject.SetActive(false);

            if (isAI)
            {
                projectileHolder.gameObject.SetActive(false);


                usedWeaponIds = new List<int>();
                usedWeaponIds.Add(1);
                usedWeaponIds.Add(2);
                usedWeaponIds.Add(3);
                usedWeaponIds.Add(4);
                usedWeaponIds.Add(5);
                usedWeaponIds.Add(6);

                usedWeaponIds = Randomize<int>(usedWeaponIds);
            }
        }
        else
        {
            healthbar = GameController.GetInstance().health1;
            healthbar.Init();
        }
        usedWeaponIds = new List<int>();

        activeWeapons = new List<RowWeapon>();
        foreach (RowWeapon weapon in GameController.Instance.allWeapons)
            activeWeapons.Add(weapon);



        Physics2D.gravity = GRAVITY;
        GenerateDot();
    }

    public void StartAI()
    {
        if (corAI != null)
            StopCoroutine(corAI);
        corAI = StartCoroutine(IStartAI());

    }


    public static List<T> Randomize<T>(List<T> list)
    {
        List<T> randomizedList = new List<T>();
        while (list.Count > 0)
        {
            int index = Random.Range(0, list.Count); //pick a random item from the master list
            randomizedList.Add(list[index]); //place it at the end of the randomized list
            list.RemoveAt(index);
        }
        return randomizedList;
    }
    private void Move()
    {

        float acc = 0;
        if (isActive)
        {
            if (fuelAmount <= 0)
            {
                acc = 0;
            }
            else if (moveJoystic.Horizontal > 0)
            {
                acc = 1;
                if (fuelAmount > 0)
                    fuelAmount -= 10f * Time.deltaTime;
            }

            else if (moveJoystic.Horizontal < 0)
            {
                acc = -1;
                if (fuelAmount > 0)
                    fuelAmount -= 10f * Time.deltaTime;
            }

            GameController.GetInstance().fuel.fillAmount = fuelAmount * 0.01f;
            if (acc != 0)
            {
                if (!audioSource.isPlaying && GameData.playerState.isSound)
                {
                    audioSource.Play();
                }


            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

            }
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }

        JointMotor2D motor = new JointMotor2D { motorSpeed = (carSpeed + GameData.playerState.speedMultiplier * 100) * acc , maxMotorTorque = 10000 };
        frontWheel.motor = motor;
        backWheel.motor = motor;
        midleWheel.motor = motor;

    }

    private void Rotate()
    {
        Vector2 dir = (rotateJoystic.Direction).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle > 0)
        {
            angle = Mathf.Clamp(angle, maxAngle.x, maxAngle.y);
            turret.eulerAngles = Vector3.forward * angle;
            textAngle.text = (int)angle + "";
        }

    }

    private Vector2 CalculatePosition(float elapsedTime, Vector2 initialPos)
    {
        return GRAVITY * elapsedTime * elapsedTime * 0.5f +
                   LAUNCH_VELOCITY * elapsedTime + initialPos;
    }

    private void DrawProjectile()
    {
        if (rotateJoystic.Direction.magnitude > 0)
            bulletSpeed = rotateJoystic.Direction.magnitude * MAX_SPEED;

        bulletSpeed = Mathf.Clamp(bulletSpeed, 5f, MAX_SPEED);

        LAUNCH_VELOCITY = turret.right * bulletSpeed;

        textPower.text = (int)(100 / MAX_SPEED * bulletSpeed) + "";

        for (int i = 0; i < projetileDots.Count; i++)
        {
            projetileDots[i].position = CalculatePosition(DOT_TIME_STEP * (i + 1), spwan.position);
        }


    }
    private void GenerateDot()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject trajectoryDot = Instantiate(dotPref, projectileHolder);
            trajectoryDot.transform.position = CalculatePosition(DOT_TIME_STEP * i, spwan.position);
            projetileDots.Add(trajectoryDot.transform);
        }
    }

    private Vector2 Calculate(Vector2 target)
    {
        float gravity = -9.8f;
        float h = Random.Range(4, 8);
        float displacementY = target.y - spwan.position.y;
        Vector2 displacementX = new Vector2(target.x - spwan.position.x, 0);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * gravity * h);
        Vector2 velocityXZ = displacementX / time;
        return velocityXZ + velocityY;
    }
    public void Fire()
    {
        if (!fireEnabled) return;

        SoundManager.Instance.PlayShot();
        GameController.Instance.InputController(false);
        GameObject bullet = Instantiate(bulletPrefs[selectedWeapon.id - 1], spwan.position, Quaternion.Euler(turret.eulerAngles));

        bullet.GetComponent<Bullet>().Fire(LAUNCH_VELOCITY, selectedWeapon.demageAmount);

        fireEnabled = false;

        for (int i = 0; i < activeWeapons.Count; i++)
        {
            if (activeWeapons[i].id == selectedWeapon.id)
            {

                activeWeapons.RemoveAt(i);
                usedWeaponIds.Add(selectedWeapon.id);
            }
        }



        if (activeWeapons.Count <= 0)
        {
            foreach (RowWeapon weapon in GameController.Instance.allWeapons)
                activeWeapons.Add(weapon);
            usedWeaponIds.Clear();

        }




    }

    private void ChangeTurn()
    {
        GameController.GetInstance().ChangeTurn();

    }
    public void SetActive(bool isActive)
    {

        this.isActive = isActive;
        fireEnabled = isActive;

        if (isActive)
        {
            fuelAmount = 100;
            GameController.GetInstance().fuel.fillAmount = 1;

            selectedWeapon = activeWeapons[0];
            MAX_SPEED = selectedWeapon.power;
            WeaponButton.Instance.UpdateView(selectedWeapon);
            GameController.Instance.PrepareWeaponList(usedWeaponIds);
        }

        if (!isAI)
        {
            projectileHolder.gameObject.SetActive(isActive);
            canvas.gameObject.SetActive(isActive);
        }

    }

    public void HitMe(float demageAmount)
    {
        if (GameController.Instance.isGameover) return;
        health -= demageAmount;
        healthbar.Demage(health);

        if (health <= 0)
        {
            StartCoroutine(IGameOver());
        }
    }

    public void SetWeapon(RowWeapon weapon)
    {
        selectedWeapon = weapon;
        MAX_SPEED = selectedWeapon.power;
    }

    private IEnumerator IStartAI()
    {
        yield return new WaitForSeconds(2);
        float angle = 0;

        Vector2[] posiblePositions = new Vector2[4];
        Transform enemy = GameController.Instance.player1.transform;

        float distance = Mathf.Abs(enemy.position.x - transform.position.x);


        float time = Random.Range(2, 6);
        float acc = distance > 20 ? -1 : 1;

        if (GameData.playerState.isSound)
            audioSource.Play();
        JointMotor2D motor = new JointMotor2D { motorSpeed = carSpeed * acc, maxMotorTorque = 10000 };
        while (time > 0)
        {

            frontWheel.motor = motor;
            backWheel.motor = motor;
            midleWheel.motor = motor;
            time -= Time.deltaTime;
            yield return 0;

        }
        acc = 0;
        motor = new JointMotor2D { motorSpeed = carSpeed * acc, maxMotorTorque = 10000 };
        frontWheel.motor = motor;
        backWheel.motor = motor;
        midleWheel.motor = motor;

        if (audioSource.isPlaying)
            audioSource.Stop();


        posiblePositions[0] = new Vector2(enemy.position.x - 3, enemy.position.y);
        posiblePositions[1] = new Vector2(enemy.position.x, enemy.position.y);
        posiblePositions[2] = new Vector2(enemy.position.x + 3, enemy.position.y);
        posiblePositions[3] = new Vector2(enemy.position.x + 5, enemy.position.y);
        int pos = Random.Range(0, posiblePositions.Length);

        Vector2 target = posiblePositions[pos];
        Vector2 velocity = Calculate(target);

        angle = Vector2.Angle(Vector2.right, velocity);

        turret.eulerAngles = Vector3.forward * angle;

        yield return new WaitForSeconds(0.5f);



        int index = Random.Range(0, activeWeapons.Count);
        selectedWeapon = activeWeapons[index];
        usedWeaponIds.Add(selectedWeapon.id);
        activeWeapons.RemoveAt(index);

        LAUNCH_VELOCITY = velocity;

        Fire();
    }

    private IEnumerator IGameOver()
    {
        print("Game over");
        GameController.Instance.isGameover = true;

        //big particles
        //blast tank
        //rip image from top
        GameController.Instance.GameOver(playerType);
        CameraController.Instance.Shake(2);
        GameObject go = Instantiate(gameoverParticle, transform.position, Quaternion.identity);
        transform.position = new Vector3(-50, 0, 0);
        yield return new WaitForSeconds(2);
        Destroy(go);
        GameController.Instance.ExitTo(ScreenTransController.STAGE.GAMEOVER);

    }

    //for RnD 
    private Vector2 CalculateInitVelocity(Vector2 target, Vector2 origin, float t)
    {
        Vector2 result = Vector2.zero;
        Vector2 distance = (target - origin);
        float s = distance.magnitude;

        //create float represent distance
        float Sy = distance.y;
        float Sx = distance.magnitude;

        float Vx = Sx / t;
        float Vy = Sy / t + 0.5f * 9.8f * t;

        result = distance.normalized;
        result *= Vx;
        result.y = Vy;

        return result;
    }
}
