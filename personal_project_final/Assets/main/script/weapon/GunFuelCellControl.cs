using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFuelCellControl : MonoBehaviour
{
    public GameObject[] FuelCells;
    public Vector3 FuelCellUsedPosition;
    public Vector3 FuelCellUnusedPosition;

    Gun gun;
    public GameObject powerShotEffect;
    public GameObject powerShotFlash;

    public float fuelCellRatio;
    public AudioSource audioSource;
    public AudioClip powerShotFire;


    void Start()
    {
        FuelCellUsedPosition = new Vector3(0f, 0.1f, 0f);
        FuelCellUnusedPosition = new Vector3(0f, 0f, 0f);

        gun = GetComponent<Gun>();
        fuelCellRatio = 1;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UseBulletFuelCell();

        if (gun.FuelCellRatio <= 0 && !gun.isReload)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                PowerShot();
        }


    }
    private void PowerShot()
    {

        // 피격사운드 넣기
        audioSource.clip = powerShotFire;
        audioSource.Play();
        var flashInstance = Instantiate(powerShotFlash, gun.weaponMuzzle.position, gun.weaponMuzzle.rotation);
        flashInstance.transform.forward = gameObject.transform.forward;
        var flashPs = flashInstance.GetComponent<ParticleSystem>();
        Destroy(flashInstance, flashPs.main.duration);
        GameObject powerShot = Instantiate(powerShotEffect, gun.powerShotPoint.position, gun.powerShotPoint.rotation);
        Rigidbody powerShotRigid = powerShot.GetComponent<Rigidbody>();
        powerShotRigid.velocity = transform.forward * 120f;
        gun.FuelCellRatio = 1;


    }
    
    //FuelCell을 부드럽게 이동시키기 위해 lerp랑 inversLerp사용
    private void UseBulletFuelCell()
    {
        for (int i = 0; i < FuelCells.Length; i++)
        {
            float length = FuelCells.Length;
            float lim1 = i / length;
            float lim2 = (i + 1) / length;

            float value = Mathf.InverseLerp(lim1, lim2, gun.FuelCellRatio);
            value = Mathf.Clamp01(value);
            FuelCells[i].transform.localPosition =
                Vector3.Lerp(FuelCellUsedPosition, FuelCellUnusedPosition, value);

        }
    }
    //private void ReloadFuelCell()
    
}
