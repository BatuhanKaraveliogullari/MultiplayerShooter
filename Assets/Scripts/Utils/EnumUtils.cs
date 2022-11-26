using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumUtils
{
    public static bool IsEqualColor(BulletColor currentEnum, BulletColor otherEnum) => (currentEnum & otherEnum) == otherEnum;
}
