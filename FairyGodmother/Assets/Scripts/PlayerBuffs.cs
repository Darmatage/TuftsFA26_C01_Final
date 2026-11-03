using UnityEngine;
using System.Collections.Generic;

public class PlayerBuffs : MonoBehaviour
{
    class ActiveBuff
    {
        public BuffType type;
        public float multiplier;
        public float remaining;
    }

    readonly List<ActiveBuff> active = new List<ActiveBuff>();
 
    public float SpeedMultiplier { get; private set; } = 1f;
    public float JumpMultiplier { get; private set; } = 1f;
 
    // duration <= 0 means permanent
    public void Apply(BuffType type, float multiplier, float duration)
    {
        active.Add(new ActiveBuff
        {
            type = type,
            multiplier = multiplier,
            remaining = duration > 0f ? duration : float.PositiveInfinity
        });
        Recalculate();
        Debug.Log($"Buff applied: {type} x{multiplier} for {(duration > 0f ? duration + "s" : "ever")}");
    }


    // Update is called once per frame
    void Update()
    {
        if (active.Count == 0) return;
 
        bool changed = false;
        for (int i = active.Count - 1; i >= 0; i--)
        {
            active[i].remaining -= Time.deltaTime;
            if (active[i].remaining <= 0f)
            {
                active.RemoveAt(i);
                changed = true;
            }
        }
        if (changed) Recalculate();
    }

    void Recalculate()
    {
        float speed = 1f;
        float jump = 1f;
        foreach (var b in active)
        {
            if (b.type == BuffType.Speed) speed = Mathf.Max(speed, b.multiplier);
            else if (b.type == BuffType.Jump) jump = Mathf.Max(jump, b.multiplier);
        }
        SpeedMultiplier = speed;
        JumpMultiplier = jump;
    }


}
