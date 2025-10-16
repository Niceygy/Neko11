using Neko11V2.behaviors.reusable;
using System.Drawing;
using System.Windows.Forms;

namespace Neko11V2.behaviors
{
    class FollowMouse : IBehavior
    {
        public void ChooseImage(NekoForm form, XDirections xDirection, YDirections yDirection, Flags flag)
        {
            var images = form.Images;
            string imagename =
                (yDirection == YDirections.NOT ? "" : (yDirection == YDirections.UP ? "up" : "down")) +
                (xDirection == XDirections.NOT ? "" : (xDirection == XDirections.LEFT ? "left" : "right"));

            if (imagename == "" || imagename == string.Empty)
            {
                imagename = "sleep";
            }

            form.BackgroundImage = form.BackgroundImage == images[$"{imagename}1.ico"]
                ? images[$"{imagename}2.ico"]
                : images[$"{imagename}1.ico"];
        }

        public void MoveAndChooseImage(NekoForm form, ref int dx, ref int dy, ref int ticksSinceImageChange)
        {
            Point mousePos = Control.MousePosition;
            bool isMoving = false;

            // Calculate direction towards mouse
            int speed = 2;
            dx = Math.Abs(mousePos.X - form.Left) > 20 ? (mousePos.X > form.Left ? speed : -speed) : 0;
            dy = Math.Abs(mousePos.Y - form.Top) > 20 ? (mousePos.Y > form.Top ? speed : -speed) : 0;

            //update to new location, if it has changed
            if (dx != 0 || dy != 0)
                form.Location = new Point(form.Location.X + dx, form.Location.Y + dy);

            //what direction are we moving in?
            XDirections X = dx == 0 ? XDirections.NOT : (dx > 0 ? XDirections.RIGHT : XDirections.LEFT);
            YDirections Y = dy == 0 ? YDirections.NOT : (dy > 0 ? YDirections.DOWN : YDirections.UP);

            if (ticksSinceImageChange >= 10)
            {
                Flags F = isMoving ? Flags.STILL : Flags.MOVING;
                ChooseImage(form, X, Y, F);
                ticksSinceImageChange = 0;
            }
            else
            {
                ticksSinceImageChange++;
            }
        }

        private static bool WithinXof(int a, int b, int range)
        {
            // return ((a + range) < b || (a - range) > b);
            int min = a - range;
            int max = a + range;
            return (min <= b && b <= max);
        }
    }
}