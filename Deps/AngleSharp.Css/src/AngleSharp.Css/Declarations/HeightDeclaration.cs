namespace AngleSharp.Css.Declarations
{
    using AngleSharp.Css.Values;
    using System;
    using static ValueConverters;

    static class HeightDeclaration
    {
        public static String Name = PropertyNames.Height;

        public static IValueConverter Converter = Or(AutoLengthOrPercentConverter, AssignInitial(Length.Auto));

        public static PropertyFlags Flags = PropertyFlags.Unitless | PropertyFlags.Animatable;
    }
}
