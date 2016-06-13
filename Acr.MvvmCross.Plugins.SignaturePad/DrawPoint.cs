using System;


namespace Acr.MvvmCross.Plugins.SignaturePad {

    public class DrawPoint {

        public float X { get; set; }
        public float Y { get; set; }

        public bool IsEmpty { get; private set; }

        public DrawPoint() { }
        public DrawPoint(float x, float y) {
            this.X = x;
            this.Y = y;
        }

        public override string ToString() {
            if (this.IsEmpty)
                return "e";

            return X.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                "x" + Y.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DrawPoint Parse(string parsable) {

            if (parsable == "e") {
                return Empty;
            }

            var parts = parsable.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 2)
                throw new ArgumentOutOfRangeException("Too many parts to the DrawPoint String");

            if (parts.Length < 2)
                throw new ArgumentOutOfRangeException("Too few parts to the DrawPoint String");

            float x;
            if (!float.TryParse(parts[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out x))
                throw new ArgumentOutOfRangeException("Could not parse part[0] as a float");

            float y;
            if (!float.TryParse(parts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out y))
                throw new ArgumentOutOfRangeException("Could not parse part[1] as a float");

            return new DrawPoint(x, y);
        }


        private static DrawPoint empty = new DrawPoint() { IsEmpty = true };
        public static DrawPoint Empty
        {
            get
            {
                return empty;
            }
        }

    }
}
