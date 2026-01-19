using UnityEngine;
using System.Collections.Generic;

// 캐스트에 사용할 키 종류 정의
public enum CastKey {Q, E, R, Shift}

[CreateAssetMenu(fileName = "Cast", menuName = "Scriptable Objects/Cast")]
public class SkillData : ScriptableObject
{
    public string skillName; 
    public Sprite skillIcon; // UI
    public GameObject projectilePrefab; // 투사체 프리팹
    public float fireRate = 0.5f; // 발사 간격

    [Header("Cast Pattern")]
    public List<CastKey> castPattern; // 캐스트 입력 순서

    [Header("Cast Time Limit")]
    public float castLimitTime = 5.0f;

    // 스킬 선택 시 커서 색상 변경(일단 보류)
    //[Header("Cursor Settings")]
    //public Color skillThemeColor = Color.white;


}
