using System;
using System.Windows;

namespace Note.Helpers
{
    public abstract class BaseAnimation
    {
        private Visibility visibility;

        public double From { get; protected set; }

        public double To { get; protected set; }

        public FrameworkElement Element { get; protected set; }

        public double Duration { get; protected set; }

        public BaseAnimation(double from, double to, FrameworkElement element, double duration)
        {
            From = from;
            To = to;
            Element = element;
            Duration = duration;
        }

        public void StartIn()
        {
            visibility = Visibility.Visible;
            Element.Visibility = visibility;
            Animate(From, To);
        }

        public void StartOut()
        {
            visibility = Visibility.Hidden;
            Animate(To, From);
        }

        protected abstract void Animate(double from, double to);

        protected virtual void Completed(object sender, EventArgs e)
        {
            Element.Visibility = visibility;
        }
    }
}
