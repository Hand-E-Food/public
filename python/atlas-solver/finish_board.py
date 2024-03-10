import json
import random
import sys
from math import ceil
from numbers import Number
from tkinter import ALL
from typing import Callable, Dict, Iterable, List, Set, Tuple

from pyparsing import col

INDENT: str = ' '*4
LONG: int = 20
ANY_COLOR: str = '!'
ANY_THING: str = '?'
UNDECIDED: List[str] = [ANY_THING, ANY_COLOR]
UNCOLORED: str = 'a'
ALL_COLORS: List[str] = ['w', 'p', 'r', 'o', 'y', 'g', 'b', 'k']
UNCOLORED_WEIGHT: float = 5.5


def jsonize(value) -> str:
    if value is None: return 'null'
    if value is True: return 'true'
    if value is False: return 'false'
    if isinstance(value, list): return '[' + ','.join([' ' + jsonize(item) for item in value]) + ' ]'
    if isinstance(value, Number): return str(value)
    return f'"{value}"'


class City:
    region_chars: int = 0
    x_chars: int = 0
    y_chars: int = 0

    def __init__(self, data: dict):
        self.x: int = data.get('x', 0)
        self.y: int = data.get('y', 0)
        self.name: str = data['name']
        self.region: str = data.get('region', None)
        self.terminus: bool = data.get('terminus', False)
        self.tracks: List[Track] = []

        region_length = len(self.region) if self.region else 0
        if City.region_chars < region_length: City.region_chars = region_length

    def to_json(self) -> dict:
        json = {}
        json['x'] = self.x
        json['y'] = self.y
        if self.region: json['region'] = self.region
        json['name'] = self.name
        if self.terminus: json['terminus'] = self.terminus
        return json

    def to_str(self) -> str:
        attributes = []
        attributes.append(f'"x": ' + str(self.x).rjust(City.x_chars))
        attributes.append(f'"y": ' + str(self.y).rjust(City.y_chars))
        if self.region: attributes.append(f'"region": "{self.region}"'.ljust(City.region_chars + 12))
        attributes.append(f'"name": "{self.name}"')
        if self.terminus: attributes.append(f'"terminus": true')
        return '{ ' + ', '.join(attributes) + ' }'


class Track:
    def __init__(self, cities: List[City], data: dict):
        self.distance: int = data['distance']
        self.colors: List[str] = data['colors']
        self.width: int = len(self.colors)
        self.cities: List[City] = [next(city for city in cities if city.name == name) for name in data['cities']]
        self.engines: int = data.get('engines', 0)
        self.tunnel: bool = data.get('tunnel', False)
        for city in self.cities:
            city.tracks.append(self)

    def adjacent_colors(self) -> List[str]:
        for track in self.adjacent_tracks():
            for color in track.colors:
                yield color

    def adjacent_tracks(self) -> Set['Track']:
        adjacent_tracks: Set['Track'] = set()
        for city in self.cities:
            for track in city.tracks:
                adjacent_tracks.add(track)
        return adjacent_tracks

    def other_city(self, city: City) -> City:
        if self.cities[0] == city:
            return self.cities[1]
        elif self.cities[1] == city:
            return self.cities[0]
        else:
            raise ValueError(f'City {city.name} is not attached to this track')

    def to_json(self) -> dict:
        json = {}
        json['distance'] = self.distance
        json['colors'] = self.colors
        json['cities'] = [city.name for city in self.cities]
        if self.engines > 0: json['engines'] = self.engines
        if self.tunnel: json['tunnel'] = self.tunnel
        return json

    def to_str(self) -> str:
        attributes = []
        attributes.append(f'"distance": {self.distance}')
        attributes.append(f'"colors": {jsonize(self.colors)}')
        attributes.append(f'"cities": {jsonize([city.name for city in self.cities])}')
        if self.engines > 0: attributes.append(f'"engines": {self.engines}')
        if self.tunnel: attributes.append('"tunnel": true')
        return '{ ' + ', '.join(attributes) + ' }'


