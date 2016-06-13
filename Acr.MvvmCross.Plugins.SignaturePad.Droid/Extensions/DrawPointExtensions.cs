using System.Drawing;

namespace Acr.MvvmCross.Plugins.SignaturePad.Droid.Extensions {
    public static class DrawPointExtensions {
        public static PointF GetPointF(this DrawPoint drawpoint) {
            if (drawpoint.IsEmpty)
                return PointF.Empty;

            return new PointF(drawpoint.X, drawpoint.Y);
        }

        public static DrawPoint GetDrawPoint(this PointF point) {
            if (point.IsEmpty)
                return DrawPoint.Empty;

            return new DrawPoint(point.X, point.Y);
        }
    }
}