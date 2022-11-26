using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtils
{
   private static Color[] clientColors = { Color.white, Color.blue, Color.green, Color.red, Color.yellow, Color.gray, Color.black, Color.cyan, Color.magenta, };

   public static Color GetColorForClient(int clientID) => clientColors[clientID];

}
