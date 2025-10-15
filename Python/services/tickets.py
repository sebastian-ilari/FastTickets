from pytest import Session

from ..data.models import BuyTicketRequest, Sector, Ticket


def buy_tickets(sector: Sector, show_id: int, buy_ticket_request: BuyTicketRequest, session: Session) -> Ticket:
    sector.available_spots -= buy_ticket_request.quantity
    session.add(sector)
    session.commit()
    session.refresh(sector)

    ticket = Ticket(show_id=show_id, sector_id=buy_ticket_request.sector_id, quantity=buy_ticket_request.quantity)
    session.add(ticket)
    session.commit()
    session.refresh(ticket)

    return ticket
