using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMove : MonoBehaviour
{
    private float turnSpeed; //마우스 회전 속도
    public float xRotate;   //내부 사용할 x축 회전량은 별도 정의(카메라 상/하)

    float walkSpeed;
    float runSpeed;
    float slowSpeed;
    float slowRunSpeed;
    public float currentSpeed;
    float jumpSpeed;
    float gravity;

    private Vector3 moveForce;

    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
   
    public float walkSoundTime;
    public float currentWalkSoundTime;

    CharacterController controller;
    public AudioSource audioSource;

    Vector3 moveDir;

    public bool isRun;
    public bool isWalk;
    public bool isSlow;

    float slowTimer;
    float currentSlowTimer;
    public Image blind;

   public float TurnSpeed
    {
        get {  return turnSpeed; }
        set { turnSpeed = value; }
    }
    public float CurrentSpeed
    {
        get { return currentSpeed; }
        set { currentSpeed = value; }
    }
    public float CurrentSlowTimer
    {
        get { return currentSlowTimer; }
        set { currentSlowTimer = value; }
    }
    public float SlowTimer
    {
        get { return slowTimer; }
    }


    private void Awake()
    {
        
        //마우스 커서 보이지 않게 설정하고, 현재 위치에 고정시킨다.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        controller = GetComponent<CharacterController>();
        //audioSource = GetComponent<AudioSource>();


        walkSpeed = 25f;
        runSpeed = walkSpeed *1.5f;
        slowSpeed = walkSpeed * 0.5f;
        slowRunSpeed = walkSpeed;
        currentSpeed = walkSpeed;
        jumpSpeed = 30f;
        gravity = 40;

        currentWalkSoundTime = 0.2f;

        turnSpeed = 0.8f;
        xRotate = 0f;

        isRun = false;
        isWalk = false;
        isSlow = false;

       

        slowTimer = 5f;
        CurrentSlowTimer = 0f;


    }
    
    void Update()
    {
        
        MouseRotation();
        KeyboardMove();
        SlowControl();
           

    }
    void SlowControl()
    {
        if (CurrentSlowTimer > 0)
        {
            Slow();
            isSlow = true;
        }
        else
        {
            currentSpeed = walkSpeed;
            isSlow = false;
            runSpeed = slowRunSpeed * 1.5f ;
        }
    }
    void Slow()
    {
        CurrentSlowTimer -= Time.deltaTime;
        currentSpeed = slowSpeed;
        runSpeed = slowRunSpeed;

    }

    private void MouseRotation()
    {

        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산

        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // 회전 각을 제한 (화면 밖으로 마우스가 나갔을때 x축 회전 덤블링 하듯 계속 도는 것을 방지)
        // Clamp 는 값의 범위를 제한하는 함수

        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)


    }

    private void KeyboardMove()
    {
        //걸을때만 뛸 수 있게 구현
        if (isWalk)
            CheckRun();
        else
            isRun = false;


        //이동(x,z값)
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = transform.rotation * new Vector3(x, 0, z);
        //이동 방향 = 캐릭터의 회전 값 * 방향 값
        //카메라 회전으로 전방 방향이 변해야 하기 때문에 회전 값을 곱한다.

        moveForce = new Vector3(direction.x * currentSpeed, moveDir.y, direction.z * currentSpeed);
        //이동 힘 = 이동방향 * 속도


        controller.Move(moveForce * Time.deltaTime);
        //1초당 moveForce 속력으로 이동


        //y값 설정 : 중력, 점프
        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDir.y = jumpSpeed;
                StartCoroutine(JumpSoundCo());
            }
        }
        moveDir.y -= gravity * Time.deltaTime;
        // 중력의 영향을 받아 아래쪽으로 하강


        //isWalk 컨트롤
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            isWalk = true;

        else
            isWalk = false;

        if(!audioSource.isPlaying&& controller.isGrounded)
            WalkSound();
    }
    private void CheckRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            Running(true);
        if (Input.GetKeyUp(KeyCode.LeftShift))
            Running(false); //running cancel
    }
    private void Running(bool run)
    {
        isRun = run;

        if (run)
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;
    }
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    private void PlayDelaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.PlayDelayed(0.1f);
    }
    private void PlayDelay2SE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.PlayDelayed(0.25f);
    }
    private void StopSE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Stop();
    }
    IEnumerator JumpSoundCo()
    {
        PlaySE(jumpSound);
        yield return new WaitForSeconds(1.5f);
        PlaySE(landSound);
    }
    private void WalkSound()
    {
        if (!isSlow && isWalk && isRun)
            PlaySE(walkSound);
        else if (!isSlow && isWalk && !isRun || isSlow && isWalk && isRun)
            PlayDelaySE(walkSound);
        else if (isSlow && isWalk && !isRun)
            PlayDelay2SE(walkSound);
        else
            StopSE(walkSound);

    }

  public void   Blind()
    {
        blind.gameObject.SetActive(true);
        Invoke("BlindOff", 2.5f);
    }

    private void BlindOff()
    {
        blind.gameObject.SetActive(false);
    }



}
