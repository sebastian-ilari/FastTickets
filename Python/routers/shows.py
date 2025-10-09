from fastapi import APIRouter, HTTPException
from sqlmodel import select

from ..models import Sector, Show, ShowWithSectors
from ..data.setup import SessionDep


router = APIRouter()

@router.get(
        "/shows/", 
        response_model=list[ShowWithSectors],
        summary="Get all shows with their sectors",
        response_description="A list of shows with their sectors",
        tags=["shows"])
async def get_shows(session: SessionDep):
    """
    Gets all shows with their sectors
    """
    shows = session.exec(select(Show)).all()
    return shows

@router.get(
        "/show/{show_id}", 
        response_model=list[Sector],
        summary="Get sectors for a specific show",
        response_description="A list of sectors for the specified show",
        responses={404: {"description": "Show not found"}},
        tags=["shows"])
async def get_show_sectors(show_id: int, session: SessionDep):
    """
    Gets sectors for a specific show by its ID

    - **show_id**: The ID of the show to retrieve sectors for
    """
    show = session.exec(select(Show).where(Show.id == show_id)).all()
    if not show:
        raise HTTPException(status_code=404, detail="Show not found")

    sectors = session.exec(select(Sector).where(Sector.show_id == show_id)).all()
    return sectors
