namespace Neko11V2.behaviors.reusable
{
    public interface IBehavior
    {
        public void UpdateImage(NekoForm form, XDirections xDirection, YDirections yDirection, Flags flag);
        public void Update(NekoForm form, ref int dx, ref int dy, ref int ticksSinceImageChange);


    }
}