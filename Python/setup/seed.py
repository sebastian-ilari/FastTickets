from fastapi import FastAPI
from fastapi.concurrency import asynccontextmanager
from sqlmodel import Session

from ..setup.database import create_db_and_tables, get_engine
from ..data.models import Show
from ..data.seed import get_application_data


def seed_data(shows: list[Show], session: Session):
    session.add_all(shows)
    session.commit()

def seed_data_and_session(shows: list[Show]):
    with Session(get_engine()) as session:
        seed_data(shows, session)


@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup code
    create_db_and_tables()
    seed_data_and_session(get_application_data())

    yield

    # Shutdown code
