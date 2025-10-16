import pytest
from fastapi.testclient import TestClient
from sqlmodel import Session

from ...main import API_ROUTE_PREFIX, app
from ...setup.database import create_db_and_tables, engine, get_session
from ...setup.seed import seed_data
from ...data.seed_test import get_test_data


@pytest.fixture(name="session")
def session_fixture():
    create_db_and_tables()
    connection = engine.connect()
    transaction = connection.begin()
    session = Session(bind=connection)
    
    yield session
    
    session.close()
    transaction.rollback()
    connection.close()


@pytest.fixture(name="client")
def client_fixture(session: Session):
    seed_data(get_test_data(), session)

    def get_session_override():
        return session

    app.dependency_overrides[get_session] = get_session_override

    client = TestClient(app)
    client.base_url = str(client.base_url) + API_ROUTE_PREFIX
    
    yield client

    app.dependency_overrides.clear()
