using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class AType
{
    public float dmgsModifier(AType other)
    {
        if (noEffect(other))
        {
            return (0);
        }
        else if (isNotVeryEffective(other))
        {
            return (0.5f);
        }
        else if (isSuperEffective(other))
        {
            return (2);
        }
        else
        {
            return (1);
        }
    }
    abstract protected bool noEffect(AType other);
    abstract protected bool isNotVeryEffective(AType other);
    abstract protected bool isSuperEffective(AType other);
}

public class Normal : AType
{
    override protected bool noEffect(AType other)
    {
        if (other.GetType() == typeof(Ghost))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Rock)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        return (false);
    }
}

public class Fire : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Fire)
            || other.GetType() == typeof(Water)
            || other.GetType() == typeof(Rock)
            || other.GetType() == typeof(Dragon))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Grass)
            || other.GetType() == typeof(Ice)
            || other.GetType() == typeof(Bug)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

}

public class Water : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Water)
            || other.GetType() == typeof(Grass)
            || other.GetType() == typeof(Dragon))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Fire)
            || other.GetType() == typeof(Ground)
            || other.GetType() == typeof(Rock))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}
public class Elec : AType
{
    override protected bool noEffect(AType other)
    {
        if (other.GetType() == typeof(Ground))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Elec)
            || other.GetType() == typeof(Grass)
            || other.GetType() == typeof(Dragon))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Water)
            || other.GetType() == typeof(Fly))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Grass : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Fire)
            || other.GetType() == typeof(Grass)
            || other.GetType() == typeof(Poison)
            || other.GetType() == typeof(Fly)
            || other.GetType() == typeof(Bug)
            || other.GetType() == typeof(Dragon)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Water)
            || other.GetType() == typeof(Ground)
            || other.GetType() == typeof(Rock))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Ice : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Fire)
            || other.GetType() == typeof(Water)
            || other.GetType() == typeof(Ice)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Grass)
            || other.GetType() == typeof(Ground)
            || other.GetType() == typeof(Fly)
            || other.GetType() == typeof(Dragon))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Fighting : AType
{
    override protected bool noEffect(AType other)
    {
        if (other.GetType() == typeof(Ghost))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Poison)
            || other.GetType() == typeof(Fly)
            || other.GetType() == typeof(Psy)
            || other.GetType() == typeof(Bug))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Normal)
            || other.GetType() == typeof(Ice)
            || other.GetType() == typeof(Rock)
            || other.GetType() == typeof(Darkness)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}


    public class Poison : AType
{
    override protected bool noEffect(AType other)
    {
        if (other.GetType() == typeof(Steel))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Poison)
            || other.GetType() == typeof(Ground)
            || other.GetType() == typeof(Rock)
            || other.GetType() == typeof(Ghost))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Grass))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Ground : AType
{
    override protected bool noEffect(AType other)
    {
        if (other.GetType() == typeof(Fly))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Grass)
            || other.GetType() == typeof(Bug))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Fire)
            || other.GetType() == typeof(Elec)
            || other.GetType() == typeof(Poison)
            || other.GetType() == typeof(Rock)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Fly : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Elec)
            || other.GetType() == typeof(Rock)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Grass)
            || other.GetType() == typeof(Fighting)
            || other.GetType() == typeof(Bug))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Psy : AType
{
    override protected bool noEffect(AType other)
    {
        if (other.GetType() == typeof(Darkness))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Psy)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Fighting)
            || other.GetType() == typeof(Poison))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Bug : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Fire)
            || other.GetType() == typeof(Fighting)
            || other.GetType() == typeof(Poison)
            || other.GetType() == typeof(Fly)
            || other.GetType() == typeof(Ghost)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Grass)
            || other.GetType() == typeof(Psy)
            || other.GetType() == typeof(Darkness))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Rock : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Fighting)
            || other.GetType() == typeof(Ground)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Fire)
            || other.GetType() == typeof(Ice)
            || other.GetType() == typeof(Fly)
            || other.GetType() == typeof(Bug))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Ghost : AType
{
    override protected bool noEffect(AType other)
    {
        if (other.GetType() == typeof(Normal))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Darkness)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Psy)
            || other.GetType() == typeof(Ghost))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Dragon : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Dragon))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Darkness : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Fighting)
            || other.GetType() == typeof(Darkness)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Psy)
            || other.GetType() == typeof(Ghost))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}

public class Steel : AType
{
    override protected bool noEffect(AType other)
    {
        return (false);
    }

    override protected bool isNotVeryEffective(AType other)
    {
        if (other.GetType() == typeof(Fire)
            || other.GetType() == typeof(Water)
            || other.GetType() == typeof(Elec)
            || other.GetType() == typeof(Steel))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }

    override protected bool isSuperEffective(AType other)
    {
        if (other.GetType() == typeof(Ice)
            || other.GetType() == typeof(Rock))
        {
            return (true);
        }
       else
        {
            return (false);
        }
    }
}