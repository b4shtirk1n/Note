using System.Windows;

namespace Note.Helpers
{
    interface IAnimation
    {
        double From { get; }

        double To { get; }

        UIElement Element { get; }

        void StartIn();

        void StartOut();
    }
}
