from fastapi import FastAPI

from .data.setup import create_db_and_tables, seed_data_and_session
from .data.seed import get_application_data
from .routers import shows, tickets


"""
This doesn't work with integration tests
@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup code
    create_db_and_tables()
    seed_database_with_data(get_application_data())

    yield

    # Shutdown code

app = FastAPI(lifespan=lifespan)
"""

create_db_and_tables()
seed_data_and_session(get_application_data())

app = FastAPI()

app.include_router(shows.router)
app.include_router(tickets.router)