class Route:
    def __init__(self, points: int, cities: List[City]):
        self.points: int = points
        self.cities: List[City] = cities
        self.is_long: bool = points >= LONG
        self.is_international: bool = cities[0].region != cities[1].region

    def to_json(self) -> dict:
        json = {}
        json['points'] = self.points
        json['cities'] = [city.name for city in self.cities]
        if self.is_long: json['long'] = True
        return json

    def to_str(self):
        attributes = []
        attributes.append(f'"points": {self.points:2}')
        attributes.append(f'"cities": {jsonize([city.name for city in self.cities])}')
        if self.is_long: attributes.append('"long": true')
        return '{ ' + ', '.join(attributes) + ' }'


class Map:
    def __init__(self, data: dict):
        self.title: str = data.get('title', None)
        self.image: str = data.get('image', None)
        self.width: int = data.get('width', 1)
        self.height: int = data.get('height', 1)
        self.cities: List[City] = [City(item) for item in data.get('cities', [])]
        self.tracks: List[Track] = [Track(self.cities, item) for item in data.get('tracks', [])]
        self.routes: List[Route] = []
        City.x_chars = len(str(self.width))
        City.y_chars = len(str(self.height))

    def to_json(self) -> dict:
        json = {}
        if self.title: json['title'] = self.title
        if self.image: json['image'] = self.image
        json['width'] = self.width
        json['height'] = self.height
        json['cities'] = [city.to_json() for city in self.cities]
        json['tracks'] = [track.to_json() for track in self.tracks]
        json['routes'] = [route.to_json() for route in self.routes]
        return json
    
    def to_str(self) -> str:
        attributes = []
        if self.title: attributes.append(f'"title": "{self.title}"')
        if self.image: attributes.append(f'"image": "{self.image}"')
        attributes.append(f'"width" : {self.width}')
        attributes.append(f'"height": {self.height}')
        attributes.append(f'"cities": {self.list_to_str(self.cities)}')
        attributes.append(f'"tracks": {self.list_to_str(self.tracks)}')
        attributes.append(f'"routes": {self.list_to_str(self.routes)}')
        return ('{\n' + ',\n'.join(attributes)).replace('\n', '\n' + INDENT) + '\n}'
    
    def list_to_str(self, list: list) -> str:
        if list is None: return 'null'
        if len(list) == 0: return '[]'
        return ('[\n' + ',\n'.join([item.to_str() for item in list])).replace('\n', '\n' + INDENT) + '\n]'


