const allRecipes = [
    {
        "name": "Mushroom Mountain",
        "value": 80,
        "ingredients": [ "Bluecap", "Bluecap" ]
    },
    {
        "name": "Meteoric Mushroom",
        "value": 100,
        "ingredients": [ "Greenstalk", "Bluecap" ]
    },
    {
        "name": "Double Mush Potage",
        "value": 70,
        "ingredients": [ "Greenstalk", "Greenstalk" ]
    },
    {
        "name": "Purple Haze",
        "value": 220,
        "ingredients": [ "Poisonpuff", "Bluecap" ]
    },
    {
        "name": "Mushstrone",
        "value": 120,
        "ingredients": [ "Poisonpuff", "Greenstalk" ]
    },
    {
        "name": "Mushroom Bomber",
        "value": 40,
        "ingredients": [ "Poisonpuff", "Poisonpuff" ]
    },
    {
        "name": "Cream of Mushgrass",
        "value": 70,
        "ingredients": [ "Stabgrass", "Bluecap" ]
    },
    {
        "name": "Sticky Grass Stew",
        "value": 90,
        "ingredients": [ "Stabgrass", "Greenstalk" ]
    },
    {
        "name": "Murky Marsh",
        "value": 130,
        "ingredients": [ "Stabgrass", "Poisonpuff" ]
    },
    {
        "name": "Monobrew",
        "value": 70,
        "ingredients": [ "Stabgrass", "Stabgrass" ]
    },
    {
        "name": "Mushroom Net",
        "value": 110,
        "ingredients": [ "Tsutavine", "Bluecap" ]
    },
    {
        "name": "Vineshroom",
        "value": 100,
        "ingredients": [ "Tsutavine", "Greenstalk" ]
    },
    {
        "name": "Creme de Creeper",
        "value": 20,
        "ingredients": [ "Tsutavine", "Poisonpuff" ]
    },
    {
        "name": "Turf Noodle ",
        "value": 140,
        "ingredients": [ "Tsutavine", "Stabgrass" ]
    },
    {
        "name": "Dooble Noodle Soup",
        "value": 90,
        "ingredients": [ "Tsutavine", "Tsutavine" ]
    },
    {
        "name": "Seabrine Shroom",
        "value": 120,
        "ingredients": [ "Brineweed", "Bluecap" ]
    },
    {
        "name": "Split Sea Soup",
        "value": 100,
        "ingredients": [ "Brineweed", "Greenstalk" ]
    },
    {
        "name": "Depth Charge",
        "value": 260,
        "ingredients": [ "Brineweed", "Poisonpuff" ]
    },
    {
        "name": "Herbe De La Mer",
        "value": 300,
        "ingredients": [ "Brineweed", "Stabgrass" ]
    },
    {
        "name": "Wonder Wakame",
        "value": 130,
        "ingredients": [ "Brineweed", "Tsutavine" ]
    },
    {
        "name": "Deep Brineweed Porridge",
        "value": 80,
        "ingredients": [ "Brineweed", "Brineweed" ]
    },
    {
        "name": "Mellow Mushroom",
        "value": 120,
        "ingredients": [ "Corn Shell", "Bluecap" ]
    },
    {
        "name": "Cosmo Cornfield",
        "value": 100,
        "ingredients": [ "Corn Shell", "Greenstalk" ]
    },
    {
        "name": "Gloomaize",
        "value": 260,
        "ingredients": [ "Corn Shell", "Poisonpuff" ]
    },
    {
        "name": "Husker Dew",
        "value": 300,
        "ingredients": [ "Corn Shell", "Stabgrass" ]
    },
    {
        "name": "Nubby Noodle",
        "value": 140,
        "ingredients": [ "Corn Shell", "Tsutavine" ]
    },
    {
        "name": "Salty Cob",
        "value": 150,
        "ingredients": [ "Corn Shell", "Brineweed" ]
    },
    {
        "name": "Creamed Cornbug",
        "value": 100,
        "ingredients": [ "Corn Shell", "Corn Shell" ]
    },
    {
        "name": "Tomushroom",
        "value": 130,
        "ingredients": [ "Tomaty Steak", "Bluecap" ]
    },
    {
        "name": "Red Roast Delight",
        "value": 200,
        "ingredients": [ "Tomaty Steak", "Greenstalk" ]
    },
    {
        "name": "Bursting Buzz",
        "value": 160,
        "ingredients": [ "Tomaty Steak", "Poisonpuff" ]
    },
    {
        "name": "Grassy Gazpacho",
        "value": 90,
        "ingredients": [ "Tomaty Steak", "Stabgrass" ]
    },
    {
        "name": "Tomaty Squeeze",
        "value": 140,
        "ingredients": [ "Tomaty Steak", "Tsutavine" ]
    },
    {
        "name": "Carmine Comet",
        "value": 250,
        "ingredients": [ "Tomaty Steak", "Brineweed" ]
    },
    {
        "name": "Chile Con Cornbug",
        "value": 160,
        "ingredients": [ "Tomaty Steak", "Corn Shell" ]
    },
    {
        "name": "Major Tom",
        "value": 170,
        "ingredients": [ "Tomaty Steak", "Tomaty Steak" ]
    },
    {
        "name": "Mineral Mash",
        "value": 120,
        "ingredients": [ "Squidfly Chunk", "Bluecap" ]
    },
    {
        "name": "Slimy Shroom",
        "value": 30,
        "ingredients": [ "Squidfly Chunk", "Greenstalk" ]
    },
    {
        "name": "Incredible Ink",
        "value": 120,
        "ingredients": [ "Squidfly Chunk", "Poisonpuff" ]
    },
    {
        "name": "Hairy Squid",
        "value": 40,
        "ingredients": [ "Squidfly Chunk", "Stabgrass" ]
    },
    {
        "name": "Turbo Tentacle",
        "value": 300,
        "ingredients": [ "Squidfly Chunk", "Tsutavine" ]
    },
    {
        "name": "Sea Urchin Ceviche",
        "value": 240,
        "ingredients": [ "Squidfly Chunk", "Brineweed" ]
    },
    {
        "name": "Fly Chowder",
        "value": 150,
        "ingredients": [ "Squidfly Chunk", "Corn Shell" ]
    },
    {
        "name": "Flyin' Tomaty",
        "value": 150,
        "ingredients": [ "Squidfly Chunk", "Tomaty Steak" ]
    },
    {
        "name": "Squid Singularity",
        "value": 70,
        "ingredients": [ "Squidfly Chunk", "Squidfly Chunk" ]
    },
    {
        "name": "Blueberry Scales",
        "value": 130,
        "ingredients": [ "Strawburi Filet", "Bluecap" ]
    },
    {
        "name": "Strawshroom Spectrum",
        "value": 120,
        "ingredients": [ "Strawburi Filet", "Greenstalk" ]
    },
    {
        "name": "Trench Toad",
        "value": 160,
        "ingredients": [ "Strawburi Filet", "Poisonpuff" ]
    },
    {
        "name": "Strawburi Field",
        "value": 130,
        "ingredients": [ "Strawburi Filet", "Stabgrass" ]
    },
    {
        "name": "Tangleburi Twist",
        "value": 40,
        "ingredients": [ "Strawburi Filet", "Tsutavine" ]
    },
    {
        "name": "Bitter Sweet Broth",
        "value": 30,
        "ingredients": [ "Strawburi Filet", "Brineweed" ]
    },
    {
        "name": "Cornburi en Croute",
        "value": 160,
        "ingredients": [ "Strawburi Filet", "Corn Shell" ]
    },
    {
        "name": "Bloodberry Chowder",
        "value": 50,
        "ingredients": [ "Strawburi Filet", "Tomaty Steak" ]
    },
    {
        "name": "Red Ink Rasam",
        "value": 10,
        "ingredients": [ "Strawburi Filet", "Squidfly Chunk" ]
    },
    {
        "name": "Strawburi Jam",
        "value": 180,
        "ingredients": [ "Strawburi Filet", "Strawburi Filet" ]
    },
    {
        "name": "Al Fungi",
        "value": 160,
        "ingredients": [ "Pinapurana Filet", "Bluecap" ]
    },
    {
        "name": "Verdant Fang",
        "value": 50,
        "ingredients": [ "Pinapurana Filet", "Greenstalk" ]
    },
    {
        "name": "Pinestool",
        "value": 170,
        "ingredients": [ "Pinapurana Filet", "Poisonpuff" ]
    },
    {
        "name": "Green Snapper",
        "value": 40,
        "ingredients": [ "Pinapurana Filet", "Stabgrass" ]
    },
    {
        "name": "Dropfruit Soup",
        "value": 200,
        "ingredients": [ "Pinapurana Filet", "Tsutavine" ]
    },
    {
        "name": "Bottom Feeder",
        "value": 130,
        "ingredients": [ "Pinapurana Filet", "Brineweed" ]
    },
    {
        "name": "Golden Gullet",
        "value": 40,
        "ingredients": [ "Pinapurana Filet", "Corn Shell" ]
    },
    {
        "name": "Razor Rasam",
        "value": 170,
        "ingredients": [ "Pinapurana Filet", "Tomaty Steak" ]
    },
    {
        "name": "Chum Chowder",
        "value": 160,
        "ingredients": [ "Pinapurana Filet", "Squidfly Chunk" ]
    },
    {
        "name": "Heavy Syrup",
        "value": 300,
        "ingredients": [ "Pinapurana Filet", "Strawburi Filet" ]
    },
    {
        "name": "StarTropic",
        "value": 150,
        "ingredients": [ "Pinapurana Filet", "Pinapurana Filet" ]
    },
    {
        "name": "Sloppy Shroom",
        "value": 40,
        "ingredients": [ "Kabo Chunk", "Bluecap" ]
    },
    {
        "name": "Equinosh",
        "value": 150,
        "ingredients": [ "Kabo Chunk", "Greenstalk" ]
    },
    {
        "name": "Purple Jack",
        "value": 160,
        "ingredients": [ "Kabo Chunk", "Poisonpuff" ]
    },
    {
        "name": "TumbleTurf Soup",
        "value": 170,
        "ingredients": [ "Kabo Chunk", "Stabgrass" ]
    },
    {
        "name": "Vegan Vine Vichy",
        "value": 220,
        "ingredients": [ "Kabo Chunk", "Tsutavine" ]
    },
    {
        "name": "Brine Rind Boil",
        "value": 50,
        "ingredients": [ "Kabo Chunk", "Brineweed" ]
    },
    {
        "name": "Harvest Brew ",
        "value": 160,
        "ingredients": [ "Kabo Chunk", "Corn Shell" ]
    },
    {
        "name": "Squashed Tomaty Treat",
        "value": 160,
        "ingredients": [ "Kabo Chunk", "Tomaty Steak" ]
    },
    {
        "name": "Squalid Squash",
        "value": 40,
        "ingredients": [ "Kabo Chunk", "Squidfly Chunk" ]
    },
    {
        "name": "Snozberry Stew",
        "value": 160,
        "ingredients": [ "Kabo Chunk", "Strawburi Filet" ]
    },
    {
        "name": "Flipfang Chowder",
        "value": 160,
        "ingredients": [ "Kabo Chunk", "Pinapurana Filet" ]
    },
    {
        "name": "Giga Gourd ",
        "value": 210,
        "ingredients": [ "Kabo Chunk", "Kabo Chunk" ]
    },
    {
        "name": "Blue Screen",
        "value": 40,
        "ingredients": [ "Oxygrass", "Bluecap" ]
    },
    {
        "name": "Morning Mushroom",
        "value": 260,
        "ingredients": [ "Oxygrass", "Greenstalk" ]
    },
    {
        "name": "Dynamite Trip",
        "value": 250,
        "ingredients": [ "Oxygrass", "Poisonpuff" ]
    },
    {
        "name": "Antigrav Grass",
        "value": 300,
        "ingredients": [ "Oxygrass", "Stabgrass" ]
    },
    {
        "name": "Ventvine Stew",
        "value": 200,
        "ingredients": [ "Oxygrass", "Tsutavine" ]
    },
    {
        "name": "Bouillon du Bends",
        "value": 20,
        "ingredients": [ "Oxygrass", "Brineweed" ]
    },
    {
        "name": "Popcorn Potage",
        "value": 190,
        "ingredients": [ "Oxygrass", "Corn Shell" ]
    },
    {
        "name": "Brilliant Tomato",
        "value": 220,
        "ingredients": [ "Oxygrass", "Tomaty Steak" ]
    },
    {
        "name": "Aerosquid Bisque",
        "value": 190,
        "ingredients": [ "Oxygrass", "Squidfly Chunk" ]
    },
    {
        "name": "Strawburi-Ade",
        "value": 220,
        "ingredients": [ "Oxygrass", "Strawburi Filet" ]
    },
    {
        "name": "Pulsing Pine",
        "value": 200,
        "ingredients": [ "Oxygrass", "Pinapurana Filet" ]
    },
    {
        "name": "Brisk Burster",
        "value": 220,
        "ingredients": [ "Oxygrass", "Kabo Chunk" ]
    },
    {
        "name": "Bling Bling Burble",
        "value": 220,
        "ingredients": [ "Oxygrass", "Oxygrass" ]
    },
    {
        "name": "Wattlecap",
        "value": 30,
        "ingredients": [ "Chickenberry", "Bluecap" ]
    },
    {
        "name": "Chicken of the Cave",
        "value": 140,
        "ingredients": [ "Chickenberry", "Greenstalk" ]
    },
    {
        "name": "Full-Bodied Fryer",
        "value": 300,
        "ingredients": [ "Chickenberry", "Poisonpuff" ]
    },
    {
        "name": "Hungry Hen",
        "value": 150,
        "ingredients": [ "Chickenberry", "Stabgrass" ]
    },
    {
        "name": "Chicken Noodle Cluster",
        "value": 160,
        "ingredients": [ "Chickenberry", "Tsutavine" ]
    },
    {
        "name": "Brinebird Bouillon",
        "value": 170,
        "ingredients": [ "Chickenberry", "Brineweed" ]
    },
    {
        "name": "Chicken Feed",
        "value": 270,
        "ingredients": [ "Chickenberry", "Corn Shell" ]
    },
    {
        "name": "Parmesan Potage",
        "value": 200,
        "ingredients": [ "Chickenberry", "Tomaty Steak" ]
    },
    {
        "name": "Squid Smuggler",
        "value": 140,
        "ingredients": [ "Chickenberry", "Squidfly Chunk" ]
    },
    {
        "name": "Mixed Berry Broth",
        "value": 320,
        "ingredients": [ "Chickenberry", "Strawburi Filet" ]
    },
    {
        "name": "Chicken Royale",
        "value": 190,
        "ingredients": [ "Chickenberry", "Pinapurana Filet" ]
    },
    {
        "name": "Pollo Peligroso",
        "value": 400,
        "ingredients": [ "Chickenberry", "Kabo Chunk" ]
    },
    {
        "name": "Rooster Rocket",
        "value": 90,
        "ingredients": [ "Chickenberry", "Oxygrass" ]
    },
    {
        "name": "Two Piece Twister",
        "value": 160,
        "ingredients": [ "Chickenberry", "Chickenberry" ]
    },
    {
        "name": "Woodland Spike",
        "value": 170,
        "ingredients": [ "Thornstalk", "Bluecap" ]
    },
    {
        "name": "Stellar Shroom",
        "value": 300,
        "ingredients": [ "Thornstalk", "Greenstalk" ]
    },
    {
        "name": "Gouger Gruel",
        "value": 40,
        "ingredients": [ "Thornstalk", "Poisonpuff" ]
    },
    {
        "name": "Cosmic Nettle",
        "value": 100,
        "ingredients": [ "Thornstalk", "Stabgrass" ]
    },
    {
        "name": "Prickle Pickle",
        "value": 120,
        "ingredients": [ "Thornstalk", "Tsutavine" ]
    },
    {
        "name": "Submarine Stinger",
        "value": 130,
        "ingredients": [ "Thornstalk", "Brineweed" ]
    },
    {
        "name": "Choppy Chowder",
        "value": 210,
        "ingredients": [ "Thornstalk", "Corn Shell" ]
    },
    {
        "name": "Redspine Stew",
        "value": 140,
        "ingredients": [ "Thornstalk", "Tomaty Steak" ]
    },
    {
        "name": "Cosmo Kraken",
        "value": 220,
        "ingredients": [ "Thornstalk", "Squidfly Chunk" ]
    },
    {
        "name": "Blowfish Bisque",
        "value": 120,
        "ingredients": [ "Thornstalk", "Strawburi Filet" ]
    },
    {
        "name": "Prickly Pine",
        "value": 20,
        "ingredients": [ "Thornstalk", "Pinapurana Filet" ]
    },
    {
        "name": "Snarecrow",
        "value": 140,
        "ingredients": [ "Thornstalk", "Kabo Chunk" ]
    },
    {
        "name": "Puncture Punch",
        "value": 200,
        "ingredients": [ "Thornstalk", "Oxygrass" ]
    },
    {
        "name": "Danger Drumstick",
        "value": 50,
        "ingredients": [ "Thornstalk", "Chickenberry" ]
    },
    {
        "name": "Thick Thorn Stew",
        "value": 20,
        "ingredients": [ "Thornstalk", "Thornstalk" ]
    },
    {
        "name": "Wildbloom",
        "value": 180,
        "ingredients": [ "Thornbloom", "Bluecap" ]
    },
    {
        "name": "Natural Low",
        "value": 30,
        "ingredients": [ "Thornbloom", "Greenstalk" ]
    },
    {
        "name": "Nutrient Nova",
        "value": 500,
        "ingredients": [ "Thornbloom", "Poisonpuff" ]
    },
    {
        "name": "Ochre Orchid",
        "value": 180,
        "ingredients": [ "Thornbloom", "Stabgrass" ]
    },
    {
        "name": "Pliant Petal Potage",
        "value": 70,
        "ingredients": [ "Thornbloom", "Tsutavine" ]
    },
    {
        "name": "Sour Sea Slurp",
        "value": 80,
        "ingredients": [ "Thornbloom", "Brineweed" ]
    },
    {
        "name": "Sweet 'N' Flower Soup",
        "value": 90,
        "ingredients": [ "Thornbloom", "Corn Shell" ]
    },
    {
        "name": "Lucky Tom",
        "value": 210,
        "ingredients": [ "Thornbloom", "Tomaty Steak" ]
    },
    {
        "name": "Buzzy Lotus",
        "value": 180,
        "ingredients": [ "Thornbloom", "Squidfly Chunk" ]
    },
    {
        "name": "Strawburi Bloom",
        "value": 210,
        "ingredients": [ "Thornbloom", "Strawburi Filet" ]
    },
    {
        "name": "Squidfly Trap ",
        "value": 220,
        "ingredients": [ "Thornbloom", "Pinapurana Filet" ]
    },
    {
        "name": "Duskdrop Soup",
        "value": 210,
        "ingredients": [ "Thornbloom", "Kabo Chunk" ]
    },
    {
        "name": "Fizzy Lifting Drink",
        "value": 250,
        "ingredients": [ "Thornbloom", "Oxygrass" ]
    },
    {
        "name": "Cosmic Stuffing",
        "value": 230,
        "ingredients": [ "Thornbloom", "Chickenberry" ]
    },
    {
        "name": "Mandala Mash",
        "value": 350,
        "ingredients": [ "Thornbloom", "Thornstalk" ]
    },
    {
        "name": "Twin Luna",
        "value": 300,
        "ingredients": [ "Thornbloom", "Thornbloom" ]
    },
    {
        "name": "Clashroom Chowder",
        "value": 120,
        "ingredients": [ "Sunblossom", "Bluecap" ]
    },
    {
        "name": "Green Sun Chowder",
        "value": 30,
        "ingredients": [ "Sunblossom", "Greenstalk" ]
    },
    {
        "name": "Daylight Dynamite",
        "value": 150,
        "ingredients": [ "Sunblossom", "Poisonpuff" ]
    },
    {
        "name": "Sunflower Stodge",
        "value": 20,
        "ingredients": [ "Sunblossom", "Stabgrass" ]
    },
    {
        "name": "Earlybird",
        "value": 180,
        "ingredients": [ "Sunblossom", "Tsutavine" ]
    },
    {
        "name": "Seaflower Stew",
        "value": 110,
        "ingredients": [ "Sunblossom", "Brineweed" ]
    },
    {
        "name": "Corn Impact",
        "value": 90,
        "ingredients": [ "Sunblossom", "Corn Shell" ]
    },
    {
        "name": "Sun-Dyed Tomaty",
        "value": 150,
        "ingredients": [ "Sunblossom", "Tomaty Steak" ]
    },
    {
        "name": "Squidflower Six",
        "value": 290,
        "ingredients": [ "Sunblossom", "Squidfly Chunk" ]
    },
    {
        "name": "Sweet Flower Porridge",
        "value": 250,
        "ingredients": [ "Sunblossom", "Strawburi Filet" ]
    },
    {
        "name": "Filet of Fission",
        "value": 160,
        "ingredients": [ "Sunblossom", "Pinapurana Filet" ]
    },
    {
        "name": "Orange Orbit",
        "value": 150,
        "ingredients": [ "Sunblossom", "Kabo Chunk" ]
    },
    {
        "name": "Zephyr Broth",
        "value": 400,
        "ingredients": [ "Sunblossom", "Oxygrass" ]
    },
    {
        "name": "Red Line Rotisserie",
        "value": 170,
        "ingredients": [ "Sunblossom", "Chickenberry" ]
    },
    {
        "name": "Solar Spike Stew",
        "value": 130,
        "ingredients": [ "Sunblossom", "Thornstalk" ]
    },
    {
        "name": "Flareburst",
        "value": 300,
        "ingredients": [ "Sunblossom", "Thornbloom" ]
    },
    {
        "name": "Double Rainbow Dew",
        "value": 130,
        "ingredients": [ "Sunblossom", "Sunblossom" ]
    },
    {
        "name": "Mushed Potato",
        "value": 30,
        "ingredients": [ "Masher Yam", "Bluecap" ]
    },
    {
        "name": "Mountain Melange",
        "value": 170,
        "ingredients": [ "Masher Yam", "Greenstalk" ]
    },
    {
        "name": "Dark Masher",
        "value": 140,
        "ingredients": [ "Masher Yam", "Poisonpuff" ]
    },
    {
        "name": "Grass Roots",
        "value": 100,
        "ingredients": [ "Masher Yam", "Stabgrass" ]
    },
    {
        "name": "Truffle Shuffle",
        "value": 180,
        "ingredients": [ "Masher Yam", "Tsutavine" ]
    },
    {
        "name": "Tidal Tuber",
        "value": 130,
        "ingredients": [ "Masher Yam", "Brineweed" ]
    },
    {
        "name": "Cornpost",
        "value": 20,
        "ingredients": [ "Masher Yam", "Corn Shell" ]
    },
    {
        "name": "Red Root Revolution",
        "value": 140,
        "ingredients": [ "Masher Yam", "Tomaty Steak" ]
    },
    {
        "name": "Masher Lasher",
        "value": 130,
        "ingredients": [ "Masher Yam", "Squidfly Chunk" ]
    },
    {
        "name": "Solar Spudding",
        "value": 140,
        "ingredients": [ "Masher Yam", "Strawburi Filet" ]
    },
    {
        "name": "Codswallow",
        "value": 150,
        "ingredients": [ "Masher Yam", "Pinapurana Filet" ]
    },
    {
        "name": "Harvest Moon",
        "value": 140,
        "ingredients": [ "Masher Yam", "Kabo Chunk" ]
    },
    {
        "name": "Porous Potato",
        "value": 200,
        "ingredients": [ "Masher Yam", "Oxygrass" ]
    },
    {
        "name": "Gaseous Gumbo",
        "value": 160,
        "ingredients": [ "Masher Yam", "Chickenberry" ]
    },
    {
        "name": "Sauerkraut Circus",
        "value": 120,
        "ingredients": [ "Masher Yam", "Thornstalk" ]
    },
    {
        "name": "Sopa Blanca",
        "value": 190,
        "ingredients": [ "Masher Yam", "Thornbloom" ]
    },
    {
        "name": "Hot Potato",
        "value": 350,
        "ingredients": [ "Masher Yam", "Sunblossom" ]
    },
    {
        "name": "Potato Porridge",
        "value": 30,
        "ingredients": [ "Masher Yam", "Masher Yam" ]
    },
    {
        "name": "Blue Bison",
        "value": 190,
        "ingredients": [ "Bisausage", "Bluecap" ]
    },
    {
        "name": "Green Grinder",
        "value": 200,
        "ingredients": [ "Bisausage", "Greenstalk" ]
    },
    {
        "name": "Pungent Porridge",
        "value": 220,
        "ingredients": [ "Bisausage", "Poisonpuff" ]
    },
    {
        "name": "Haymaker ",
        "value": 190,
        "ingredients": [ "Bisausage", "Stabgrass" ]
    },
    {
        "name": "Wild Link Stew",
        "value": 190,
        "ingredients": [ "Bisausage", "Tsutavine" ]
    },
    {
        "name": "Water Bison",
        "value": 210,
        "ingredients": [ "Bisausage", "Brineweed" ]
    },
    {
        "name": "Pozole Planet",
        "value": 320,
        "ingredients": [ "Bisausage", "Corn Shell" ]
    },
    {
        "name": "Meaty 'Maty",
        "value": 220,
        "ingredients": [ "Bisausage", "Tomaty Steak" ]
    },
    {
        "name": "Black Hole Bison",
        "value": 210,
        "ingredients": [ "Bisausage", "Squidfly Chunk" ]
    },
    {
        "name": "Sweetmeat Soup",
        "value": 220,
        "ingredients": [ "Bisausage", "Strawburi Filet" ]
    },
    {
        "name": "Light Year Luau",
        "value": 280,
        "ingredients": [ "Bisausage", "Pinapurana Filet" ]
    },
    {
        "name": "Gutted Gourd",
        "value": 40,
        "ingredients": [ "Bisausage", "Kabo Chunk" ]
    },
    {
        "name": "Psycho Bison",
        "value": 100,
        "ingredients": [ "Bisausage", "Oxygrass" ]
    },
    {
        "name": "Nitrate Nebula",
        "value": 90,
        "ingredients": [ "Bisausage", "Chickenberry" ]
    },
    {
        "name": "Spiny Sausage",
        "value": 180,
        "ingredients": [ "Bisausage", "Thornstalk" ]
    },
    {
        "name": "Petal Pounder Potage",
        "value": 270,
        "ingredients": [ "Bisausage", "Thornbloom" ]
    },
    {
        "name": "Chorizo Chowder",
        "value": 210,
        "ingredients": [ "Bisausage", "Sunblossom" ]
    },
    {
        "name": "Big Bangers 'N' Mash",
        "value": 150,
        "ingredients": [ "Bisausage", "Masher Yam" ]
    },
    {
        "name": "Sausage Party",
        "value": 300,
        "ingredients": [ "Bisausage", "Bisausage" ]
    },
    {
        "name": "Mushroom Monarchy",
        "value": 450,
        "ingredients": [ "Mammoth Meat", "Bluecap" ]
    },
    {
        "name": "Primal Potage",
        "value": 260,
        "ingredients": [ "Mammoth Meat", "Greenstalk" ]
    },
    {
        "name": "Dizzy Mammoth",
        "value": 100,
        "ingredients": [ "Mammoth Meat", "Poisonpuff" ]
    },
    {
        "name": "Woolly Back Beef",
        "value": 270,
        "ingredients": [ "Mammoth Meat", "Stabgrass" ]
    },
    {
        "name": "Raw Hide Ramen",
        "value": 120,
        "ingredients": [ "Mammoth Meat", "Tsutavine" ]
    },
    {
        "name": "Rough River",
        "value": 290,
        "ingredients": [ "Mammoth Meat", "Brineweed" ]
    },
    {
        "name": "Mammoth's Eye",
        "value": 290,
        "ingredients": [ "Mammoth Meat", "Corn Shell" ]
    },
    {
        "name": "Red Giant",
        "value": 400,
        "ingredients": [ "Mammoth Meat", "Tomaty Steak" ]
    },
    {
        "name": "Poacher's Potion",
        "value": 350,
        "ingredients": [ "Mammoth Meat", "Squidfly Chunk" ]
    },
    {
        "name": "Berry Basher",
        "value": 300,
        "ingredients": [ "Mammoth Meat", "Strawburi Filet" ]
    },
    {
        "name": "Pinapurana Jerky",
        "value": 330,
        "ingredients": [ "Mammoth Meat", "Pinapurana Filet" ]
    },
    {
        "name": "Kabo Consomme",
        "value": 270,
        "ingredients": [ "Mammoth Meat", "Kabo Chunk" ]
    },
    {
        "name": "Stampede High",
        "value": 300,
        "ingredients": [ "Mammoth Meat", "Oxygrass" ]
    },
    {
        "name": "Protein Dream",
        "value": 350,
        "ingredients": [ "Mammoth Meat", "Chickenberry" ]
    },
    {
        "name": "Tender Tusk",
        "value": 200,
        "ingredients": [ "Mammoth Meat", "Thornstalk" ]
    },
    {
        "name": "Handsome Beast",
        "value": 350,
        "ingredients": [ "Mammoth Meat", "Thornbloom" ]
    },
    {
        "name": "Heliotripe",
        "value": 150,
        "ingredients": [ "Mammoth Meat", "Sunblossom" ]
    },
    {
        "name": "Yammoth Broth",
        "value": 350,
        "ingredients": [ "Mammoth Meat", "Masher Yam" ]
    },
    {
        "name": "Meatageddon",
        "value": 360,
        "ingredients": [ "Mammoth Meat", "Bisausage" ]
    },
    {
        "name": "Mega Mammoth",
        "value": 400,
        "ingredients": [ "Mammoth Meat", "Mammoth Meat" ]
    }
];