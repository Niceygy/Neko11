using System.Windows.Forms;
using Neko11V2;
using Neko11V2.behaviors.reusable;

namespace Neko11V2.behaviors
{
    class RandomMovement : IBehavior
    {
        public void ChooseImage(NekoForm form, XDirections xDirection, YDirections yDirection, Flags flag)
        {
            var images = form.Images;
            string imagename =
                (yDirection == YDirections.UP ? "up" : "down") +
                (xDirection == XDirections.LEFT ? "left" : "right");

            form.BackgroundImage = form.BackgroundImage == images[$"{imagename}1.ico"]
                ? images[$"{imagename}2.ico"]
                : images[$"{imagename}1.ico"];
        }

        public void MoveAndChooseImage(NekoForm form, ref int dx, ref int dy, ref int ticksSinceImageChange)
        {
            var screen = Screen.PrimaryScreen!.WorkingArea;
            form.Left += dx;
            form.Top += dy;

            if (ticksSinceImageChange >= 10)
            {
                XDirections X = (dx <= 0 ? XDirections.LEFT : XDirections.RIGHT);
                YDirections Y = (dy <= 0 ? YDirections.UP : YDirections.DOWN);
                ChooseImage(form, X, Y, Flags.MOVING);
                ticksSinceImageChange = 0;
            }
            else
            {
                ticksSinceImageChange++;
            }

            // Bounce off edges
            if (form.Left < screen.Left || form.Right > screen.Right)
            {
                dx = -dx;
            }
            if (form.Top < screen.Top || form.Bottom > screen.Bottom)
            {
                dy = -dy;
            }
        }
    }
}