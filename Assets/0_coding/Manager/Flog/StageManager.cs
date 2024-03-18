using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonObjectBase<StageManager>
{
}

/// <summary>
/// カエル
/// </summary>
public class Frog
{
    private EvolutionaryType _type;
    /// <summary>
    /// カエルの状態
    /// </summary>
    public EvolutionaryType Type => _type;

    public Frog(EvolutionaryType type)
    {
        _type = type;
    }

    /// <summary>
    /// 次の状態へ
    /// </summary>
    public void Evolve()
    {
        _type = (EvolutionaryType)(Mathf.Clamp((int)Type + 1, 0, 3));
    }
}

/// <summary>
/// カエルの進化状態
/// </summary>
public enum EvolutionaryType
{
    None,
    Egg,
    Tadpole,
    Frog
}