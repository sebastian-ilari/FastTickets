from pydantic import BaseModel
from datetime import datetime


class Sector(BaseModel):
    id: int = 0
    show_id: int = 0
    name: str
    total_spots: int = 0
    available_spots: int = 0

class Show(BaseModel):
    id: int = 0
    artist: str
    name: str
    venue: str
    date: datetime
    sectors: list[Sector] = []
