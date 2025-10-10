import pytest
from datetime import datetime
from fastapi.testclient import TestClient
from sqlalchemy import StaticPool, text
from sqlmodel import SQLModel, Session, create_engine

from ..main import app
from ..models import Sector, Show
from ..data.setup import get_session


@pytest.fixture(name="session")  
def session_fixture():  
    engine = create_engine(
        "sqlite://",
        connect_args={"check_same_thread": False},
        poolclass=StaticPool
    )
    SQLModel.metadata.create_all(engine)
    with Session(engine) as session:
        yield session


@pytest.fixture(name="client")  
def client_fixture(session: Session):  
    # Clean database before adding test data
    _cleanup_database(session)

    fresh_shows_data = [
        Show(artist="Test Artist 01", name="Test Show 01", venue="Test Venue 01", date=datetime(1970, 1, 1), sectors=[
            Sector(name="Test Sector 01", total_spots=500, available_spots=500)
        ]),
        Show(artist="Test Artist 02", name="Test Show 02", venue="Test Venue 02", date=datetime(1980, 1, 1), sectors=[
            Sector(name="Test Sector 02", total_spots=100, available_spots=100),
            Sector(name="Test Sector 03", total_spots=200, available_spots=200)
        ]),
    ]

    session.add_all(fresh_shows_data)
    session.commit()

    for show in fresh_shows_data:
        session.refresh(show)
        for sector in show.sectors:
            session.refresh(sector)   

    def get_session_override():
        return session

    app.dependency_overrides[get_session] = get_session_override  

    client = TestClient(app)  
    yield client

    # Clean up database after test
    _cleanup_database(session)

    app.dependency_overrides.clear()


def _cleanup_database(session: Session):
    """Clean up database by deleting all data from tables in correct order"""
    try:
        # Delete in reverse dependency order to avoid foreign key constraints
        session.exec(text("DELETE FROM ticket"))
        session.exec(text("DELETE FROM sector")) 
        session.exec(text("DELETE FROM show"))
        session.commit()
        
        # Reset SQLite sequence counters (only if table exists)
        result = session.exec(text("SELECT name FROM sqlite_master WHERE type='table' AND name='sqlite_sequence'"))
        if result.fetchone():
            session.exec(text("DELETE FROM sqlite_sequence"))
            session.commit()

    except Exception as e:
        print(f"Error during database cleanup: {e}")
        session.rollback()
