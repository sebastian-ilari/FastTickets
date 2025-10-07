from contextlib import asynccontextmanager
from fastapi import FastAPI

from .data.setup import create_db_and_tables, seed_data
from .routers import shows



@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup code
    create_db_and_tables()
    seed_data()

    yield

    # Shutdown code


app = FastAPI(lifespan=lifespan)

app.include_router(shows.router)
