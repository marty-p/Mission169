using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IEnemyBehavior {

    bool WantToStart();
    bool UpdateBehavior();
    bool CanBeInterrupted();
    bool InProgress();
    void StartBehavior();
    int GetPriority();

}
