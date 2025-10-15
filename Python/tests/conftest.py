import pytest
from fastapi.testclient import TestClient
from sqlalchemy import StaticPool
from sqlmodel import SQLModel, Session, create_engine

from ..main import API_ROUTE_PREFIX, app
from ..setup.seed import get_session, seed_data
from ..data.seed_test import get_test_data


@pytest.fixture(name="session")  
def session_fixture():  
    engine = create_engine(
        "sqlite://",
        connect_args={"check_same_thread": False},
        poolclass=StaticPool
    )
    SQLModel.metadata.create_all(engine)
    
    # Create a connection and transaction
    connection = engine.connect()
    transaction = connection.begin()
    
    # Create session bound to the transaction
    session = Session(bind=connection)
    
    yield session
    
    # Rollback transaction and close connection
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
