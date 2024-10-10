namespace SoT.Interfaces
{
    public interface iCooldownable
    {
        float cooldownTimer { get; set; }

        bool CooldownDone(bool setTimer, float cooldownTime);
    }
}
