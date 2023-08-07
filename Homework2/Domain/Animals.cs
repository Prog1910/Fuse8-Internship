namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Животное
/// </summary>
public abstract class Animal
{
    /// <summary>
    /// true - если животное является другом человека
    /// </summary>
    public virtual bool IsHumanFriend => false;

    /// <summary>
    /// true - если у животного большой вес
    /// </summary>
    public abstract bool HasBigWeight { get; }

    /// <summary>
    /// Как говорит животное
    /// </summary>
    /// <returns>Возвращает звук, который говорит животное</returns>
    public abstract string WhatDoesSay();
}

/// <summary>
/// Собака
/// </summary>
public abstract class Dog : Animal
{
    public sealed override bool IsHumanFriend => true;

    public sealed override string WhatDoesSay() => "гав";
}

/// <summary>
/// Лиса
/// </summary>
public sealed class Fox : Animal
{
    public override bool IsHumanFriend => false;

    public override bool HasBigWeight => false;

    public override string WhatDoesSay() => "ми-ми-ми";
}

/// <summary>
/// Чихуахуа
/// </summary>
public sealed class Chihuahua : Dog
{
    public override bool HasBigWeight => false;
}

/// <summary>
/// Хаски
/// </summary>
public sealed class Husky : Dog
{
    public new string WhatDoesSay() => "ауф";

    public override bool HasBigWeight => true;
}