from fastapi import APIRouter, HTTPException
import uuid
from sqlmodel import select

from ..setup.database import SessionDep
from ..data.models import BuyTicketRequest, Sector, Show, ShowBase, ShowWithSectors, Ticket, TicketResponse
from ..services.tickets import buy_tickets as service_buy_tickets


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
        response_model=ShowBase,
        summary="Get a specific show by its ID",
        response_description="The show details",
        tags=["shows"])
async def get_show(show_id: uuid.UUID, session: SessionDep):
    """
    Gets a specific show details
    """
    show = session.exec(select(Show).where(Show.id == show_id)).one_or_none()
    if not show:
        raise HTTPException(status_code=404, detail=f"Show {show_id} not found")

    return show

@router.get(
        "/show/{show_id}/sectors", 
        response_model=list[Sector],
        summary="Get sectors for a specific show",
        response_description="A list of sectors for the specified show",
        responses={404: {"description": "Show {show_id} not found"}},
        tags=["shows"])
async def get_show_sectors(show_id: uuid.UUID, session: SessionDep):
    """
    Gets sectors for a specific show by its ID

    - **show_id**: The ID of the show to retrieve sectors for
    """
    show = session.exec(select(Show).where(Show.id == show_id)).all()
    if not show:
        raise HTTPException(status_code=404, detail=f"Show {show_id} not found")

    sectors = session.exec(select(Sector).where(Sector.show_id == show_id)).all()
    return sectors

@router.post(
        "/show/{show_id}/tickets", 
        response_model=TicketResponse,
        summary="Buy tickets for a specific show",
        response_description="The ticket bought",
        responses={
            400: {"description": "Quantity must be greater than 0"},
            400: {"description": "Not enough available spots in sector {sector.name}"},
            500: {"description": "Sector {show_id} not found"},
            500: {"description": "Show {show_id} not found"},
            },
        tags=["shows"])
async def buy_tickets(show_id: uuid.UUID, buy_ticket_request: BuyTicketRequest, session: SessionDep):
    """
    Buys tickets for a show

    - **show_id**: The ID of the show to buy tickets for

    - **sector_id**: The ID of the sector to buy tickets for
    - **quantity**: The number of tickets to buy
    """
    if buy_ticket_request.quantity <= 0:
        raise HTTPException(status_code=400, detail="Quantity must be greater than 0")

    sector = session.exec(select(Sector).where(Sector.id == buy_ticket_request.sector_id)).one_or_none()
    if not sector:
        raise HTTPException(status_code=500, detail=f"Sector {buy_ticket_request.sector_id} not found")

    show = session.exec(select(Show).where(Show.id == show_id)).all()
    if not show:
        raise HTTPException(status_code=404, detail=f"Show {show_id} not found")

    if sector.available_spots - buy_ticket_request.quantity < 0:
        raise HTTPException(status_code=400, detail=f"Not enough available spots in sector {sector.name}")

    ticket = service_buy_tickets(sector, show_id, buy_ticket_request, session)

    return Ticket.map_to_response(ticket)
