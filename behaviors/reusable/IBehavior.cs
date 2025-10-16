namespace Neko11V2.behaviors.reusable
{
    public interface IBehavior
    {
        public void ChooseImage(NekoForm form, XDirections xDirection, YDirections yDirection, Flags flag);
        public void MoveAndChooseImage(NekoForm form, ref int dx, ref int dy, ref int ticksSinceImageChange);


    }
}