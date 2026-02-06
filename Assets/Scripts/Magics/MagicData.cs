using UnityEngine;
using System.Collections.Generic;

// 캐스트에 사용할 키 종류 정의
public enum CastKey {Q, E, R, Shift}

[CreateAssetMenu(fileName = "Cast", menuName = "Scriptable Objects/Cast")]
public class MagicData : ScriptableObject
{
    public string magicName; 
    public Sprite magicIcon; // UI
  

    [Header("Cast Pattern")]
    public List<CastKey> castPattern; // 캐스트 입력 순서

    [Header("Cast Time Limit")]
    public float castLimitTime = 5.0f;

    // 스킬 선택 시 커서 색상 변경(일단 보류)
    //[Header("Cursor Settings")]
    //public Color skillThemeColor = Color.white;

    [Header("Magics Common Stats")]
    public float magicDamage; // 
    public float fireRate = 0.5f; // 발사 간격

    [Header("Magics Logic")]
    public CallMagics callMagic; // 마법을 불러오는 하나의 함수 호출
    public GameObject MagicPrefab; // 사용할 스킬의 프리팹

}
