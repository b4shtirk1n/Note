using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Note.Helpers
{
    public class HeightAnimation : BaseAnimation
    {
        public HeightAnimation(double from, double to, FrameworkElement element, double duration)
            : base(from, to, element, duration) { }

        protected override void Animate(double from, double to)
        {
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.From = from;
            Animation.To = to;
            Animation.Duration = TimeSpan.FromSeconds(Duration);
            Animation.Completed += Completed;
            Element.BeginAnimation(FrameworkElement.HeightProperty, Animation);
        }
    }
}
