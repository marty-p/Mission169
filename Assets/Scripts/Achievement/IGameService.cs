using System;

public interface IGameService {
    void UpdateAchievement(Achievement ach);
    void RetrieveProgress(Achievement ach);
    void Reset();
} 