from sqlmodel import Field, Relationship, SQLModel

from datetime import datetime


class ShowBase(SQLModel):
    artist: str = Field()
    name: str = Field()
    venue: str = Field()
    date: datetime = Field()

class Show(ShowBase, table=True):
    id: int | None = Field(default=None, primary_key=True)

    sectors: list["Sector"] = Relationship(back_populates="show")

class ShowPublic(ShowBase):
    id: int


class SectorBase(SQLModel):
    name: str = Field()
    total_spots: int = Field(default=0)
    available_spots: int = Field(default=0)

    show_id: int | None = Field(default=None, foreign_key="show.id")

class Sector(SectorBase, table=True):
    id: int | None = Field(default=None, primary_key=True)

    show: Show | None = Relationship(back_populates="sectors")

class SectorPublic(SectorBase):
    id: int


# Models to avoid circular references in responses
class ShowWithSectors(ShowPublic):
    sectors: list[SectorPublic] = []

class SectorWithShow(SectorPublic):
    show: ShowPublic | None = None
