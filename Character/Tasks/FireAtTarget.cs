using System;
using System.Collections.Generic;
using UnityEngine;

public class FireAtTarget : TreeNode
{
    public FireAtTarget(BehaviourTree tree) : base(tree)
    {
        TaskName = "Fire At Target";
    }

    public FireAtTarget(BehaviourTree tree, List<TreeNode> children) : base(tree, children)
    {
        TaskName = "Fire At Target";
    }

    public override ETreeNodeState Run()
    {
        if(_Tree.Debug)
            UpdateDebugger();
            
        Blackboard bb = _Tree.Owner.GetBlackboard();
        if(bb != null)
        {
            GameObject target = bb.GetValueAsGameObject("Target");
            if(target)
            {
                _Tree.Owner.transform.LookAt(target.transform);
                _Tree.Owner.UseWeapon();

                ShootAtTarget(bb);

                return ETreeNodeState.SUCCESS;
            }
        }

        return ETreeNodeState.FAILURE;
    }

    private void ShootAtTarget(Blackboard bb)
    {
        BaseCharacterController owner = _Tree.Owner;
        BaseCharacterController target = bb.GetValueAsGameObject("Target").GetComponent<BaseCharacterController>();
        if(!target)
            return;

        if(owner == null)
            return;

        Vector3 targetPos = target.transform.position;
        Vector3 charPos = owner.transform.position;
        float distance = Vector3.Distance(targetPos, charPos);

        CharacterAttributes attr = _Tree.Owner.Attributes;
        if(attr != null)
        {
            float currentDex = attr.CurrentDexterity;
            GunController weapon = null;
            if(owner.Inventory.EquippedWeapon1.IsInHand)
            {
                weapon = owner.Inventory.EquippedWeapon1 as GunController;
            } else if(owner.Inventory.EquippedWeapon2.IsInHand)
            {
                weapon = owner.Inventory.EquippedWeapon2 as GunController;
            }

             if(weapon == null)
                return;

            float sightMod = 1f;

            if(weapon.HasSightAttachment)
                sightMod = weapon.SightAttachment.AttachmentModifier;

            float distanceFactor = weapon.MaxAttackDistance / distance;
            float initialChanceOfHit = distanceFactor * currentDex * sightMod;

            float finalChanceOfHit = (initialChanceOfHit / (weapon.MaxAttackDistance * 1.0f)) * 100;

            float randNum = UnityEngine.Random.Range(-1.0f, 1.1f);
            if(randNum < finalChanceOfHit)
            {
                BodyPart hitPart = target.GetRandomPart();
                float damagePoints = weapon.CalculateDamage(target, hitPart);
                target.TakeDamage(damagePoints);
            }

        }
    }
}