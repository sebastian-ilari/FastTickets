from fastapi.testclient import TestClient
from fastapi.encoders import jsonable_encoder
import uuid

from ...data.seed_test import SHOW_1_UUID, SHOW_2_UUID, SECTOR_1_UUID, SECTOR_2_UUID, SECTOR_3_UUID

invalid_uuid = uuid.uuid4()


# get_shows
def test_get_shows(client: TestClient):
    response = client.get("/shows")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert len(response.json()) == 2

    first_show = response.json()[0]
    assert first_show["id"] == str(SHOW_1_UUID)
    assert first_show["artist"] == "Test Artist 01"
    assert first_show["name"] == "Test Show 01"
    assert first_show["venue"] == "Test Venue 01"
    assert first_show["date"] == "1970-01-01T00:00:00"
    assert len(first_show["sectors"]) == 1
    assert first_show["sectors"][0]["id"] == str(SECTOR_1_UUID)
    assert first_show["sectors"][0]["name"] == "Test Sector 01"
    assert first_show["sectors"][0]["total_spots"] == 500
    assert first_show["sectors"][0]["available_spots"] == 500
    assert first_show["sectors"][0]["show_id"] == first_show["id"]

    second_show = response.json()[1]
    assert second_show["id"] == str(SHOW_2_UUID)
    assert second_show["artist"] == "Test Artist 02"
    assert second_show["name"] == "Test Show 02"
    assert second_show["venue"] == "Test Venue 02"
    assert second_show["date"] == "1980-01-01T00:00:00"
    assert len(second_show["sectors"]) == 2
    assert second_show["sectors"][0]["id"] == str(SECTOR_2_UUID)
    assert second_show["sectors"][0]["name"] == "Test Sector 02"
    assert second_show["sectors"][0]["total_spots"] == 100
    assert second_show["sectors"][0]["available_spots"] == 100
    assert second_show["sectors"][0]["show_id"] == second_show["id"]
    assert second_show["sectors"][1]["id"] == str(SECTOR_3_UUID)
    assert second_show["sectors"][1]["name"] == "Test Sector 03"
    assert second_show["sectors"][1]["total_spots"] == 200
    assert second_show["sectors"][1]["available_spots"] == 200
    assert second_show["sectors"][1]["show_id"] == second_show["id"]


# get_show
def test_get_show_show_not_found_returns_404(client: TestClient):
    response = client.get(f"/show/{invalid_uuid}")

    assert response.status_code == 404
    assert response.json() == {"detail": f"Show {invalid_uuid} not found"}

def test_get_show_show_found_returns_show_without_sectors(client: TestClient):
    response = client.get(f"/show/{SHOW_2_UUID}")

    assert response.status_code == 200
    assert response.json()["id"] == str(SHOW_2_UUID)
    assert response.json()["artist"] == "Test Artist 02"
    assert response.json()["name"] == "Test Show 02"
    assert response.json()["venue"] == "Test Venue 02"
    assert response.json()["date"] == "1980-01-01T00:00:00"
    assert response.json().get("sectors") is None


# get_show_sectors
def test_get_show_by_id_invalid_id_returns_404(client: TestClient):
    response = client.get(f"/show/{invalid_uuid}/sectors")

    assert response.status_code == 404
    assert response.json() == {"detail": f"Show {invalid_uuid} not found"}

def test_get_show_by_id_returns_sectors(client: TestClient):
    response = client.get(f"/show/{SHOW_2_UUID}/sectors")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert len(response.json()) == 2

    first_sector = response.json()[0]
    assert first_sector["id"] == str(SECTOR_2_UUID)
    assert first_sector["name"] == "Test Sector 02"
    assert first_sector["total_spots"] == 100
    assert first_sector["available_spots"] == 100
    assert first_sector["show_id"] == str(SHOW_2_UUID)

    second_sector = response.json()[1]
    assert second_sector["id"] == str(SECTOR_3_UUID)
    assert second_sector["name"] == "Test Sector 03"
    assert second_sector["total_spots"] == 200
    assert second_sector["available_spots"] == 200
    assert second_sector["show_id"] == str(SHOW_2_UUID)


# buy_tickets
def test_buy_tickets_0_quantity_returns_400(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_2_UUID,
        "quantity": 0
    })
    response = client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    assert response.status_code == 400
    assert response.json() == {"detail": "Quantity must be greater than 0"}

def test_buy_tickets_negative_quantity_returns_400(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_2_UUID,
        "quantity": -5
    })
    response = client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    assert response.status_code == 400
    assert response.json() == {"detail": "Quantity must be greater than 0"}

def test_buy_tickets_sector_does_not_exist_returns_500(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": invalid_uuid,
        "quantity": 1
    })
    response = client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    assert response.status_code == 500
    assert response.json() == {"detail": f"Sector {invalid_uuid} not found"}

def test_buy_tickets_show_does_not_exist_returns_500(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_2_UUID,
        "quantity": 1
    })
    response = client.post(f"/show/{invalid_uuid}/tickets", json=buy_ticket_payload)

    assert response.status_code == 404
    assert response.json() == {"detail": f"Show {invalid_uuid} not found"}

def test_buy_tickets_not_enough_available_tickets_returns_400(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_2_UUID,
        "quantity": 200
    })
    response = client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    assert response.status_code == 400
    assert response.json() == {"detail": "Not enough available spots in sector Test Sector 02"}

def test_buy_tickets_not_enough_available_tickets_show_is_not_updated(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_2_UUID,
        "quantity": 200
    })
    response = client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    assert response.status_code == 400

    response = client.get(f"/show/{SHOW_2_UUID}/sectors")

    assert response.status_code == 200

    first_sector = response.json()[0]
    assert first_sector["id"] == str(SECTOR_2_UUID)
    assert first_sector["available_spots"] == 100

def test_buy_tickets_returns_ticket(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_2_UUID,
        "quantity": 10
    })
    response = client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    assert response.status_code == 200
    assert response.json()["id"] is not None
    assert response.json()["show"] == "Test Show 02"
    assert response.json()["artist"] == "Test Artist 02"
    assert response.json()["sector"] == "Test Sector 02"
    assert response.json()["venue"] == "Test Venue 02"
    assert response.json()["quantity"] == 10
    assert response.json()["date"] == "1980-01-01T00:00:00"

def test_buy_tickets_updates_available_tickets(client: TestClient):
    buy_ticket_payload = jsonable_encoder({
        "sector_id": SECTOR_2_UUID,
        "quantity": 10
    })
    client.post(f"/show/{SHOW_2_UUID}/tickets", json=buy_ticket_payload)

    response = client.get(f"/show/{SHOW_2_UUID}/sectors")

    assert response.status_code == 200

    first_sector = response.json()[0]
    assert first_sector["id"] == str(SECTOR_2_UUID)
    assert first_sector["available_spots"] == 90
