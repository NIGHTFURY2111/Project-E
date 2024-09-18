using System;



[Flags]
public enum exitStates
{
    
    idle = 1 << 0,
    move = 1 << 1,
    jump = 1 << 2,
    fall = 1 << 3,
    dash = 1 << 4,
    wallslide = 1 << 5,
   
}
