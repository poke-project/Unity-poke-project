﻿using UnityEngine;
using System.Collections;
using System;

public class VilleA : AZone
{
    protected override int width
    {
        get
        {
            return (10);
        }
    }
    protected override int height
    {
        get
        {
            return (10);
        }
    }
    protected override Vector2 spawn
    {
        get
        {
            return new Vector2(0, 0);
        }
    }
}
