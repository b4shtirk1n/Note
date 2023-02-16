using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Note.Helpers
{
    public class OpacityAnimation : IAnimation
    {
        private Visibility visibility;

        public double From { get; private set; }

        public double To { get; private set; }

        public UIElement Element { get; private set; }

        public OpacityAnimation(double from, double to, UIElement element)
        {
            From = from;
            To = to;
            Element = element;
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

        private void Animate(double from, double to)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = from;
            opacityAnimation.To = to;
            opacityAnimation.Duration = TimeSpan.FromSeconds(0.25);
            opacityAnimation.Completed += Completed;
            Element.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }

        private void Completed(object sender, EventArgs e)
        {
            Element.Visibility = visibility;
        }
    }
}
