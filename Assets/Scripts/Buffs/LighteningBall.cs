public class LighteningBall : Buff
{
    protected override void ApplyEffect()
    {
        foreach (Ball ball in BallManager.Instance.Balls)
        {
            ball.StartLighteningBall();
        }
    }
}