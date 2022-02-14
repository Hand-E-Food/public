Atlas
=====

A tool to help find locations on a map. Originally made for the Ticket to Ride board games, but suitable for any 2D map.

https://raw.githack.com/Hand-E-Food/public/master/atlas/


Data
----

Data files consist of a JSON and an optional image.

```json
{
    "title": string, // Web page title
    "image": string, // Optional. Filename of image, path relative to `index.html`
    "width":  int, // Image width. Required even if image is omitted.
    "height": int, // Image height. Required even if image is omitted.
    "locations": [ // List of locations. Order is preserved.
        {
            "x": int, // Location's x-position on image.
            "y": int, // Location's y-position on image.
            "name": string, // Location's name in the index.
        },
        ...
    ],
}
```

As data files are added, also add them to `index.html`.
