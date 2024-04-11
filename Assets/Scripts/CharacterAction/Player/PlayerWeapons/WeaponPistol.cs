using UnityEngine;

public class WeaponPistol : PlayerWeaponBase
{
    /* ==================== Fields ==================== */

    private GameObject _prefab = null;



    /* ==================== Public Methods ==================== */

    public WeaponPistol()
    {
        _prefab = Resources.Load<GameObject>("PlayerWeapons/PistolEffect");
        StageManagerBase.ObjectPool.PoolPreparing(_prefab);
        AttackTime = Constants.WEAPON_PISTOL_TIME;
    }


    public override void Attack(Vector2 pos, sbyte direction, float range, ushort damage)
    {
        if (IsPressed)
        {
            return;
        }

        IsPressed = true;

        // Range
        range = Constants.WEAPON_PISTOL_RANGE + Constants.WEAPON_PISTOL_RANGE * range;

        // Hit scan
        RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(direction, 0.0f), range, Constants.LAYER_B_ENEMY + Constants.LAYER_B_TERRAIN);

        // Attack effect
        LineRendererAttackEffect eft = StageManagerBase.ObjectPool.GetObject(_prefab).GetComponent<LineRendererAttackEffect>();
        if (hit.collider == null)
        {
            eft.Begin(
                pos,
                new Vector2(
                    pos.x + range * direction,
                    pos.y
                )
            );
        }
        else
        {
            eft.Begin(
                pos,
                new Vector2(
                    hit.transform.position.x,
                    pos.y
                )
            );

            // Deal Damage
            GameObject target = hit.collider.gameObject;
            if (target.layer == Constants.LAYER_D_ENEMY)
            {
                target.GetComponent<IHit>().Hit((ushort)(damage + Constants.WEAPON_PISTOL_DAMAGE), direction);
            }
        }
    }
}
