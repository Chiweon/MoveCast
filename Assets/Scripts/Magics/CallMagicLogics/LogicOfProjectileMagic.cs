using UnityEngine;

[CreateAssetMenu(fileName = "LogicOfProjectileMagic", menuName = "Call/Magics/LogicOfProjectileMagic")]
public class LogicOfProjectileMagic : CallMagics
{
    [Header("Projectile Specifics")]
    public float projectileSpeed = 5.0f;
    public Vector3 spawnOffset; // FirePoint 오브젝트 위치 기준 미세 조정 계산

    public override void Execute(GameObject player, MagicData magicData)
    {
        Transform firePointPos = player.transform.Find("FirePoint") ?? player.transform;

        TargetManager targetMgr = player.GetComponent<TargetManager>();
        Vector3 targetPos = targetMgr.targetPosition;

        if (targetPos == Vector3.zero)
        {
            targetPos = firePointPos.position + player.transform.forward * 10f;
        }
        //Vector3 vFPP = firePointPos.position // FirePoint 오브젝트 위치 기준 spawnOffset을 적용한 위치 계산
        //    + (firePointPos.forward * spawnOffset.z * 10.0f)
        //    + (firePointPos.up * spawnOffset.y)
        //    + (firePointPos.right * spawnOffset.x);

        Vector3 direction = (targetPos - firePointPos.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject magicSpawner = Instantiate(magicData.MagicPrefab, firePointPos.position, rotation);
        ProjectilePhsic proj = magicSpawner.GetComponent<ProjectilePhsic>();

        if (proj != null)
        {
            proj.Init(projectileSpeed, magicData.magicDamage);
        }

    }
}
