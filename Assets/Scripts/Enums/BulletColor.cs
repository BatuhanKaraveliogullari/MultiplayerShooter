using System;

[Flags]
public enum BulletColor : byte
{
    None  = 0,      //00000000
    Red   = 1,      //00000001
    Blue  = 1 << 1, //00000010
    Green = 1 << 2  //00000100
}