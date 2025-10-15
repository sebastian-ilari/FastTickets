from fastapi import FastAPI
from fastapi.concurrency import asynccontextmanager

from ..data.seed import get_application_data
from ..data.setup import create_db_and_tables, seed_data_and_session


@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup code
    create_db_and_tables()
    seed_data_and_session(get_application_data())

    yield

    # Shutdown code
