using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityFX : MonoBehaviour
{

    private SpriteRenderer sr;
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;



    [Header("Ailment colors")]
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color=Color.white;

        yield return new WaitForSeconds(.2f);

        sr.color = currentColor;
        sr.material = originalMat;


    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
            sr.color = Color.red;
    }
    private void ChillColorFX()
    {
        sr.color = chillColor;
    }
    public void ShockFXFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }
    public void ChillFXFor(float _seconds)
    {
        Invoke("ChillColorFX", .21f);
        Invoke("CancelColorChange", _seconds);
    }
    public void IgniteFXFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

}
