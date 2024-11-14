using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public static List<string> Directions = new() { "None", "Up", "Down", "Left", "Right" };

    [SerializeField] private GameObject trigger;
    [SerializeField] private Transform root;
    [SerializeField] private Transform infoBlock;
    [SerializeField] private Transform healthBar;
    [SerializeField] private Transform energyBar;
    [SerializeField] private TMPro.TextMeshPro healthToast;
    [SerializeField] private TMPro.TextMeshPro energyToast;
    [SerializeField] private TMPro.TextMeshPro unitNameLabel;

    public string address;
    public string unitName;
    public int initHealth;
    public int initEnergy;
    public string team;
    //
    public Vector3Int lastPosition;
    public int lastHealth;
    public int lastEnergy;

    public bool dead;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger entered on " + name);
        SetAction(0);
    }

    private void Start()
    {
        Debug.Log("UnitController.Start()");
        infoBlock.eulerAngles = new();
        trigger.SetActive(false);

        unitNameLabel.text = unitName;
        healthToast.gameObject.SetActive(false);
        energyToast.gameObject.SetActive(false);
    }

    public void WaitFinishStep()
    {
        Invoke("FinishStep", 1f);
    }

    private void FinishStep()
    {
        healthToast.gameObject.SetActive(false);
        energyToast.gameObject.SetActive(false);
    }

    public void SetPosition(Vector3Int position)
    {
        if (dead) return;
        this.transform.localPosition = position;
    }

    public void SetHealth(int value)
    {
        if (dead) return;
        if (value != lastHealth)
        {
            healthToast.gameObject.SetActive(true);
            healthToast.text = (value - lastHealth) + "hp";
        }
        healthBar.localScale = new Vector3(0.8f * (float)value / initHealth, 0.1f, 1f);
        lastHealth = value;
        if (value < 0.0001f) Die();
    }

    public void SetEnergy(int value)
    {
        if (dead) return;
        if (value != lastEnergy)
        {
            energyToast.gameObject.SetActive(true);
            energyToast.text = (value - lastEnergy) + "nrg";
        }
        energyBar.localScale = new Vector3(0.8f * (float)value / initEnergy, 0.1f, 1f);
        lastEnergy = value;
    }

    public void SetDirection(string value)
    {
        if (dead) return;
        switch (value)
        {
            case "Up":
                //up
                transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case "Down":
                //down
                transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case "Left":
                //left
                transform.localEulerAngles = new Vector3(0, 270, 0);
                break;
            case "Right":
                //right
                transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
        }
        infoBlock.eulerAngles = new();
        healthToast.gameObject.SetActive(false);
        energyToast.gameObject.SetActive(false);
    }

    public void SetDirection(int value)
    {
        if (dead) return;
        switch (value)
        {
            case 1:
                //up
                transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case 2:
                //down
                transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case 3:
                //left
                transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case 4:
                //right
                transform.localEulerAngles = new Vector3(0, 270, 0);
                break;
        }
        infoBlock.eulerAngles = new();
        healthToast.gameObject.SetActive(false);
        energyToast.gameObject.SetActive(false);
    }

    public void SetAction(int value)
    {
        if (dead) return;
        switch (value)
        {
            case 0:
                Debug.Log("character is beaten");
                //beaten
                GetComponent<Animator>().SetTrigger("Wound");
                break;
            case 1:
                Debug.Log("character attacks quick");
                //quick attack
                GetComponent<Animator>().SetTrigger("Hit");
                trigger.SetActive(true);
                break;
            case 2:
                Debug.Log("character attacks precise");
                GetComponent<Animator>().SetTrigger("Hit2");
                trigger.SetActive(true);
                //precise attack
                break;
            case 3:
                Debug.Log("character heavily attacks");
                GetComponent<Animator>().SetTrigger("Attack");
                trigger.SetActive(true);
                //heavy attack
                break;
            case 4:
                Debug.Log("character moves");
                //move
                GetComponent<Animator>().SetBool("Walking", true);
                StartCoroutine(MoveForwardCoroutine());
                break;
            case 5:
                Debug.Log("character is taking rest");
                //rest
                break;
        }
        Invoke("StopAction", 1f);
    }

    private void Die()
    {
        if (dead) return;
        dead = true;
        infoBlock.gameObject.SetActive(false);
        GetComponent<Animator>().SetTrigger("Die");
    }

    private void StopAction()
    {
        GetComponent<Animator>().SetBool("Walking", false);
        trigger.SetActive(false);
    }

    IEnumerator MoveForwardCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            // Move the transform forward by speed per frame, adjusted by deltaTime
            transform.Translate(Vector3.forward * 1f * Time.deltaTime);
            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            // Wait until the next frame
            yield return null;
        }
    }
}
