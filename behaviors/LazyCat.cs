using Neko11V2.behaviors.reusable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko11V2.behaviors
{
    class LazyCat : IBehavior
    {
        private DateTime SleepUntilTime = DateTime.Now;
        private Flags flags = Flags.STILL;
        private readonly Random rand = new Random();

        public void ChooseImage(NekoForm form, XDirections xDirection, YDirections yDirection, Flags flag)
        {
            var images = form.Images;
            string imagename =
                (yDirection == YDirections.UP ? "up" : "down") +
                (xDirection == XDirections.LEFT ? "left" : "right");

            if (flag == Flags.ASLEEP)
            {
                imagename = "yawn";
            }

            form.BackgroundImage = form.BackgroundImage == images[$"{imagename}1.ico"]
                ? images[$"{imagename}2.ico"]
                : images[$"{imagename}1.ico"];
        }

        public void MoveAndChooseImage(NekoForm form, ref int dx, ref int dy, ref int ticksSinceImageChange)
        {
            if (flags == Flags.STILL)
            {
                var now = DateTime.Now;
                var secondsSinceLastSleep = (SleepUntilTime - now).Seconds;
                int FallsAsleepChance = rand.Next(0, secondsSinceLastSleep);

                if (FallsAsleepChance == 1)
                {
                    flags = Flags.ASLEEP;

                }
            }
        }
    }
}
