from datetime import datetime

from .models import Show, Sector


def get_test_data():
    return [
        Show(
            artist="Test Artist 01", 
            name="Test Show 01", 
            venue="Test Venue 01", 
            date=datetime(1970, 1, 1), 
            sectors=[Sector(name="Test Sector 01", total_spots=500, available_spots=500)]
        ),
        Show(
            artist="Test Artist 02", 
            name="Test Show 02", 
            venue="Test Venue 02", 
            date=datetime(1980, 1, 1), 
            sectors=[
                Sector(name="Test Sector 02", total_spots=100, available_spots=100),
                Sector(name="Test Sector 03", total_spots=200, available_spots=200)
            ]
        ),
    ]
