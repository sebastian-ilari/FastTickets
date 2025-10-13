from datetime import datetime

from .models import Show, Sector


def get_application_data():
    return [
        Show(artist="Pink Floyd", name="Live at Pompeii", venue="Ancient Roman amphitheatre - Pompeii", date=datetime(1971, 10, 1), sectors=[
            Sector(name="Crew", total_spots=100, available_spots=100)
        ]),
        Show(artist="David Bowie", name="Ziggy Stardust tour", venue="Borough Assembly Hall", date=datetime(1972, 1, 29), sectors=[
            Sector(name="Standing", total_spots=5000, available_spots=5000),
            Sector(name="Left side seating", total_spots=1000, available_spots=1000),
            Sector(name="Right side seating", total_spots=1000, available_spots=1000),
            Sector(name="Back seating", total_spots=2000, available_spots=2000)
        ]),
        Show(artist="Queen", name="Live Aid", venue="Wembley Stadium", date=datetime(1985, 7, 13), sectors=[
            Sector(name="Standing", total_spots=200000, available_spots=200000),
            Sector(name="First level seating", total_spots=3000, available_spots=3000),
            Sector(name="Second level seating", total_spots=15000, available_spots=15000)
        ]),
        Show(artist="U2", name="Zoo TV", venue="Ireland RDS Arena", date=datetime(1993, 8, 28), sectors=[
            Sector(name="General", total_spots=7000, available_spots=7000),
            Sector(name="VIP", total_spots=4000, available_spots=4000)
        ])
    ]
