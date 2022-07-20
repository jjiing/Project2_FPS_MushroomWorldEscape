using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public float range; //사정거리
    public float fireRate; //연사속도
    public float currentFireRate;
    public float reloadTime; //재장전 속도

    float bulletMax;
    public float bulletCurrent;


    int damage;


    public Vector3 originPos;
    public Vector3 fineSightPos; //정조준시 총 위치

    public Transform weaponMuzzle;
    public Transform powerShotPoint;
    public GameObject muzzleFlash;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public Animator anim;
    private AudioSource audioSource;
    public Camera cam;
    public GameObject hit_effect;
    public LineRenderer bulletOrbit_lazer;
    public GameObject aimPoint;


    private float fuelCellRatio;
    public static bool isPaused;





    public float BulletCurrentRatio { get; set; }

    public bool isReload;
    public bool isFineSightMode;
    public bool isFire;

    GameObject player;
    PlayerMove playerMove;


    private RaycastHit hit; //레이저 충돌 정보 받아옴
    public float FuelCellRatio
    {
        get {  return fuelCellRatio; }
        set { fuelCellRatio = value; }
    }    


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMove = player.GetComponent<PlayerMove>();
        range = 200;
        fireRate = 0.2f;
        reloadTime = 1f;
        damage = 30;
        bulletMax = 10;
        bulletCurrent = bulletMax;

        isReload = false;
        isFineSightMode = false;
        isFire = false;


        fuelCellRatio=1;

        isPaused = false;

        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        


    }


    void Update()
    {
        
        TryFire();
        TryReload();
        TryFineSight();
        GunAnim();

        UpdateBullet();
    }

    private void UpdateBullet()
    {

        BulletCurrentRatio = bulletCurrent / bulletMax;
        

    }


    private void GunFireRateCalc()
    {
        //연사속도계산
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;//1초에 1 감소시킨다.
    }

    private void TryFire()
    {
        GunFireRateCalc();
        if (!isReload && !isPaused)
            if (bulletCurrent > 0)
            {
                if (Input.GetButton("Fire1") && currentFireRate <= 0)
                {
                    isFire = true;
                    Fire();
                    
                }
                if (Input.GetButtonUp("Fire1"))
                    isFire = false;
            }
            else
                StartCoroutine(ReloadCo());
        
    }
    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R)&& !isReload && bulletCurrent <bulletMax)
        {
            StartCoroutine(ReloadCo());
        }
    }


    private void Fire()
    {
        currentFireRate = fireRate; //연사속도제어변수 초기화
        bulletCurrent--;


        //muzzle 초록색 플래쉬 이펙트
        GameObject muzzleFlashInstance = Instantiate(muzzleFlash, weaponMuzzle.position,
            weaponMuzzle.rotation, weaponMuzzle.transform);
        muzzleFlashInstance.transform.SetParent(weaponMuzzle); //복제를 gun muzzle의 자식으로
        Destroy(muzzleFlashInstance, 2f);
        //사운드
        PlaySE(fireSound);
        Hit();

    }

    private void Hit()
    {
        Vector3 lazerStartPos = weaponMuzzle.transform.position;
        bulletOrbit_lazer.startWidth = 0.1f;
        bulletOrbit_lazer.endWidth = 0.2f;
        bulletOrbit_lazer.SetPosition(0, weaponMuzzle.transform.position);
        bulletOrbit_lazer.SetPosition(1, aimPoint.transform.position);
        bulletOrbit_lazer.gameObject.SetActive(true);
        StartCoroutine(bulletOrbitDeactiveCo());







        //int layerMask =1 << 7;
        //layerMask = ~layerMask;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (!hit.transform.CompareTag("Wall"))
                {
                var clone = Instantiate(hit_effect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(clone, 2f);
            }

            //레이저 - 나중에 바꿔야할듯 

            //Debug.Log("레이 충돌체 : " + hit.collider.gameObject.name);

            if (hit.transform.CompareTag("Monster"))
            {
                hit.transform.GetComponent<Monster>().Hp -= damage;
                hit.transform.GetComponent<Monster>().isDamaged = true;
                if (hit.transform.GetComponent<Monster>().isDead ==false)
                    fuelCellRatio -= 0.08f;
            }

        }
        
        
        
    }
    IEnumerator bulletOrbitDeactiveCo()
    {
        yield return new WaitForSeconds(0.05f);
        bulletOrbit_lazer.gameObject.SetActive(false);
    }
    
    //재장전
    IEnumerator ReloadCo()
    {
        isReload = true;
        anim.SetTrigger("isReload");
        PlaySE(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        StopSE(reloadSound);
        bulletCurrent = bulletMax;
        isReload = false;
        isFire = false;

    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    private void StopSE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Stop();
    }

    private void TryFineSight()
    {
        if (Input.GetButton("Fire2"))
        {
            isFineSightMode = true;
            cam.fieldOfView = 30;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            isFineSightMode = false;
            cam.fieldOfView = 45;
        }



    }
   

    private void GunAnim()
    {
        anim.SetBool("isWalk", playerMove.isWalk);
        anim.SetBool("isRun", playerMove.isRun);
        anim.SetBool("isFire", isFire);
        anim.SetBool("isFineSightMode", isFineSightMode);
        anim.SetBool("isSlow", playerMove.isSlow);

    }


}

