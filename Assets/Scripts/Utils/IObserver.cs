﻿
public enum SlugEvents {
        Fall,
        Jump,
        JumpLowSpeed,
        JumpHighSpeed,
        HitGround,
        MovingRight,
        MovingLeft,
        StartMoving,
        StopMoving,
        Turn,
        Sit,
        Stand,
        LookUp,
        Shoot,
        Attack,
        Grenade
};

public interface IObserver {

    void Observe(SlugEvents ev);

}
