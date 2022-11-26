using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    public static Vector3 CorrectHeight(Vector3 wrongVector) => new Vector3(wrongVector.x, 0, wrongVector.z);
}
