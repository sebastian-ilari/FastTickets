import pytest
from fastapi.testclient import TestClient
from sqlalchemy import StaticPool
from sqlmodel import SQLModel, Session, create_engine

from ..main import app
from ..data.setup import get_session
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

    seeded_shows = get_test_data()

    session.add_all(seeded_shows)
    session.commit()

    for show in seeded_shows:
        session.refresh(show)
        for sector in show.sectors:
            session.refresh(sector)

    def get_session_override():
        return session

    app.dependency_overrides[get_session] = get_session_override  

    client = TestClient(app)  
    yield client

    app.dependency_overrides.clear()
