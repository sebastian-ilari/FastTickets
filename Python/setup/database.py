from fastapi import Depends
from sqlmodel import create_engine, SQLModel, Session
from sqlmodel.pool import StaticPool
from typing import Annotated


"""
# File database
sqlite_file_name = "database.db"
sqlite_url = f"sqlite:///{sqlite_file_name}"

connect_args = {"check_same_thread": False}
engine = create_engine(sqlite_url, connect_args=connect_args)
"""

# In-memory database
DATABASE_ENGINE = create_engine(
    "sqlite://",
    connect_args={"check_same_thread": False},
    poolclass=StaticPool,
)

def create_db_and_tables():
    SQLModel.metadata.create_all(DATABASE_ENGINE)

def get_session():
    with Session(DATABASE_ENGINE) as session:
        yield session

SessionDep = Annotated[Session, Depends(get_session)]
