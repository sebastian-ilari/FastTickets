from sqlmodel import Field, Relationship, SQLModel

from datetime import datetime


class ShowBase(SQLModel):
    id: int = Field()
    artist: str = Field()
    name: str = Field()
    venue: str = Field()
    date: datetime = Field()

class Show(ShowBase, table=True):
    id: int | None = Field(default=None, primary_key=True)

    sectors: list["Sector"] = Relationship(back_populates="show")

class SectorBase(SQLModel):
    id: int = Field()
    name: str = Field()
    total_spots: int = Field(default=0)
    available_spots: int = Field(default=0)

    show_id: int | None = Field(default=None, foreign_key="show.id")

class Sector(SectorBase, table=True):
    id: int | None = Field(default=None, primary_key=True)

    show: Show | None = Relationship(back_populates="sectors")

# Models to avoid circular references in responses
class ShowWithSectors(ShowBase):
    sectors: list[SectorBase] = []

class SectorWithShow(SectorBase):
    show: ShowBase | None = None
