using UnityEngine;

public abstract class CallMagics : ScriptableObject
{
    public abstract void Execute(GameObject player, MagicData magicData);
}
