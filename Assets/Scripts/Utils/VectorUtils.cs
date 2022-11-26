using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    public static Vector3 CorrectHeight(Vector3 wrongVector) => new Vector3(wrongVector.x, 0, wrongVector.z);

    public static Vector3 GetScaleWithEnum(BulletSize bulletSize)
    {
        switch (bulletSize)
        {
            case BulletSize.Small:
                return Vector3.one * 0.3f;
            case BulletSize.Standard:
                return Vector3.one * 0.6f;
            case BulletSize.Large:
                return Vector3.one * 0.9f;
        }
        throw new Exception(" Three is no size ");
    }
}
