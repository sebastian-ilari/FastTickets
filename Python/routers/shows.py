from fastapi import APIRouter
from sqlmodel import select

from ..models import Show, ShowWithSectors
from ..data.setup import SessionDep


router = APIRouter()

@router.get("/shows/", tags=["shows"])
async def get_shows(session: SessionDep) -> list[ShowWithSectors]:
    shows = session.exec(select(Show)).all()
    return shows
