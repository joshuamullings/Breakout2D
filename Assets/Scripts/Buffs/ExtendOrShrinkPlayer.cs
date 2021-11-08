public class ExtendOrShrinkPlayer : Buff
{
    public float NewWidth = 1.5f;

    protected override void ApplyEffect()
    {
        if (Player.Instance != null && !Player.Instance.PlayerIsTransforming)
        {
            Player.Instance.StartWidthAnimation(NewWidth);
        }
    }
}