class RouteFactory:
    def __init__(self, map: Map):
        self._all_cities: List[City] = [city for city in map.cities if city.name]

    def find_all_routes(self) -> Iterable[Route]:
        for a in range(1, len(self._all_cities)):
            destinations: List[City] = self._all_cities[:a]
            origin: City = self._all_cities[a]
            waypoints: Dict[City, int] = {origin: 0}
            candidate_cities: Set[City] = set()
            for track in origin.tracks:
                candidate_cities.add(track.other_city(origin))

            while destinations:
                destination, points = self.__find_city_closest_to_origin(candidate_cities, waypoints)
                waypoints[destination] = points
                
                candidate_cities.remove(destination)
                destination_neighbours = (track.other_city(destination) for track in destination.tracks)
                for city in destination_neighbours:
                    if city not in waypoints:
                        candidate_cities.add(city)

                if destination in destinations:
                    destinations.remove(destination)
                    # Routes cannot terminate in adjacent cities, even if the shortest route passes through other cities.
                    if all(track.other_city(destination) != origin for track in destination.tracks):
                        route_cities = [origin, destination]
                        random.shuffle(route_cities)
                        yield Route(waypoints[destination], route_cities)

    def __find_city_closest_to_origin(self, candidate_cities: Set[City], waypoints: Dict[City, int]) -> Tuple[City, int]:
        destination: City = None
        destination_points: int = None
        for city in candidate_cities:
            for track in city.tracks:
                waypoint = track.other_city(city)
                if waypoint in waypoints:
                    track_points = waypoints[waypoint] + track.distance
                    if destination_points is None or destination_points > track_points:
                        destination_points = track_points
                        destination = city
        return destination, destination_points

    def select_all_routes(self, all_routes: Iterable[Route]) -> Iterable[Route]:
        all_routes: List[Route] = [*all_routes]
        selected_routes: List[Route] = []

        usage: Dict[any, int] = {}
        for route in all_routes:
            for city in route.cities:
                usage[city] = 0
                usage[city.region] = 0

        # Long routes
        selected_routes.extend(self.__select_routes( 0, all_routes, usage, lambda route: 20 <= route.points <= 21 and route.is_long and sum(city.terminus for city in route.cities) == 1))
        selected_routes.extend(self.__select_routes( 8, all_routes, usage, lambda route: 20 <= route.points <= 21 and route.is_long))
        # International medium routes
        selected_routes.extend(self.__select_routes( 2, all_routes, usage, lambda route: 13 == route.points       and route.is_international))
        selected_routes.extend(self.__select_routes( 2, all_routes, usage, lambda route: 12 == route.points       and route.is_international))
        selected_routes.extend(self.__select_routes( 2, all_routes, usage, lambda route: 11 == route.points       and route.is_international))
        selected_routes.extend(self.__select_routes( 6, all_routes, usage, lambda route: 10 == route.points       and route.is_international))
        selected_routes.extend(self.__select_routes(12, all_routes, usage, lambda route:  8 <= route.points <=  9 and route.is_international))
        # Domestic short routes
        selected_routes.extend(self.__select_routes( 6, all_routes, usage, lambda route: 8 <= route.points <= 9 and not route.is_international))
        selected_routes.extend(self.__select_routes( 6, all_routes, usage, lambda route: 7 == route.points      and not route.is_international))
        selected_routes.extend(self.__select_routes( 5, all_routes, usage, lambda route: 6 == route.points      and not route.is_international))
        selected_routes.extend(self.__select_routes( 5, all_routes, usage, lambda route: 5 == route.points      and not route.is_international))

        return selected_routes

    def __select_routes(self, count: int, all_routes: List[Route], usage: Dict[any, int], condition: Callable[[Route], bool]) -> Iterable[Route]:
        routes: List[Route] = list(filter(condition, all_routes))

        while routes and count != 0:
            if usage:
                priority_usage: int = None
                for route in routes:
                    route_usage = sum(usage[city] + usage[city.region] for city in route.cities)
                    if priority_usage is None or priority_usage > route_usage:
                        priority_usage = route_usage
                        priority_routes = [route]
                    elif route_usage == priority_usage:
                        priority_routes.append(route)
            else:
                priority_routes = routes

            route: Route = random.choice(priority_routes)
            routes.remove(route)
            all_routes.remove(route)

            for city in route.cities:
                if usage:
                    usage[city] += 1
                    usage[city.region] += 1

            count -= 1
            yield route


