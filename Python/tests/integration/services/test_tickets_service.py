import uuid

from ....services.tickets import buy_tickets
from ....data.models import BuyTicketRequest, Sector
from ..base_test import DatabaseTestCase


class TestTicketsService(DatabaseTestCase):

    def test_buy_tickets_decrements_available_spots_and_returns_ticket(self):
        show_id = uuid.uuid4()
        sector_id = uuid.uuid4()
        
        sector = Sector(id=sector_id, name="Test Sector", show_id=show_id, available_spots=100)
        buy_ticket_request = BuyTicketRequest(sector_id=sector_id, quantity=10)
        
        ticket = buy_tickets(sector, show_id, buy_ticket_request, self.session)

        self.assertEqual(sector.available_spots, 90)
        self.assertIsNotNone(ticket)
        self.assertIsNotNone(ticket.id)
        self.assertEqual(ticket.show_id, show_id)
        self.assertEqual(ticket.sector_id, sector_id)
        self.assertEqual(ticket.quantity, 10)
