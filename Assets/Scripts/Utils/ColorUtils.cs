using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtils
{
   private static Color[] clientColors = { Color.white, Color.blue, Color.green, Color.red, Color.yellow, Color.gray, Color.black, Color.cyan, Color.magenta, };

   public static Color GetColorForClient(ulong clientID) => clientColors[clientID];

   public static Color GetColorWithEnum(BulletColor bulletColor)
   {
      switch (bulletColor)
      {
         case BulletColor.Red:
            return Color.red;
         case BulletColor.Blue:
            return Color.blue;
         case BulletColor.Green:
            return Color.green;
      }
      throw new Exception(" Three is no color ");
   }

}
