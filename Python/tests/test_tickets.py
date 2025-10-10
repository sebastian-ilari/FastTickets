from fastapi.testclient import TestClient


# get_tickets
def test_get_tickets_no_tickets(client: TestClient):
    response = client.get("/tickets")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert not response.json()

def test_get_tickets_one_ticket(client: TestClient):
    buy_ticket_payload = {
        "sector_id": 2,
        "quantity": 10
    }
    client.post("/show/2/tickets", json=buy_ticket_payload)

    response = client.get("/tickets")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert len(response.json()) == 1
    ticket = response.json()[0]

    assert ticket["id"] is not None
    assert ticket["show"] == "Test Show 02"
    assert ticket["artist"] == "Test Artist 02"
    assert ticket["sector"] == "Test Sector 02"
    assert ticket["quantity"] == 10
    assert ticket["venue"] == "Test Venue 02"
    assert ticket["date"] == "1980-01-01T00:00:00"


# get_ticket_by_id
def test_get_ticket_by_id_ticket_not_found_returns_404(client: TestClient):
    response = client.get("/tickets/999")

    assert response.status_code == 404
    assert response.json() == {"detail": "Ticket 999 not found"}

def test_get_ticket_by_id_ticket_found_returns_ticket(client: TestClient):
    buy_ticket_payload = {
        "sector_id": 3,
        "quantity": 30
    }
    buy_response = client.post("/show/2/tickets", json=buy_ticket_payload)

    response = client.get(f"/tickets/{buy_response.json()["id"]}")

    assert response.status_code == 200
    assert response.json()["id"] is not None
    assert response.json()["show"] == "Test Show 02"
    assert response.json()["artist"] == "Test Artist 02"
    assert response.json()["sector"] == "Test Sector 03"
    assert response.json()["quantity"] == 30
    assert response.json()["venue"] == "Test Venue 02"
    assert response.json()["date"] == "1980-01-01T00:00:00"