class ColorFactory:
    def __init__(self, map: Map):
        self._all_tracks = [*map.tracks]
        random.shuffle(self._all_tracks)
        self._all_tracks.sort(key=lambda track: track.distance, reverse=True)

        total_cars = sum(track.distance * track.width for track in self._all_tracks)
        cars_per_color = ceil(total_cars / (UNCOLORED_WEIGHT + len(ALL_COLORS)))

        self._color_count: Dict[str, int] = {}
        self._color_count[UNCOLORED] = total_cars - cars_per_color * len(ALL_COLORS)
        for color in ALL_COLORS:
            self._color_count[color] = cars_per_color

        for track in self._all_tracks:
            for color in track.colors:
                if color in self._color_count:
                    self._color_count[color] -= track.distance

    def color_all_tracks(self) -> None:
        self.__color_track_groups(lambda track: track.width > 1)
        self.__color_tracks(UNCOLORED, lambda track: track.engines > 0)
        self.__color_tracks(UNCOLORED, lambda track: track.tunnel and track.distance >= 4)
        self.__color_tracks(ANY_COLOR, lambda track: track.tunnel)
        self.__color_tracks(ANY_COLOR, lambda track: any(adjacent_track.engines > 0 for adjacent_track in track.adjacent_tracks()))
        self.__color_tracks(ANY_COLOR, lambda track: True) # Until colors run out
        self.__color_tracks(UNCOLORED, lambda track: True)

    def __color_track_groups(self, condition: Callable[[Track], bool]) -> None:
        tracks: List[Track] = [track for track in self._all_tracks if condition(track)]
        while tracks:
            track_group = self.__build_track_group(tracks, tracks[0])
            remaining_colors = [*ALL_COLORS]
            random.shuffle(remaining_colors)
            for track in track_group:
                for n in range(track.width):
                    if track.colors[n] in UNDECIDED:
                        selected_color = remaining_colors.pop(0)
                        track.colors[n] = selected_color
                        self._color_count[selected_color] -= track.distance

    def __build_track_group(self, tracks: List[Track], track: Track) -> List[Track]:
        tracks.remove(track)
        track_group = [track]
        for adjacent_track in track.adjacent_tracks():
            if adjacent_track in tracks:
                track_group.extend(self.__build_track_group(tracks, adjacent_track))
        return track_group

    def __color_tracks(self, desired_color_type: str, condition: Callable[[Track], bool]) -> None:
        desired_colors = self.__get_colors_for_type(desired_color_type)
        
        for track in self._all_tracks:
            if condition(track):
                for i in range(track.width):
                    track_color_type = track.colors[i]
                    if track_color_type in UNDECIDED:
                        valid_colors = [color for color in self.__get_colors_for_type(track_color_type) if color in desired_colors and self._color_count[color] >= track.distance]
                        if valid_colors:
                            selected_color = self.__select_color(valid_colors, track)
                            track.colors[i] = selected_color
                            self._color_count[selected_color] -= track.distance

    def __select_color(self, valid_colors: List[str], track: Track) -> str:
        valid_colors: Dict[str, int] = {valid_color: sum(valid_color == adjacent_color for adjacent_color in track.adjacent_colors()) for valid_color in valid_colors}
        best_score: int = min(valid_colors[color] for color in valid_colors)
        valid_colors: List[str] = [color for color in valid_colors if valid_colors[color] == best_score]

        if len(valid_colors) == 1:
            selected_color: str = valid_colors[0]
        else:
            weighted_colors: List[str] = []
            for color in valid_colors:
                for n in range(ceil(self._color_count[color] * (1 if color == UNCOLORED else UNCOLORED_WEIGHT))):
                    weighted_colors.append(color)
            selected_color: str = random.choice(weighted_colors)
        return selected_color

    def __get_colors_for_type(self, color_type: str) -> List[str]:
        if color_type == ANY_THING:
            return [UNCOLORED, *ALL_COLORS]
        elif color_type == ANY_COLOR:
            return ALL_COLORS
        else:
            return [color_type]


def main(filename: str) -> None:
    with open(f'{filename}.json', 'r') as file:
        data = json.load(file)
    map = Map(data)
    route_factory = RouteFactory(map)
    color_factory = ColorFactory(map)
    map.routes = list(sorted(
        route_factory.select_all_routes(route_factory.find_all_routes()),
        key=lambda route: route.points
    ))
    color_factory.color_all_tracks()
    with open(f'{filename}.out.json', 'w') as file:
        file.write(map.to_str())
    with open(f'{filename}.out.csv', 'w') as file:
        file.write('Points,CityA,RegionA,CityB,RegionB,IsLong')
        for route in map.routes:
            file.write(f'\n{route.points},"{route.cities[0].name}","{route.cities[0].region}","{route.cities[1].name}","{route.cities[1].region}",{1 if route.is_long else 0}')


if __name__ == '__main__':
    main(sys.argv[1])

    from contextlib import redirect_stdout

    import stats
    with open(sys.argv[1] + '.out.txt', 'w') as file:
        with redirect_stdout(file):
            stats.main(sys.argv[1] + '.out')
