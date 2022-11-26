using System;

[Flags]
public enum BulletSize : byte
{
    None        = 0,      //00000000
    Small       = 1,      //00000001
    Standard    = 1 << 1, //00000010
    Large       = 1 << 2  //00000100
}