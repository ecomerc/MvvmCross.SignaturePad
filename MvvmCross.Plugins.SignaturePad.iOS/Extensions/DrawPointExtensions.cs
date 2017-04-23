using CoreGraphics;
using System.Drawing;

namespace EcoMerc.MvvmCross.Plugins.SignaturePad.Touch.Extensions {
    public static class DrawPointExtensions {
        public static CGPoint GetPointF(this DrawPoint drawpoint) {
            if (drawpoint.IsEmpty)
                return CGPoint.Empty;

            return new CGPoint(drawpoint.X, drawpoint.Y);
        }

        public static DrawPoint GetDrawPoint(this CGPoint point) {
            if (point.IsEmpty)
                return DrawPoint.Empty;

            return new DrawPoint((float)point.X, (float)point.Y);
        }
    }
}