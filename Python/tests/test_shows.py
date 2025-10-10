from fastapi.testclient import TestClient


def test_get_shows(client: TestClient):
    response = client.get("/shows")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert len(response.json()) == 2

    first_show = response.json()[0]
    assert first_show["id"] is not None
    assert first_show["artist"] == "Test Artist 01"
    assert first_show["name"] == "Test Show 01"
    assert first_show["venue"] == "Test Venue 01"
    assert first_show["date"] == "1970-01-01T00:00:00"
    assert len(first_show["sectors"]) == 1
    assert first_show["sectors"][0]["name"] == "Test Sector 01"
    assert first_show["sectors"][0]["total_spots"] == 500
    assert first_show["sectors"][0]["available_spots"] == 500
    assert first_show["sectors"][0]["show_id"] == first_show["id"]
    assert first_show["sectors"][0]["id"] is not None

    second_show = response.json()[1]
    assert second_show["id"] is not None
    assert second_show["artist"] == "Test Artist 02"
    assert second_show["name"] == "Test Show 02"
    assert second_show["venue"] == "Test Venue 02"
    assert second_show["date"] == "1980-01-01T00:00:00"
    assert len(second_show["sectors"]) == 2
    assert second_show["sectors"][0]["id"] is not None
    assert second_show["sectors"][0]["name"] == "Test Sector 02"
    assert second_show["sectors"][0]["total_spots"] == 100
    assert second_show["sectors"][0]["available_spots"] == 100
    assert second_show["sectors"][0]["show_id"] == second_show["id"]
    assert second_show["sectors"][1]["id"] is not None
    assert second_show["sectors"][1]["name"] == "Test Sector 03"
    assert second_show["sectors"][1]["total_spots"] == 200
    assert second_show["sectors"][1]["available_spots"] == 200
    assert second_show["sectors"][1]["show_id"] == second_show["id"]


def test_get_show_by_id_invalid_id_returns_404(client: TestClient):
    response = client.get("/show/999")

    assert response.status_code == 404
    assert response.json() == {"detail": "Show 999 not found"}

def test_get_show_by_id_returns_sectors(client: TestClient):
    response = client.get("/show/2")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert len(response.json()) == 2

    first_sector = response.json()[0]
    assert first_sector["id"] is not None
    assert first_sector["name"] == "Test Sector 02"
    assert first_sector["total_spots"] == 100
    assert first_sector["available_spots"] == 100
    assert first_sector["show_id"] == 2

    second_sector = response.json()[1]
    assert second_sector["id"] is not None
    assert second_sector["name"] == "Test Sector 03"
    assert second_sector["total_spots"] == 200
    assert second_sector["available_spots"] == 200
    assert second_sector["show_id"] == 2

def test_buy_tickets_success(client: TestClient):
    buy_ticket_payload = {
        "sector_id": 2,
        "quantity": 10
    }
    response = client.post("/show/2/tickets", json=buy_ticket_payload)

    assert response.status_code == 200
    assert response.json()["id"] is not None
    assert response.json()["show"] == "Test Show 02"
    assert response.json()["artist"] == "Test Artist 02"
    assert response.json()["sector"] == "Test Sector 02"
    assert response.json()["venue"] == "Test Venue 02"
    assert response.json()["quantity"] == 10
    assert response.json()["date"] == "1980-01-01T00:00:00"
