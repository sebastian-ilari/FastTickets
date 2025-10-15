from fastapi import APIRouter, FastAPI
from contextlib import asynccontextmanager

from .data.setup import create_db_and_tables, seed_data_and_session
from .data.seed import get_application_data
from .routers import shows, tickets

API_ROUTE_PREFIX = "/fast-tickets"

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup code
    create_db_and_tables()
    seed_data_and_session(get_application_data())

    yield

    # Shutdown code

app = FastAPI(lifespan=lifespan)

router_prefix = APIRouter(prefix=API_ROUTE_PREFIX)

router_prefix.include_router(shows.router)
router_prefix.include_router(tickets.router)

app.include_router(router_prefix)
