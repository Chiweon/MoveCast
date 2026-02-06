using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [Header("Magic Slots")]
    [SerializeField] private MagicData[] magicSlots = new MagicData[4];

    private List<CastKey> currentInputs = new List<CastKey>();

    private MagicData selectedMagic;
    private CursorController cursorController;
    private InputSystem_Actions inputActions;
    
    private bool isCasting = false;
    private bool isReadyToFire = false;
    private Coroutine magicProcessCoroutine;

    private void Awake()
    {
        cursorController = FindFirstObjectByType<CursorController>();
        inputActions = new InputSystem_Actions();

        // 스킬 선택 연결
        inputActions.Player.SelectMagic1.performed += ctx => SelectMagic(0);
        inputActions.Player.SelectMagic2.performed += ctx => SelectMagic(1);
        inputActions.Player.SelectMagic3.performed += ctx => SelectMagic(2);
        inputActions.Player.SelectMagic4.performed += ctx => SelectMagic(3);

        // 캐스트 키 입력 연결
        inputActions.Player.CastQ.performed += ctx => OnCastKeyPressed(CastKey.Q);
        inputActions.Player.CastE.performed += ctx => OnCastKeyPressed(CastKey.E);
        inputActions.Player.CastR.performed += ctx => OnCastKeyPressed(CastKey.R);
        inputActions.Player.CastShift.performed += ctx => OnCastKeyPressed(CastKey.Shift);

        // 발사 입력 연결
        inputActions.Player.Attack.performed += ctx => OnFire();
    }

    private void SelectMagic(int index)
    {
        if (index < magicSlots.Length && magicSlots[index] == null)
        {
            Debug.Log("index value may be more than magicSlots index or magicSlots is empty");
            return;
        }

        if (isCasting || magicProcessCoroutine != null)
        {
            ResetCombatState();
        }

        selectedMagic = magicSlots[index];
        magicProcessCoroutine = StartCoroutine(MagicRoutine());
        Debug.Log($"<Color=cyan>[System]</color> {selectedMagic.magicName} selected!");
    }

    private void OnCastKeyPressed(CastKey key)
    {
        if (isCasting && !isReadyToFire)
        {
            currentInputs.Add(key);
            Debug.Log($"Key Input: {key}");
        }
    }

    private IEnumerator MagicRoutine()
    {
        isCasting = true;
        isReadyToFire = false;
        currentInputs.Clear();

        int nextCorrectIndex = 0;
        float timeoutTimer = 0;
        float limitTime = selectedMagic.castLimitTime;
        Debug.Log($"<color=yellow>[Casting]</color> {selectedMagic.magicName} (제한 시간: {limitTime}초)");

        while (nextCorrectIndex < selectedMagic.castPattern.Count)
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
                if (currentInputs[nextCorrectIndex] == selectedMagic.castPattern[nextCorrectIndex])
                {
                    nextCorrectIndex++;
                    Debug.Log($"Match! ({nextCorrectIndex}/{selectedMagic.castPattern.Count})");
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
            if(selectedMagic != null && selectedMagic.callMagic != null) {
                selectedMagic.callMagic.Execute(gameObject, selectedMagic);
            }
            else { Debug.Log("selectedMagic or callMagic is null"); }

                Debug.Log($"<color=orange>SUCCESS!</color> {selectedMagic.magicName} 발사!");
            ResetCombatState();
            
        }
    }

    private void ResetCombatState()
    {
        if (magicProcessCoroutine != null)
        {
            StopCoroutine(magicProcessCoroutine);
            magicProcessCoroutine = null;
        }

        isCasting = false;
        isReadyToFire = false;
        currentInputs.Clear();
        //selectedMagic = null; // 필요에 따라 유지하거나 비울 수 있음
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
}
