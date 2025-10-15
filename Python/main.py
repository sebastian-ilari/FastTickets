from fastapi import APIRouter, FastAPI, HTTPException, Request
from fastapi.exception_handlers import http_exception_handler
from contextlib import asynccontextmanager
import logging

from .data.setup import create_db_and_tables, seed_data_and_session
from .data.seed import get_application_data
from .routers import shows, tickets

API_ROUTE_PREFIX = "/fast-tickets"


# Set up logging
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(levelname)s - %(message)s",
    datefmt="%Y-%m-%d %H:%M:%S",
)

logger = logging.getLogger()


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


@app.exception_handler(HTTPException)
async def exception_handler(request: Request, exception: HTTPException):
    logger.error(f"{request.method} {request.url.path} - {exception}")                   

    return await http_exception_handler(request, exception)
