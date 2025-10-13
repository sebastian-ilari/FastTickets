from pydantic import BaseModel
from sqlmodel import Field, Relationship, SQLModel

from datetime import datetime

# Domain models
class ShowBase(SQLModel):
    id: int = Field()
    artist: str = Field()
    name: str = Field()
    venue: str = Field()
    date: datetime = Field()

class SectorBase(SQLModel):
    id: int = Field()
    name: str = Field()
    total_spots: int = Field(default=0)
    available_spots: int = Field(default=0)

    show_id: int | None = Field(default=None, foreign_key="show.id")

class TicketBase(SQLModel):
    id: int = Field()
    quantity: int = Field()


# Database models

class Show(ShowBase, table=True):
    id: int | None = Field(default=None, primary_key=True)

    sectors: list["Sector"] = Relationship(back_populates="show")

class Sector(SectorBase, table=True):
    id: int | None = Field(default=None, primary_key=True)

    show: Show | None = Relationship(back_populates="sectors")

class Ticket(TicketBase, table=True):
    id: int | None = Field(default=None, primary_key=True)

    show_id: int | None = Field(default=None, foreign_key="show.id")
    show: Show | None = Relationship()
    sector_id: int | None = Field(default=None, foreign_key="sector.id")
    sector: Sector | None = Relationship()

    def map_to_response(self) -> "TicketResponse":
        return TicketResponse(
            id=self.id,
            show=self.show.name if self.show else "",
            artist=self.show.artist if self.show else "",
            sector=self.sector.name if self.sector else "",
            quantity=self.quantity,
            venue=self.show.venue if self.show else "",
            date=self.show.date if self.show else datetime.min
        )

# Request models
class BuyTicketRequest(BaseModel):
    sector_id: int
    quantity: int


# Output models (designed to avoid circular references in responses)
class ShowWithSectors(ShowBase):
    sectors: list[SectorBase] = []

class SectorWithShow(SectorBase):
    show: ShowBase | None = None
    
class TicketResponse(SQLModel):
    id: int
    show: str
    artist: str
    sector: str
    quantity: int
    venue: str
    date: datetime
