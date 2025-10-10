from fastapi.testclient import TestClient


def test_get_tickets(client: TestClient):
    response = client.get("/tickets")

    assert response.status_code == 200
    assert isinstance(response.json(), list)
    assert not response.json()
