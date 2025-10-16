from ....services.tickets import buy_tickets
from ....data.models import BuyTicketRequest, Sector
from ..base_test import DatabaseTestCase


class TestTicketsService(DatabaseTestCase):

    def test_buy_tickets_decrements_available_spots_and_returns_ticket(self):
        sector = Sector(id=1, name="Test Sector", show_id=1, available_spots=100)
        buy_ticket_request = BuyTicketRequest(sector_id=1, quantity=10)
        
        ticket = buy_tickets(sector, 1, buy_ticket_request, self.session)

        self.assertEqual(sector.available_spots, 90)
        self.assertIsNotNone(ticket)
        self.assertIsNotNone(ticket.id)
        self.assertGreater(ticket.id, 0)
        self.assertEqual(ticket.show_id, 1)
        self.assertEqual(ticket.sector_id, 1)
        self.assertEqual(ticket.quantity, 10)
