using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ConstantsSpace;

public class HealthBarManager : MonoBehaviour
{
    public GameObject healthbarParent;
    public GameObject healthbarOverbar;
    public GameObject healthbarUnderbar;
    public RectTransform canvasRect;

    private CanvasGroup healthbarParent_group;
    private RectTransform healthbarParent_transform;
    private RectTransform healthbarOverbar_transform;
    private RectTransform healthbarUnderbar_transform;
    // private Image healthbarOverbar_image;
    private Image healthbarUnderbar_image;

    private Camera mainCam;

    private byte healthbarState = Constants.healthbarState_idle;

    private const float barWidth_max = 122f;
    // private float barWidth_current = barWidth_max;
    // private float barWidth_transitional = barWidth_max;

    private Vector2 barSize_overbar = new Vector2(122f, 16f);
    private Vector2 barSize_underbar = new Vector2(122f, 16f);

    private const byte preMoveFrames_max_healed = 30;
    private const byte preMoveFrames_max_damaged = 45;
    private const byte preMoveFrames_max_fade = 60;
    private byte preMoveFrames_current = 0;

    private const byte barMoveFrames_max = 30;
    private byte barMoveFrames_current = 0;

    private const byte barFadeOutFrames_max = 15;
    private byte barFadeOutFrames_current = 0;
    private bool healthBarIsVisible = false;

    // private const float barMovePerFrame = barWidth_max / 3f / barMoveFrames_max;
    private float barMovePerFrame;
    private float barFadePerFrame = 1f / barFadeOutFrames_max;

    private const byte health_max = 3;
    private byte health_current = health_max;

    private Vector3 healthBarWorldPoint;
    private Vector3 healthBarOffset = new Vector3(1.5f, 0f, -1.5f);

    private Vector3 healthBarScreenPoint_3D;
    private Vector2 healthBarScreenPoint_2D = Vector2.zero;
    private Vector2 healthBarCanvasPoint = Vector2.zero;

