from fastapi import APIRouter, HTTPException
from sqlmodel import select

from ..models import Ticket, TicketResponse
from ..data.setup import SessionDep


router = APIRouter()

@router.get(
        "/tickets/", 
        response_model=list[TicketResponse],
        summary="Get all the tickets sold",
        response_description="A list of tickets sold",
        tags=["tickets"])
async def get_tickets(session: SessionDep):
    """
    Gets all the tickets sold
    """
    tickets = session.exec(select(Ticket)).all()
    return [ticket.map_to_response() for ticket in tickets]

@router.get(
        "/tickets/{ticket_id}", 
        response_model=TicketResponse,
        summary="Get a specific ticket by its ID",
        response_description="The ticket",
        responses={404: {"description": "Ticket {ticket_id} not found"}},
        tags=["tickets"])
async def get_ticket_by_id(ticket_id: int, session: SessionDep):
    """
    Gets a specific ticket by its ID

    - **ticket_id**: The ID of the ticket to retrieve
    """
    ticket = session.exec(select(Ticket).where(Ticket.id == ticket_id)).one_or_none()
    if not ticket:
        raise HTTPException(status_code=404, detail=f"Ticket {ticket_id} not found")

    return Ticket.map_to_response(ticket)
