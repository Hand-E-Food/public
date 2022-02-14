Atlas Solver
============

A dump of tools for Ticket to Ride boards. The primary tool allows the user to freely place tracks
to try achieve the highest possible score on a Ticket to Ride board.

https://raw.githack.com/Hand-E-Food/public/master/atlas-solver/


Data
----

Data files consist of a JSON and an optional image.

```json
{
    "title": string, // Web page title
    "image": string, // Optional. Filename of image, path relative to `index.html`
    "width":  int, // Image width. Required even if image is omitted.
    "height": int, // Image height. Required even if image is omitted.
    "cities": [ // List of cities.
        {
            "x": int, // City's x-position on image.
            "y": int, // City's y-position on image.
            "name": string, // City's name.
        },
        ...
    ],
    "tracks": [
        {
            "distance": int, // The number of cards required to build this track.
            "colors": [ string, ... ], // The color of each track (single or dual). See valid values below.
            "cities": [ string, string ], // The names of the two cities this track connects.
            "tunnel": boolean, // Optional. True if this track is a tunnel.
            "engines": int, // Optional. Number of rainbow cards included in this track's distance.
        },
        ...
    ],
    "routes": [
        {
            "points": int, // The number of points this route is worth. Typically the minimum number of pieces required to join the cities.
            "cities": [ string, string ], // The names of the two cities this route wants to connect.
            "long": boolean, // Optional. True if this is a long route (ie. each player is given one at the start of the game).
        },
        ...
    ],
}
```

As data files are added, also add them to `index.html`.

Track colors
------------

| Code  | Color |
| ----- | ----- |
| `"a"` | gray (any color) |
| `"w"` | white |
| `"p"` | pink |
| `"r"` | red |
| `"o"` | orange |
| `"y"` | yellow |
| `"g"` | green |
| `"b"` | blue |
| `"k"` | black |
| `"?"` | any color, including gray (for build tools) |
| `"!"` | any color, not gray (for build tools) |
