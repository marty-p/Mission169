
public enum SlugEvents {
        Fall,
        JumpLowSpeed,
        JumpHighSpeed,
        HitGround,
        StartMoving,
        IsMoving,
        StopMoving,
        Turn,
        Sit,
        Stand,
        LookUp,
        Shoot,
        Attack};

public interface IObserver {

    void Observe(SlugEvents ev);

}
