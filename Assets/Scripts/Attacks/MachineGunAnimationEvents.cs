using UnityEngine;
using System.Collections;
using Utils;

public class MachineGunAnimationEvents : MonoBehaviour {

    private RetVoidTakeVoid horizontalCB;
    private RetVoidTakeVoid upCB;
    private RetVoidTakeInt diagUpCB; 
    private RetVoidTakeInt diagDownCB;
    private RetVoidTakeVoid downCB;
    private RetVoidTakeVoid sittingCB;

    public void SetMgHorizontalCB(RetVoidTakeVoid cb) {
        horizontalCB = cb;
    }

    public void SetMgUpCB(RetVoidTakeVoid cb) {
        upCB = cb;
    }

    public void SetMgDownCB(RetVoidTakeVoid cb) {
        downCB = cb;
    }

    public void SetMgDiagUp(RetVoidTakeInt cb) {
        diagUpCB = cb;
    }

    public void SetMgDiagDown(RetVoidTakeInt cb) {
        diagDownCB = cb;
    }

    public void SetMgSittingCB(RetVoidTakeVoid cb) {
        sittingCB = cb;
    }

    public void MgShootHorizontal() {
        horizontalCB();
    }

    public void MgShootDiagUp(int animStep) {
        diagUpCB(animStep);
    }

    public void MgShootDiagDown(int animStep) {
        diagDownCB(animStep);
    }

   public void MgShootUp() {
        upCB();
    }

    public void MgShootSitting() {
        sittingCB();
    }

    public void MgShootDown() {
        downCB();
    }

}
