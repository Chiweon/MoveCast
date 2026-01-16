using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [Header("Data Settings")]
    [SerializeField] private CursorData currentCursor;

    [Header("Cursor UI follow speed")]
    [SerializeField] private float followSpeed = 20f;

    private Image cursorImage;
    private RectTransform rectTransform;
    
    void Awake()
    {
        cursorImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        
        ApplyCursorData(); //CursorData에 있는 스프라이트를 컴포넌트에 전달 및 적용
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; //커서를 화면 밖으로 나가는 것을 제한 
    }

    void LateUpdate()
    {
        if(Mouse.current != null)
        {
            // 커서 UI가 즉각적으로 하드웨어 커서를 따라갈려면 아래 줄 주석 제거 후 1, 2번 코드 주석처리
            //rectTransform.position = Mouse.current.position.ReadValue();
            Vector2 mousePos = Mouse.current.position.ReadValue(); // 1.
            rectTransform.position = Vector3.Lerp(rectTransform.position, mousePos, followSpeed);
        }

        // 커서 회전
        if(currentCursor != null && currentCursor.rotationSpeed != 0)
        {
            rectTransform.Rotate(Vector3.forward,currentCursor.rotationSpeed * Time.deltaTime);
        }
    }

    public void ChangeCursor(CursorData newData)
    {
        currentCursor = newData;
        ApplyCursorData();
    }

    private void ApplyCursorData()
    {
        if (currentCursor != null && currentCursor.cursorImage != null)
        {
            cursorImage.sprite = currentCursor.cursorImage;
            cursorImage.color = currentCursor.cursorColor;

            Texture2D texture = currentCursor.cursorImage.texture;
            Vector2 hotspot = new Vector2(texture.width / 2f, texture.height / 2f);
            Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
        }
    }
}
