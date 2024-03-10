using System.Collections.Generic;

namespace FiveStar
{
    public static class Predefined
    {
        public static class Australia
        {
            public static IEnumerable<SphericalPoint> AllCapitals = new[]
            {
                new SphericalPoint(149.1204446, -35.2813043, 6371, "Canberra"),
                new SphericalPoint(150.3715513, -33.8469761, 6371, "Sydney"),
                new SphericalPoint(130.7932238, -12.4258646, 6371, "Darwin"),
                new SphericalPoint(152.4327621, -27.3810235, 6371, "Brisbane"),
                new SphericalPoint(138.3309789, -35.0004451, 6371, "Adelaide"),
                new SphericalPoint(147.3110468, -42.8823389, 6371, "Hobart"),
                new SphericalPoint(145.0531353, -37.9725665, 6371, "Melbourne"),
                new SphericalPoint(115.9615356, -32.0400639, 6371, "Perth"),
            };

            public static IEnumerable<SphericalPoint> NewSouthWales = new[]
            {
                new SphericalPoint(150.3715513, -33.8469761, 6371, "Sydney"),
                new SphericalPoint(153.0891777, -27.9539379, 6371, "Tweed Heads"),
                new SphericalPoint(151.5496170, -32.9765848, 6371, "Newcastle"),
                new SphericalPoint(149.1204446, -35.2813043, 6371, "Canberra"),
                new SphericalPoint(150.8755489, -34.4282514, 6371, "Wollongong"),
            };

            public static IEnumerable<SphericalPoint> NorthernTerritory = new[]
            {
                new SphericalPoint(130.7932238, -12.4258646, 6371, "Darwin"),
                new SphericalPoint(133.8713752, -23.6993532, 6371, "Alice Springs"),
            };

            public static IEnumerable<SphericalPoint> Queensland = new[]
            {
                new SphericalPoint(152.4327621, -27.3810235, 6371, "Brisbane"),
                new SphericalPoint(153.0891777, -27.9539379, 6371, "Gold Coast"),
                new SphericalPoint(152.9340729, -26.6064139, 6371, "Sunshine Coast"),
                new SphericalPoint(146.8003286, -19.2596723, 6371, "Townsville"),
                new SphericalPoint(145.5768599, -16.8804789, 6371, "Cairns"),
            };

            public static IEnumerable<SphericalPoint> SouthAustralia = new[]
            {
                new SphericalPoint(138.3309789, -35.0004451, 6371, "Adelaide"),
                new SphericalPoint(140.7106046, -37.8263015, 6371, "Mount Gambier"),
                new SphericalPoint(138.5793668, -35.5470711, 6371, "Victor Harbour"),
                new SphericalPoint(137.5616348, -33.0382221, 6371, "Whyalla"),
                new SphericalPoint(139.1973347, -35.1293887, 6371, "Murray Bridge"),
            };

            public static IEnumerable<SphericalPoint> Tasmania = new[]
            {
                new SphericalPoint(147.3110468, -42.8823389, 6371, "Hobart"),
                new SphericalPoint(147.1263772, -41.4333817, 6371, "Launceston"),
                new SphericalPoint(146.3096623, -41.1754290, 6371, "Devonport"),
                new SphericalPoint(145.8919412, -41.0541043, 6371, "Burnie-Wynyard"),
                new SphericalPoint(146.1152398, -41.1773260, 6371, "Ulverstone"),
            };

            public static IEnumerable<SphericalPoint> Victoria = new[]
            {
                new SphericalPoint(145.0531353, -37.9725665, 6371, "Melbourne"),
                new SphericalPoint(144.3453714, -38.1481387, 6371, "Geelong"),
                new SphericalPoint(143.7827012, -37.5674314, 6371, "Ballarat"),
                new SphericalPoint(144.2713972, -36.7561795, 6371, "Bendigo"),
                new SphericalPoint(146.8474072, -36.1257495, 6371, "Wodonga"),
            };

            public static IEnumerable<SphericalPoint> WesternAustralia = new[]
            {
                new SphericalPoint(115.9615356, -32.0400639, 6371, "Perth"),
                new SphericalPoint(115.6246980, -33.3188751, 6371, "Bunbury"),
                new SphericalPoint(115.3407965, -33.6559678, 6371, "Busselton"),
                new SphericalPoint(114.5936913, -28.7757056, 6371, "Geraldton"),
                new SphericalPoint(117.8666723, -35.0280658, 6371, "Albany"),
            };
        }

        public static class Brazil
        {
            public static IEnumerable<SphericalPoint> RioGrandeDoNorte = new[]
            {
                new SphericalPoint(-35.2222442, -5.7999190, 6371, "Natal"),
                new SphericalPoint(-37.3416400, -5.1952368, 6371, "Mossoró"),
                new SphericalPoint(-35.2111008, -5.9226086, 6371, "Parnamirim"),
                new SphericalPoint(-35.3595264, -5.7748574, 6371, "São Gonçalo do Amarante"),
            };
        }
    }
}
