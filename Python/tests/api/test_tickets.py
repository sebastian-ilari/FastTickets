from fastapi.testclient import TestClient
from fastapi.encoders import jsonable_encoder
import uuid

from ...data.seed_test import SHOW_1_UUID, SHOW_2_UUID, SECTOR_1_UUID, SECTOR_2_UUID, SECTOR_3_UUID
from ...tests.helpers.uuid_validator import is_valid_uuid

invalid_uuid = uuid.uuid4()


# get_tickets
def test_get_tickets_no_tickets(client: TestClient):
    response = client.get("/tickets")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert not response.json()

def test_get_tickets_one_ticket(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_2_UUID,
        "quantity": 10
    })
    client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    response = client.get("/tickets")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert len(response.json()) == 1
    ticket = response.json()[0]

    assert ticket["id"] is not None
    assert is_valid_uuid(ticket["id"])
    assert ticket["show"] == "Test Show 02"
    assert ticket["artist"] == "Test Artist 02"
    assert ticket["sector"] == "Test Sector 02"
    assert ticket["quantity"] == 10
    assert ticket["venue"] == "Test Venue 02"
    assert ticket["date"] == "1980-01-01T00:00:00"


# get_ticket_by_id
def test_get_ticket_by_id_ticket_not_found_returns_404(client: TestClient):
    response = client.get(f"/ticket/{invalid_uuid}")

    assert response.status_code == 404
    assert response.json() == {"detail": f"Ticket {invalid_uuid} not found"}

def test_get_ticket_by_id_ticket_found_returns_ticket(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_3_UUID,
        "quantity": 30
    })
    buy_response = client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    response = client.get(f"/ticket/{buy_response.json()["id"]}")

    assert response.status_code == 200
    assert response.json()["id"] is not None
    assert is_valid_uuid(response.json()["id"])
    assert response.json()["show"] == "Test Show 02"
    assert response.json()["artist"] == "Test Artist 02"
    assert response.json()["sector"] == "Test Sector 03"
    assert response.json()["quantity"] == 30
    assert response.json()["venue"] == "Test Venue 02"
    assert response.json()["date"] == "1980-01-01T00:00:00"