    private Color color_healed = new Color32(240, 240, 240, 255);
    private Color color_damaged = new Color32(255, 115, 115, 255);



    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        healthbarParent_group = healthbarParent.GetComponent<CanvasGroup>();
        healthbarParent_transform = healthbarParent.GetComponent<RectTransform>();
        healthbarOverbar_transform = healthbarOverbar.GetComponent<RectTransform>();
        healthbarUnderbar_transform = healthbarUnderbar.GetComponent<RectTransform>();
        // healthbarOverbar_image = healthbarOverbar.GetComponent<Image>();
        healthbarUnderbar_image = healthbarUnderbar.GetComponent<Image>();
    }

    
    public void updateHealthBar(Vector3 boatPos)
    {
        if (healthBarIsVisible)
            updateHealthbarPosition(boatPos);

        switch (healthbarState)
        {
            case Constants.healthbarState_damaged:
                takeNextStep_damaged();
                break;

            case Constants.healthbarState_healed:
                takeNextStep_healed();
                break;

            case Constants.healthbarState_fadingOut:
                takeNextStep_fadingOut();
                break;

            default: 
                break;
        }
    }


    private void updateHealthbarPosition(Vector3 boatPos)
    {
        healthBarWorldPoint = boatPos + healthBarOffset;

        healthBarScreenPoint_3D = mainCam.WorldToScreenPoint(healthBarWorldPoint);
        healthBarScreenPoint_2D.x = healthBarScreenPoint_3D.x;
        healthBarScreenPoint_2D.y = healthBarScreenPoint_3D.y;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, healthBarScreenPoint_2D, null, out healthBarCanvasPoint);

        // healthbarParent_transform.anchoredPosition = healthBarCanvasPoint + healthBarOffset;
        healthbarParent_transform.anchoredPosition = healthBarCanvasPoint;
    }


    private void takeNextStep_damaged()
    {
        if (preMoveFrames_current > 0)
        {
            preMoveFrames_current--;
        } else if (barMoveFrames_current > 0)
        {
            barMoveFrames_current--;
            if (barMoveFrames_current > 0)
            {
                barSize_underbar.x -= barMovePerFrame;
                healthbarUnderbar_transform.sizeDelta = barSize_underbar;
            } else {
                changeHealthbarState_idle();
            }
        }
    }


    private void takeNextStep_healed()
    {
        if (preMoveFrames_current > 0)
        {
            preMoveFrames_current--;
        } else if (barMoveFrames_current > 0)
        {
            barMoveFrames_current--;
            if (barMoveFrames_current > 0)
            {
                barSize_overbar.x += barMovePerFrame;
                healthbarOverbar_transform.sizeDelta = barSize_overbar;
            } else {
                changeHealthbarState_idle();
            }
        }
    }


    private void takeNextStep_fadingOut()
    {
        if (preMoveFrames_current > 0)
        {
            preMoveFrames_current--;
        } else if (barFadeOutFrames_current > 0)
        {
            barFadeOutFrames_current--;
            if (barFadeOutFrames_current > 0)
            {
                healthbarParent_group.alpha -= barFadePerFrame;
            } else {
                changeHealthbarState_invisible();
            }
        }
    }


    public void playerHasBeenDamaged()
    {
        if (health_current == health_max)
        {
            healthbarParent_group.alpha = 1f;
            healthBarIsVisible = true;
        }

        healthbarState = Constants.healthbarState_damaged;

        barMovePerFrame = barWidth_max / 3f / barMoveFrames_max;

        // Change the bar's width:
        barSize_overbar.x = ((float)health_current-1) / health_max * barWidth_max;
        barSize_underbar.x = ((float)health_current) / health_max * barWidth_max;

        healthbarOverbar_transform.sizeDelta = barSize_overbar;
        healthbarUnderbar_transform.sizeDelta = barSize_underbar;

        // Change the bar's color:
        healthbarUnderbar_image.color = color_damaged;

        preMoveFrames_current = preMoveFrames_max_damaged;
        barMoveFrames_current = barMoveFrames_max;
        health_current--;
    }


    public void playerHasBeenHealed(byte health_new)
    {
        if (health_new != health_current)
        {
            healthbarState = Constants.healthbarState_healed;

            barMovePerFrame = barWidth_max * (health_new-health_current) / 3f / barMoveFrames_max;

            // Change the bar's width:
            barSize_overbar.x = ((float)health_current) / health_max * barWidth_max;
            barSize_underbar.x = ((float)health_new) / health_max * barWidth_max;

            healthbarOverbar_transform.sizeDelta = barSize_overbar;
            healthbarUnderbar_transform.sizeDelta = barSize_underbar;

            // Change the bar's color:
            healthbarUnderbar_image.color = color_healed;

            preMoveFrames_current = preMoveFrames_max_healed;
            // barMoveFrames_current = (byte)(barMoveFrames_max * health_new-health_current);
            barMoveFrames_current = barMoveFrames_max;
            health_current = health_new;
        }
    }


    private void changeHealthbarState_idle()
    {
        // healthbarState = Constants.healthbarState_idle;

        // Change the bar's width:
        barSize_overbar.x = ((float)health_current) / health_max * barWidth_max;
        barSize_underbar.x = 0;

        healthbarOverbar_transform.sizeDelta = barSize_overbar;
        healthbarUnderbar_transform.sizeDelta = barSize_underbar;


        if (healthBarIsVisible && health_current == health_max)
        {
            preMoveFrames_current = preMoveFrames_max_fade;
            barFadeOutFrames_current = barFadeOutFrames_max;
            healthbarState = Constants.healthbarState_fadingOut;
        } else {
            healthbarState = Constants.healthbarState_idle;
        }
    }


    private void changeHealthbarState_invisible()
    {
        healthbarParent_group.alpha = 0f;
        healthBarIsVisible = false;

        changeHealthbarState_idle();
    }
}
