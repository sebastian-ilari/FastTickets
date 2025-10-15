from fastapi import APIRouter, FastAPI, HTTPException, Request
from fastapi.exception_handlers import http_exception_handler

from .setup.logger import logger
from .setup.seed import lifespan
from .routers import shows, tickets

API_ROUTE_PREFIX = "/fast-tickets"

app = FastAPI(lifespan=lifespan)

router_prefix = APIRouter(prefix=API_ROUTE_PREFIX)

router_prefix.include_router(shows.router)
router_prefix.include_router(tickets.router)

app.include_router(router_prefix)


@app.exception_handler(HTTPException)
async def exception_handler(request: Request, exception: HTTPException):
    logger.error(f"{request.method} {request.url.path} - {exception}")                   

    return await http_exception_handler(request, exception)
