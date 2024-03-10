using System;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{
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
            var mapGenerator = new MapGenerator();
            mapGenerator.Random = new Random(seed);

            // The island comproses of triangles of a (mostly) fixed area.  Roads are between 5 and 15km long.
            var layoutGenerator = new LayoutGenerator();
            layoutGenerator.Name = "Generating island dimensions...";
            layoutGenerator.MaximumJunctions = junctions;
            layoutGenerator.MaximumRoadLength = 150;
            layoutGenerator.MinimumRoadLength = 50;
            layoutGenerator.ZoneGenerator = new FixedAreaTriangleGenerator();
            mapGenerator.AddTask(layoutGenerator);

            // The map extremities are placed equidistantly from the centre axes.
            var mapCentralizer = new MapCentralizer();
            mapCentralizer.Name = "Centralizing island...";
            mapGenerator.AddTask(mapCentralizer);

            // The ocean surrounds all other land and is filled with salt water.
            var outsideIdentifier = new OutsideIdentifier();
            outsideIdentifier.Name = "Filling ocean...";
            outsideIdentifier.Biome = Biome.Ocean;
            mapGenerator.AddTask(outsideIdentifier);

            // Biomes are seeded randomly on the map.  Larger maps have more seeds.
            var biomeSeeder = new BiomeSeeder();
            biomeSeeder.Name = "Seeding biomes...";
            int biomeSets = (int)Math.Round(layoutGenerator.MaximumJunctions / 50.0);
            for (int i = 0; i < biomeSets; i++)
                biomeSeeder.Biomes.AddRange(new[] { Biome.Lake, Biome.Desert, Biome.Forest, Biome.Pasture, Biome.Mountain });
            mapGenerator.AddTask(biomeSeeder);

            // Biomes spread out randomly, but statistically evenly.  Smaller biomes are given a better chance
            // to grow.
            var biomeSpreader = new BiomeSpreader();
            biomeSpreader.Name = "Spreading biomes...";
            biomeSpreader.Biomes.Add(Biome.Desert);
            biomeSpreader.Biomes.Add(Biome.Forest);
            biomeSpreader.Biomes.Add(Biome.Pasture);
            biomeSpreader.Biomes.Add(Biome.Mountain);
            mapGenerator.AddTask(biomeSpreader);

            // Archipelagos stuff up the biome spreader algorithm.
            var zoneFinalizer = new ZoneFinalizer();
            zoneFinalizer.Name = "Spreading biomes across archipelagos...";
            zoneFinalizer.AllowedBiomes.AddRange(biomeSpreader.Biomes);
            mapGenerator.AddTask(zoneFinalizer);

            // Large mountains are tall and have snow falls.
            var mountainSnowifier = new BiomePromoter();
            mountainSnowifier.Name = "Sprinkling snow...";
            mountainSnowifier.Biome = Biome.Snow;
            mountainSnowifier.Condition = (zone) =>
                zone.Junctions                              // For each of the zone's junctions
                .SelectMany((j) => j.Zones)                 // find all zones that are adjacent to the junction,
                .All((z2) => new[] { Biome.Mountain, Biome.Snow }
                    .Contains(z2.Biome));                   // and confirm they all have mountainous or snowy biomes.
            mapGenerator.AddTask(mountainSnowifier);

            // Lakes drain into the ocean.
            var lakeRiverLayer = new RiverLayer();
            lakeRiverLayer.Name = "Melting snow and draining lakes...";
            lakeRiverLayer.AllowedRoadQualities.Add(RoadQuality.Undefined);
            lakeRiverLayer.AllowedRoadQualities.Add(RoadQuality.None);
            lakeRiverLayer.AllowedRoadQualities.Add(RoadQuality.River);
            lakeRiverLayer.LaidRoadQuality = RoadQuality.River;
            lakeRiverLayer.SourceBiomes.Add(Biome.Lake);
            lakeRiverLayer.SourceBiomes.Add(Biome.Snow);
            lakeRiverLayer.TargetBiomes.Add(Biome.Ocean);
            lakeRiverLayer.TargetBiomes.Add(Biome.Lake);
            mapGenerator.AddTask(lakeRiverLayer);

            // Swamps occur near the ocean in forests with poor drainage.
            var forestSwampifier = new BiomePromoter();
            forestSwampifier.Name = "Swamping forests...";
            forestSwampifier.Biome = Biome.Swamp;
            forestSwampifier.Condition = (zone) =>
                zone.Biome == Biome.Forest                  // Where the zone has a forest biome;
            &&                                              //   and
                zone.Roads                                  // the zone's surrounding roads
                .All((r) => r.Quality != RoadQuality.River) // have no rivers;
            &&                                              //   and
                zone.Junctions                              // for each of the zone's junctions
                .SelectMany((j) => j.Zones)                 // find all zones adjacent to the junction,
                .Any((z2) => z2.Biome == Biome.Ocean);      // and confirm at least one is oceanic.
            mapGenerator.AddTask(forestSwampifier);

            // Roads shorter than 5km are not used.
            var shortRoadRemover = new ShortRoadRemover();
            shortRoadRemover.Name = "Removing poor excuses for roads...";
            shortRoadRemover.MinimumRoadLength = 50;
            mapGenerator.AddTask(shortRoadRemover);

            // Roads are removed where there is an alternate route of similar distance.
            var redundantRoadRemover = new RedundantRoadRemover();
            redundantRoadRemover.Name = "Removing redundant roads...";
            redundantRoadRemover.AlternateRouteRatio = 1.4;
            mapGenerator.AddTask(redundantRoadRemover);

            // Settlements are randomly added to the map.  They spread out into surrounding zones.  All
            // settlements are at least 30km apart (as the crow flies.)
            var settlementBuilder = new SettlementBuilder();
            settlementBuilder.Name = "Settling settlements...";
            settlementBuilder.MaximumCities = (int)(layoutGenerator.MaximumJunctions / 10);
            settlementBuilder.MinimumDistance = (layoutGenerator.MaximumRoadLength + layoutGenerator.MinimumRoadLength) * 3/2;
            settlementBuilder.SettleableBiomes[SettlementSize.Homestead] = new[] { Biome.Pasture };
            settlementBuilder.SettleableBiomes[SettlementSize.Town     ] = new[] { Biome.Pasture, Biome.Desert, Biome.Mountain };
            settlementBuilder.SettleableBiomes[SettlementSize.City     ] = new[] { Biome.Pasture, Biome.Desert, Biome.Forest, Biome.Mountain, Biome.Snow };
            settlementBuilder.SettlementSizeWeights[SettlementSize.Homestead] = 3;
            settlementBuilder.SettlementSizeWeights[SettlementSize.Town     ] = 6;
            settlementBuilder.SettlementSizeWeights[SettlementSize.City     ] = 1;
            mapGenerator.AddTask(settlementBuilder);

            // Roads connect any pair of towns within 60km of each other.  Road networks form routes between more
            // distant towns.
            var distantCityRoadLayer = new DistantCityRoadLayer();
            distantCityRoadLayer.Name = "Laying roads between neighbours...";
            distantCityRoadLayer.AllowedRoadQualities.Add(RoadQuality.Undefined);
            distantCityRoadLayer.AllowedRoadQualities.Add(RoadQuality.Wild);
            distantCityRoadLayer.AllowedRoadQualities.Add(RoadQuality.Dirt);
            distantCityRoadLayer.AllowedRoadQualities.Add(RoadQuality.Paved);
            distantCityRoadLayer.AllowedRoadQualities.Add(RoadQuality.Highway);
            distantCityRoadLayer.AlternateRouteRatio = redundantRoadRemover.AlternateRouteRatio;
            distantCityRoadLayer.CasualDistance = settlementBuilder.MinimumDistance * 2;
            distantCityRoadLayer.LaidRoadQuality = RoadQuality.Paved;
            distantCityRoadLayer.SettlementSizes.Add(SettlementSize.Town);
            distantCityRoadLayer.SettlementSizes.Add(SettlementSize.City);
            mapGenerator.AddTask(distantCityRoadLayer);

            // Roads between cities are promoted to highways.
            var highwayRoadLayer = new DistantCityRoadLayer();
            highwayRoadLayer.Name = "Laying highways between cities...";
            highwayRoadLayer.AllowedRoadQualities.Add(RoadQuality.Paved);
            highwayRoadLayer.AllowedRoadQualities.Add(RoadQuality.Highway);
            highwayRoadLayer.AlternateRouteRatio = redundantRoadRemover.AlternateRouteRatio;
            highwayRoadLayer.CasualDistance = layoutGenerator.MaximumRoadLength * 5;
            highwayRoadLayer.LaidRoadQuality = RoadQuality.Highway;
            highwayRoadLayer.SettlementSizes.Add(SettlementSize.City);
            mapGenerator.AddTask(highwayRoadLayer);

            // Build service stations on long routes.
            var serviceProvider = new ServiceProvider();
            serviceProvider.Name = "Providing service to regional districts...";
            serviceProvider.AllowedRoadQualities.Add(RoadQuality.Paved);
            serviceProvider.AllowedRoadQualities.Add(RoadQuality.Highway);
            serviceProvider.AllowedSettlementSizes.Add(SettlementSize.Service);
            serviceProvider.AllowedSettlementSizes.Add(SettlementSize.Town);
            serviceProvider.AllowedSettlementSizes.Add(SettlementSize.City);
            serviceProvider.MaximumDistance = settlementBuilder.MinimumDistance * 3/2;
            serviceProvider.SettlementSize = SettlementSize.Service;
            mapGenerator.AddTask(serviceProvider);

            // Homesteads beat out a driveway to the nearest road.
            var homesteadDrivewayLayer = new DrivewayLayer();
            homesteadDrivewayLayer.Name = "Laying homestead driveways...";
            homesteadDrivewayLayer.AllowedRoadQualities.Add(RoadQuality.Undefined);
            homesteadDrivewayLayer.AllowedRoadQualities.Add(RoadQuality.Dirt);
            homesteadDrivewayLayer.LaidRoadQuality = RoadQuality.Dirt;
            homesteadDrivewayLayer.SettlementSizes.Add(SettlementSize.Homestead);
            homesteadDrivewayLayer.TargetRoadQualities.Add(RoadQuality.Paved);
            homesteadDrivewayLayer.TargetRoadQualities.Add(RoadQuality.Highway);
            homesteadDrivewayLayer.TargetSettlementSizes.Add(SettlementSize.Town);
            homesteadDrivewayLayer.TargetSettlementSizes.Add(SettlementSize.City);
            mapGenerator.AddTask(homesteadDrivewayLayer);

            // All other potential roads are only available by bush-bashing.
            var roadFinalizer = new RoadFinalizer();
            roadFinalizer.Name = "Removing all non-roads...";
            roadFinalizer.LaidRoadQuality = RoadQuality.Wild;
            mapGenerator.AddTask(roadFinalizer);

            // All other junctions are marked as unsettled (or, perhaps, vacant.)
            var junctionFinalizer = new JunctionFinalizer();
            junctionFinalizer.Name = "Removing all non-settlements...";
            mapGenerator.AddTask(junctionFinalizer);

            // Roads are sorted so that higher quality roads are painted on top of lower quality roads.
            var roadSorter = new RoadSorter();
            roadSorter.Name = "Sorting roads...";
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
}
