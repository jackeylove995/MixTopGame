using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform background; // 摇杆背景
    private RectTransform handle; // 摇杆手柄
    private Vector2 touchPosition;
    private Vector2 inputDircetion;
    public static event Action OnBeginMove;
    public static event Action<float, float> OnMove;
    public static event Action OnEndMove;
    private void Awake()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>(); // 摇杆手柄是背景的子元素
    }
    void FixedUpdate()
    {
        if (inputDircetion != Vector2.zero)
        {
            OnMove?.Invoke(inputDircetion.x, inputDircetion.y);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            // 获取触摸位置相对于摇杆背景的百分比
            touchPosition.x = (touchPosition.x / background.sizeDelta.x) * 2;
            touchPosition.y = (touchPosition.y / background.sizeDelta.y) * 2;
            bool normalized = touchPosition.magnitude > 1f;
            touchPosition = normalized ? touchPosition.normalized : touchPosition;
            // touchPosition = touchPosition.normalized;
            // 更新摇杆手柄的位置
            handle.anchoredPosition = new Vector2(touchPosition.x * (background.sizeDelta.x / 2), touchPosition.y * (background.sizeDelta.y / 2));
            // 更新输入方向
            inputDircetion = normalized ? touchPosition : touchPosition.normalized;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnBeginMove?.Invoke();
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 重置摇杆位置和输入方向
        handle.anchoredPosition = Vector2.zero;
        inputDircetion = Vector2.zero;
        OnEndMove?.Invoke();
    }
}
