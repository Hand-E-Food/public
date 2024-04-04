using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

partial class MapGenerator
{
    /// <summary>
    /// Returns the default implementation of the MapGenerator class using a specified random number seed.
    /// </summary>
    /// <param name="junctions">The number of junctions to create.</param>
    /// <param name="seed">The random number seed to use.</param>
    /// <returns>An initialised MapGenerator class.</returns>
    public static MapGenerator Default(int junctions, int seed)
    {
        MapGenerator mapGenerator = new()
        {
            Name = "Default Map Genreator",
            Random = new(seed),
        };

        // The island comproses of triangles of a (mostly) fixed area.  Roads are between 5 and 15km long.
        LayoutGenerator layoutGenerator = new()
        {
            Name = "Generating island dimensions...",
            MaximumJunctions = junctions,
            MaximumRoadLength = 150,
            MinimumRoadLength = 50,
            ZoneGenerator = new FixedAreaTriangleGenerator()
        };
        mapGenerator.AddTask(layoutGenerator);

        // The map extremities are placed equidistantly from the centre axes.
        MapCentralizer mapCentralizer = new()
        {
            Name = "Centralizing island..."
        };
        mapGenerator.AddTask(mapCentralizer);

        // The ocean surrounds all other land and is filled with salt water.
        OutsideIdentifier outsideIdentifier = new()
        {
            Name = "Filling ocean...",
            Biome = Biome.Ocean
        };
        mapGenerator.AddTask(outsideIdentifier);

        // Biomes are seeded randomly on the map.  Larger maps have more seeds.
        BiomeSeeder biomeSeeder = new()
        {
            Name = "Seeding biomes..."
        };
        int biomeSets = (int)Math.Round(layoutGenerator.MaximumJunctions / 50.0);
        for (int i = 0; i < biomeSets; i++)
            biomeSeeder.Biomes.AddRange([Biome.Lake, Biome.Desert, Biome.Forest, Biome.Pasture, Biome.Mountain]);
        mapGenerator.AddTask(biomeSeeder);

        // Biomes spread out randomly, but statistically evenly.  Smaller biomes are given a better chance
        // to grow.
        BiomeSpreader biomeSpreader = new()
        {
            Name = "Spreading biomes...",
            Biomes = { Biome.Desert, Biome.Forest, Biome.Pasture, Biome.Mountain },
        };
        mapGenerator.AddTask(biomeSpreader);

        // Archipelagos stuff up the biome spreader algorithm.
        ZoneFinalizer zoneFinalizer = new()
        {
            Name = "Spreading biomes across archipelagos..."
        };
        zoneFinalizer.AllowedBiomes.AddRange(biomeSpreader.Biomes);
        mapGenerator.AddTask(zoneFinalizer);

        // Large mountains are tall and have snow falls.
        BiomePromoter mountainSnowifier = new()
        {
            Name = "Sprinkling snow...",
            Biome = Biome.Snow,
            Condition = zone =>
                zone.Junctions                              // For each of the zone's junctions
                .SelectMany(j => j.Zones)                 // find all zones that are adjacent to the junction,
                .All(z2 => new[] { Biome.Mountain, Biome.Snow }
                    .Contains(z2.Biome))                   // and confirm they all have mountainous or snowy biomes.
        };
        mapGenerator.AddTask(mountainSnowifier);

        // Lakes drain into the ocean.
        RiverLayer lakeRiverLayer = new()
        {
            Name = "Melting snow and draining lakes...",
            AllowedRoadQualities = { RoadQuality.Undefined, RoadQuality.None, RoadQuality.River },
            LaidRoadQuality = RoadQuality.River,
            SourceBiomes = { Biome.Lake, Biome.Snow },
            TargetBiomes = { Biome.Ocean, Biome.Lake }
        };
        mapGenerator.AddTask(lakeRiverLayer);

        // Swamps occur near the ocean in forests with poor drainage.
        BiomePromoter forestSwampifier = new()
        {
            Name = "Swamping forests...",
            Biome = Biome.Swamp,
            Condition = zone =>
                zone.Biome == Biome.Forest                  // Where the zone has a forest biome;
            &&                                              //   and
                zone.Roads                                  // the zone's surrounding roads
                .All(r => r.Quality != RoadQuality.River) // have no rivers;
            &&                                              //   and
                zone.Junctions                              // for each of the zone's junctions
                .SelectMany(j => j.Zones)                 // find all zones adjacent to the junction,
                .Any(z2 => z2.Biome == Biome.Ocean)      // and confirm at least one is oceanic.
        };
        mapGenerator.AddTask(forestSwampifier);

        // Roads shorter than 5km are not used.
        ShortRoadRemover shortRoadRemover = new()
        {
            Name = "Removing poor excuses for roads...",
            MinimumRoadLength = 50
        };
        mapGenerator.AddTask(shortRoadRemover);

        // Roads are removed where there is an alternate route of similar distance.
        RedundantRoadRemover redundantRoadRemover = new()
        {
            Name = "Removing redundant roads...",
            AlternateRouteRatio = 1.4
        };
        mapGenerator.AddTask(redundantRoadRemover);

        // Settlements are randomly added to the map.  They spread out into surrounding zones.  All
        // settlements are at least 30km apart (as the crow flies.)
        SettlementBuilder settlementBuilder = new()
        {
            Name = "Settling settlements...",
            MaximumCities = (int)(layoutGenerator.MaximumJunctions / 10),
            MinimumDistance = (layoutGenerator.MaximumRoadLength + layoutGenerator.MinimumRoadLength) * 3 / 2,
            SettleableBiomes = {
                { SettlementSize.Homestead, [ Biome.Pasture ] },
                { SettlementSize.Town, [ Biome.Pasture, Biome.Desert, Biome.Mountain ] },
                { SettlementSize.City, [ Biome.Pasture, Biome.Desert, Biome.Forest, Biome.Mountain, Biome.Snow ] },
            },
            SettlementSizeWeights = {
                { SettlementSize.Homestead, 3 },
                { SettlementSize.Town, 6 },
                { SettlementSize.City, 1 },
            },
        };
        mapGenerator.AddTask(settlementBuilder);

        // Roads connect any pair of towns within 60km of each other.  Road networks form routes between more
        // distant towns.
        DistantCityRoadLayer distantCityRoadLayer = new()
        {
            Name = "Laying roads between neighbours...",
            AllowedRoadQualities = { RoadQuality.Undefined, RoadQuality.Wild, RoadQuality.Dirt, RoadQuality.Paved, RoadQuality.Highway },
            AlternateRouteRatio = redundantRoadRemover.AlternateRouteRatio,
            CasualDistance = settlementBuilder.MinimumDistance * 2,
            LaidRoadQuality = RoadQuality.Paved,
            SettlementSizes = { SettlementSize.Town, SettlementSize.City },
        };
        mapGenerator.AddTask(distantCityRoadLayer);

        // Roads between cities are promoted to highways.
        DistantCityRoadLayer highwayRoadLayer = new()
        {
            Name = "Laying highways between cities...",
            AllowedRoadQualities = { RoadQuality.Paved, RoadQuality.Highway },
            AlternateRouteRatio = redundantRoadRemover.AlternateRouteRatio,
            CasualDistance = layoutGenerator.MaximumRoadLength * 5,
            LaidRoadQuality = RoadQuality.Highway,
            SettlementSizes = { SettlementSize.City },
        };
        mapGenerator.AddTask(highwayRoadLayer);

        // Build service stations on long routes.
        ServiceProvider serviceProvider = new()
        {
            Name = "Providing service to regional districts...",
            AllowedRoadQualities = { RoadQuality.Paved, RoadQuality.Highway },
            AllowedSettlementSizes = { SettlementSize.Service, SettlementSize.Town, SettlementSize.City },
            MaximumDistance = settlementBuilder.MinimumDistance * 3 / 2,
            SettlementSize = SettlementSize.Service,
        };
        mapGenerator.AddTask(serviceProvider);

        // Homesteads beat out a driveway to the nearest road.
        DrivewayLayer homesteadDrivewayLayer = new()
        {
            Name = "Laying homestead driveways...",
            AllowedRoadQualities = { RoadQuality.Undefined, RoadQuality.Dirt },
            LaidRoadQuality = RoadQuality.Dirt,
            SettlementSizes = { SettlementSize.Homestead },
            TargetRoadQualities = { RoadQuality.Paved , RoadQuality.Highway },
            TargetSettlementSizes = { SettlementSize.Town, SettlementSize.City },
        };
        mapGenerator.AddTask(homesteadDrivewayLayer);

        // All other potential roads are only available by bush-bashing.
        RoadFinalizer roadFinalizer = new()
        {
            Name = "Removing all non-roads...",
            LaidRoadQuality = RoadQuality.Wild
        };
        mapGenerator.AddTask(roadFinalizer);

        // All other junctions are marked as unsettled (or, perhaps, vacant.)
        JunctionFinalizer junctionFinalizer = new()
        {
            Name = "Removing all non-settlements..."
        };
        mapGenerator.AddTask(junctionFinalizer);

        // Roads are sorted so that higher quality roads are painted on top of lower quality roads.
        RoadSorter roadSorter = new()
        {
            Name = "Sorting roads..."
        };
        mapGenerator.AddTask(roadSorter);

        mapGenerator.Initialize();

        return mapGenerator;
    }

    /// <summary>
    /// Returns the default implementation of the MapGenerator class using a random seed.
    /// </summary>
    /// <param name="junctions">The number of junctions to create.</param>
    /// <returns>An initialised MapGenerator class.</returns>
    public static MapGenerator Default(int junctions)
    {
        return Default(junctions, DateTime.Now.Ticks.GetHashCode());
    }
}
