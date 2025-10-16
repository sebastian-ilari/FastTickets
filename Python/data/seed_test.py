from datetime import datetime
import uuid

from .models import Show, Sector

SHOW_1_UUID = uuid.uuid4()
SHOW_2_UUID = uuid.uuid4()
SECTOR_1_UUID = uuid.uuid4()
SECTOR_2_UUID = uuid.uuid4()
SECTOR_3_UUID = uuid.uuid4()

def get_test_data():
    return [
        Show(
            id=SHOW_1_UUID,
            artist="Test Artist 01", 
            name="Test Show 01", 
            venue="Test Venue 01", 
            date=datetime(1970, 1, 1), 
            sectors=[Sector(id=SECTOR_1_UUID, name="Test Sector 01", total_spots=500, available_spots=500)]
        ),
        Show(
            id=SHOW_2_UUID,
            artist="Test Artist 02", 
            name="Test Show 02", 
            venue="Test Venue 02", 
            date=datetime(1980, 1, 1), 
            sectors=[
                Sector(id=SECTOR_2_UUID, name="Test Sector 02", total_spots=100, available_spots=100),
                Sector(id=SECTOR_3_UUID, name="Test Sector 03", total_spots=200, available_spots=200)
            ]
        )
    ]
