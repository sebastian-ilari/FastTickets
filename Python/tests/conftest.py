import pytest
from fastapi.testclient import TestClient
from sqlalchemy import StaticPool
from sqlmodel import SQLModel, Session, create_engine

from ..data.setup import get_session, seed_data
from ..data.seed_test import shows_data
from ..main import app


@pytest.fixture(name="session")  
def session_fixture():  
    engine = create_engine(
        "sqlite://",
        connect_args={"check_same_thread": False},
        poolclass=StaticPool
    )
    SQLModel.metadata.create_all(engine)
    with Session(engine) as session:
        session.add_all(shows_data)
        session.commit()

        for show in shows_data:
            session.refresh(show)
            for sector in show.sectors:
                session.refresh(sector)   

        yield session


@pytest.fixture(name="client")  
def client_fixture(session: Session):  
    def get_session_override():  
        return session

    app.dependency_overrides[get_session] = get_session_override  

    client = TestClient(app)  
    yield client  

    app.dependency_overrides.clear()
