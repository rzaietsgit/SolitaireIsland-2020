using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ITSoft
{
    public class GuideSystem : MonoBehaviour
    {

        public GameObject panel;
        
        public Button clickButton;
        private RectTransform clickArea;
        public Image maskImage;

        private Vector2 guideMaskSize;
        private Vector3 guideMaskWorldPos;

        private Action onClickEvent = null;
        
        private void Awake()
        {
            clickButton.onClick.AddListener(OnClickArea);
            maskImage.GetComponent<Button>().onClick.AddListener(OnOtherAreaClick);
            clickArea = clickButton.transform as RectTransform;
        }

        public void SetClickAction(UnityAction action)
        {
            onClickEvent = () =>
            {
                action?.Invoke();
            };
        }
        
        private void OnClickArea()
        {
            isClickGuideArea = true;
            onClickEvent?.Invoke();
            // panel.SetActive(false);
        }

        public void ShowHelp(Vector3 position, Vector2 size)
        {
            guideMaskWorldPos = position;
            guideMaskSize = size;
            SendPanelShowArgs();
        }
        
        private Vector3 GetGuideMaskArgs(out Vector2 size)
        {
            size = guideMaskSize;
            return guideMaskWorldPos;
        }
        
        private void OnOtherAreaClick()
        {
            onClickEvent?.Invoke();
            // panel.SetActive(false);
            //UIManager.HidePanel(gameObject);
        }

        public void SendArgs(int sourcePanelIndex, params int[] args)
        {
        }

        // int guideStep;
        int sourcePanelIndex = -1;
        bool isClickGuideArea = false;

        public void SendPanelShowArgs(params int[] args)
        {
            isClickGuideArea = false;
            Vector3 worldPos = GetGuideMaskArgs(out Vector2 size);
            size.x += 50;
            size.y += 50;
            Vector3 localPos = SetGuideMaskPos(worldPos, size);
            // switch (guideStep)
            // {
            //     case (int) GuideType.Cashout_Cash:
            //         GameManager.Instance.AddGuideStep();
            //         cashoutGuideImage.gameObject.SetActive(true);
            //         cashoutGuideArrowImage.gameObject.SetActive(true);
            //         cursorRect.gameObject.SetActive(false);
            //         dialogueRect.gameObject.SetActive(false);
            //         cashoutGudieArrowRect.localPosition = new Vector3(localPos.x,
            //             localPos.y - 150 - cashoutGudieArrowRect.sizeDelta.y / 2f);
            //         cashoutGuideRect.localPosition = new Vector3(0,
            //             cashoutGudieArrowRect.localPosition.y - cashoutGudieArrowRect.sizeDelta.y / 2f -
            //             cashoutGuideRect.sizeDelta.y / 2f + 9.6239f);
            //         cashoutGuideImage.sprite = SpriteManager.GetSprite(SpriteAtlas_Name.Guide, "cashoutguide1");
            //         cashoutGuideArrowImage.sprite =
            //             SpriteManager.GetSprite(SpriteAtlas_Name.Guide, "cashoutguide1_arrow");
            //         break;
            //     case (int) GuideType.Cashout_Amazon:
            //         GameManager.Instance.AddGuideStep();
            //         cashoutGuideImage.gameObject.SetActive(true);
            //         cashoutGuideArrowImage.gameObject.SetActive(true);
            //         cursorRect.gameObject.SetActive(false);
            //         dialogueRect.gameObject.SetActive(false);
            //         cashoutGudieArrowRect.localPosition = new Vector3(localPos.x,
            //             localPos.y - 150 - cashoutGudieArrowRect.sizeDelta.y / 2f);
            //         cashoutGuideRect.localPosition = new Vector3(0,
            //             cashoutGudieArrowRect.localPosition.y - cashoutGudieArrowRect.sizeDelta.y / 2f -
            //             cashoutGuideRect.sizeDelta.y / 2f + 9.6239f);
            //         cashoutGuideImage.sprite = SpriteManager.GetSprite(SpriteAtlas_Name.Guide, "cashoutguide2");
            //         cashoutGuideArrowImage.sprite =
            //             SpriteManager.GetSprite(SpriteAtlas_Name.Guide, "cashoutguide2_arrow");
            //         break;
            //     case (int) GuideType.GameGuide_ClickCard:
            //         GameManager.Instance.AddGuideStep();
            //         cashoutGuideImage.gameObject.SetActive(false);
            //         cashoutGuideArrowImage.gameObject.SetActive(false);
            //         cursorImage.sprite = SpriteManager.GetSprite(SpriteAtlas_Name.Guide, "cursor");
            //         cursorImage.SetNativeSize();
            //         cursorRect.gameObject.SetActive(true);
            //         dialogueRect.gameObject.SetActive(false);
            //         cursorRect.localPosition = new Vector3(localPos.x + size.x / 2f, localPos.y);
            //         StartCoroutine(AutoMoveCursor(true));
            //         break;
            //     case (int) GuideType.GameGuide_ExtraProp:
            //         GameManager.Instance.AddGuideStep();
            //         cashoutGuideImage.gameObject.SetActive(false);
            //         cashoutGuideArrowImage.gameObject.SetActive(false);
            //         cursorRect.gameObject.SetActive(false);
            //         dialogueRect.gameObject.SetActive(true);
            //         dialogueRect.localPosition = new Vector3(localPos.x + size.x / 2f, localPos.y);
            //         break;
            //     case (int) GuideType.Unlock_Pig:
            //     case (int) GuideType.Unlock_Task:
            //     case (int) GuideType.Unlock_Giveaways:
            //     case (int) GuideType.Pt_Earn:
            //         cashoutGuideImage.gameObject.SetActive(false);
            //         cashoutGuideArrowImage.gameObject.SetActive(false);
            //         cursorImage.sprite = SpriteManager.GetSprite(SpriteAtlas_Name.Guide, "hand");
            //         cursorImage.SetNativeSize();
            //         cursorRect.gameObject.SetActive(true);
            //         dialogueRect.gameObject.SetActive(false);
            //         if (guideStep == (int) GuideType.Unlock_Giveaways)
            //         {
            //             cursorRect.localPosition = new Vector3(localPos.x - size.x / 2f, localPos.y);
            //             cursorRect.localScale = new Vector3(-1, 1, 1);
            //             StartCoroutine(AutoMoveCursor(false));
            //         }
            //         else
            //         {
            //             cursorRect.localPosition = new Vector3(localPos.x + size.x / 2f, localPos.y);
            //             cursorRect.localScale = Vector3.one;
            //             StartCoroutine(AutoMoveCursor(true));
            //         }
            //
            //         StartCoroutine(AutoHideGuide());
            //         break;
            //     default:
            //         break;
            // }

            clickArea.localPosition = localPos;
            clickArea.sizeDelta = size;
            panel.SetActive(true);
        }

        // private IEnumerator AutoMoveCursor(bool isRight)
        // {
        //     Vector3 startPos = cursorRect.localPosition;
        //     Vector3 endPos = new Vector3(startPos.x + (isRight ? 100 : -100), startPos.y, startPos.z);
        //     float timer = 0;
        //     float speed = 2;
        //     bool isIncrease = true;
        //     while (true)
        //     {
        //         yield return null;
        //         timer += (isIncrease ? speed : -speed) * Time.deltaTime;
        //         if (timer >= 1 && isIncrease)
        //         {
        //             timer = 1;
        //             isIncrease = false;
        //         }
        //         else if (timer <= 0 && !isIncrease)
        //         {
        //             timer = 0;
        //             isIncrease = true;
        //         }
        //
        //         cursorRect.localPosition = Vector3.Lerp(startPos, endPos, timer);
        //     }
        // }

        // private IEnumerator AutoHideGuide()
        // {
        //     yield return new WaitForSeconds(3);
        //     OnOtherAreaClick();
        // }

        private Vector3 SetGuideMaskPos(Vector3 worldPos, Vector2 size)
        {
            Material material = maskImage.material;
            var rect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            worldPos = Camera.main.WorldToViewportPoint(worldPos);
            worldPos.x *= rect.sizeDelta.x;
            worldPos.y *= rect.sizeDelta.y;
            worldPos.x -= rect.sizeDelta.x * 0.5f;
            worldPos.y -= rect.sizeDelta.y * 0.5f;
            material.SetVector("_Center", worldPos);
            material.SetFloat("_Width", size.x * 0.5f);
            material.SetFloat("_Height", size.y * 0.5f);
            return worldPos;
        }
    }

    public enum GuidType
    {
        
    }
}