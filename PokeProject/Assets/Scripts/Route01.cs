using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route01 : AZone
{
        protected override int width
    {
        get
        {
            return (16);
        }
    }

    protected override int height
    {
        get
        {
            return (18);
        }
    }

    protected override Vector2 spawn
    {
        get
        {
            return new Vector2(6, 0.5f);
        }
    }
}
