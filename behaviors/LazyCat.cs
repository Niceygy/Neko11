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
        private Flags currentActivity = Flags.MOVING;
        private readonly Random rand = new Random();
        private Point? destination = null;

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
            switch (currentActivity)
            {
                case Flags.ASLEEP:
                    // If it's time to wake up, switch to moving
                    if (DateTime.Now >= SleepUntilTime)
                    {
                        currentActivity = Flags.MOVING;
                    }
                    else
                    {
                        Asleep(ref ticksSinceImageChange, form);
                        return;
                    }
                    break;

                case Flags.MOVING:
                    //move to destinaton
                    Awake(form, ref dx, ref dy, ref ticksSinceImageChange);
                    break;

                case Flags.STILL:
                    //reached destination - stay here for a bit
                    //+random chance to fall asleep

                    if (rand.NextDouble() < 0.1)
                    {
                        currentActivity = Flags.ASLEEP;
                        SleepUntilTime = DateTime.Now.AddMinutes(rand.Next(1, 4)); // Sleep 1-3 minutes
                        form.BackgroundImage = form.Images["yawn1.ico"];
                        return;
                    }
                    else
                    {
                        form.BackgroundImage = form.BackgroundImage == form.Images[$"sleep1.ico"]
                        ? form.Images[$"sleep2.ico"]
                        : form.Images[$"sleep1.ico"];
                    }

                    break;
            }
        }

        private void Asleep(ref int ticksSinceImageChange, NekoForm form)
        {
            if (ticksSinceImageChange >= NekoForm.ImageUpdateFrequency * 2.5)
            {
                form.BackgroundImage = form.BackgroundImage == form.Images[$"sleep1.ico"]
                ? form.Images[$"sleep2.ico"]
                : form.Images[$"sleep1.ico"];

                ticksSinceImageChange = 0;
            }
            else
            {
                ticksSinceImageChange++;
            }
            
            if ((SleepUntilTime - DateTime.Now).Seconds <= 0)
            {
                currentActivity = Flags.MOVING;
            }
        }

        private void Awake(NekoForm form, ref int dx, ref int dy, ref int ticksSinceImageChange)
        {
            if (destination == null)
            {
                var screen = Screen.PrimaryScreen!.WorkingArea;
                int x = rand.Next(0, screen.Width);
                int y = rand.Next(0, screen.Height);
                destination = new Point(x, y);
            }
            else
            {
                int speed = 2;
                dx = Math.Abs(destination!.Value.X - form.Left) > 20 ? (destination!.Value.X > form.Left ? speed : -speed) : 0;
                dy = Math.Abs(destination!.Value.Y - form.Top) > 20 ? (destination!.Value.Y > form.Top ? speed : -speed) : 0;

                //update to new location, if it has changed
                if (dx != 0 || dy != 0)
                    form.Location = new Point(form.Location.X + dx, form.Location.Y + dy);

                if (dx < 20 && dy < 20)
                {
                    destination = null;
                    currentActivity = Flags.STILL;

                    form.BackgroundImage = form.Images["awake.ico"];
                }
                else
                {
                    //what direction are we moving in?
                    XDirections X = dx == 0 ? XDirections.NOT : (dx > 0 ? XDirections.RIGHT : XDirections.LEFT);
                    YDirections Y = dy == 0 ? YDirections.NOT : (dy > 0 ? YDirections.DOWN : YDirections.UP);
                    ChooseImage(form, X, Y, Flags.MOVING);
                }
            }
        }
    }
}
