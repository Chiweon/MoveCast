using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [Header("Skill Slots")]
    [SerializeField] private SkillData[] skillSlots = new SkillData[4];

    private List<CastKey> currentInputs = new List<CastKey>();

    private SkillData selectedSkill;
    private CursorController cursorController;
    private InputSystem_Actions inputActions;
    
    private bool isCasting = false;
    private bool isReadyToFire = false;
    private Coroutine skillProcessCoroutine;

    private void Awake()
    {
        cursorController = FindFirstObjectByType<CursorController>();
        inputActions = new InputSystem_Actions();

        // 스킬 선택 연결
        inputActions.Player.SelectSkill1.performed += ctx => SelectSkill(0);
        inputActions.Player.SelectSkill2.performed += ctx => SelectSkill(1);
        inputActions.Player.SelectSkill3.performed += ctx => SelectSkill(2);
        inputActions.Player.SelectSkill4.performed += ctx => SelectSkill(3);

        // 캐스트 키 입력 연결
        inputActions.Player.CastQ.performed += ctx => OnCastKeyPressed(CastKey.Q);
        inputActions.Player.CastE.performed += ctx => OnCastKeyPressed(CastKey.E);
        inputActions.Player.CastR.performed += ctx => OnCastKeyPressed(CastKey.R);
        inputActions.Player.CastShift.performed += ctx => OnCastKeyPressed(CastKey.Shift);

        // 발사 입력 연결
        inputActions.Player.Attack.performed += ctx => OnFire();
    }

    private void SelectSkill(int index)
    {
        if (index < skillSlots.Length && skillSlots[index] == null) return;

        if (isCasting || skillProcessCoroutine != null)
        {
            ResetCombatState();
        }

        selectedSkill = skillSlots[index];
        skillProcessCoroutine = StartCoroutine(SkillRoutine());
        Debug.Log($"<Color=cyan>[System]</color> {selectedSkill.skillName} selected!");
    }

    private void OnCastKeyPressed(CastKey key)
    {
        if (isCasting && !isReadyToFire)
        {
            currentInputs.Add(key);
            Debug.Log($"Key Input: {key}");
        }
    }

    private IEnumerator SkillRoutine()
    {
        isCasting = true;
        isReadyToFire = false;
        currentInputs.Clear();

        int nextCorrectIndex = 0;
        float timeoutTimer = 0;
        float limitTime = selectedSkill.castLimitTime;
        Debug.Log($"<color=yellow>[Casting]</color> {selectedSkill.skillName} (제한 시간: {limitTime}초)");

        while (nextCorrectIndex < selectedSkill.castPattern.Count)
        {
            timeoutTimer += Time.deltaTime;

            if (timeoutTimer > limitTime)
            {
                Debug.Log("<color=red>Time Over!</color>");
                ResetCombatState();
                yield break;
            }

            if (currentInputs.Count > nextCorrectIndex)
            {
                if (currentInputs[nextCorrectIndex] == selectedSkill.castPattern[nextCorrectIndex])
                {
                    nextCorrectIndex++;
                    Debug.Log($"Match! ({nextCorrectIndex}/{selectedSkill.castPattern.Count})");
                }
                else
                {
                    Debug.Log("<color=orange>Wrong Key! Reset.</color>");
                    currentInputs.Clear();
                    nextCorrectIndex = 0;
                }
            }
            yield return null;

        }

        isReadyToFire = true;
        isCasting = false;
        Debug.Log("<color=green>[Ready to Fire]</color>");
    }


    private void OnFire()
    {
        if (isReadyToFire)
        {
            Debug.Log($"<color=orange>SUCCESS!</color> {selectedSkill.skillName} 발사!");
            ResetCombatState();
        }
    }

    private void ResetCombatState()
    {
        if (skillProcessCoroutine != null)
        {
            StopCoroutine(skillProcessCoroutine);
            skillProcessCoroutine = null;
        }

        isCasting = false;
        isReadyToFire = false;
        currentInputs.Clear();
        //selectedSkill = null; // 필요에 따라 유지하거나 비울 수 있음
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
}
