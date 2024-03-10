import json
import sys


def main(filename):
    with open(f'{filename}.json', 'r') as file:
        data = json.load(file)

    cities = data.get('cities', [])
    tracks = data.get('tracks', [])
    routes = data.get('routes', [])

    city_count = len(cities)
    track_count = len(tracks)
    car_count = sum(track['distance'] * len(track['colors']) for track in tracks)
    route_count = len(routes)

    print('Cities:       ' + str(city_count))
    print('Tracks/City:  ' + str(2 * track_count / city_count))
    print()
    print('Tracks:       ' + str(track_count))
    print('Cars:         ' + str(car_count))
    print('Cars/Track:   ' + str(car_count / sum(len(track['colors']) for track in tracks)))
    for n in (1,2,3,4,5,6,7,8):
        score = sum(track['distance'] == n for track in tracks)
        if score > 0:
            print(f'{n}-tracks:     ' + str(score / track_count * 100) + '%')
    for key,name in (('a','any'),('w','white'),('p','pink'),('r','red'),('o','orange'),('y','yellow'),('g','green'),('b','blue'),('k','black'),('?','???')):
        print(f'{name:6} cars:  ' + str(sum(track['distance'] * sum(color == key for color in track['colors']) for track in tracks)))
    print(f'engine cars:  ' + str(sum(track.get('engines',0) * len(track['colors']) for track in tracks) / car_count * 100) + '%')
    print('Twin-tracks:  ' + str(sum(len(track['colors']) == 2 for track in tracks) / track_count * 100) + '%')
    print('Twin-cars:    ' + str(sum(track['distance'] for track in tracks if len(track['colors']) == 2)))
    print('Tunnels:      ' + str(sum(track.get('tunnel', False) for track in tracks) / track_count * 100) + '%')
    print()
    print('Routes:       ' + str(route_count))
    print('Points/Route: ' + str(sum(route['points'] for route in routes) / route_count))
    for n in (5,6,7,8,9,10,11,12,13,20,21):
        print(f'{n:2} points:    ' + str(sum(route['points'] == n for route in routes)))


if __name__ == '__main__':
    main(sys.argv[1])
