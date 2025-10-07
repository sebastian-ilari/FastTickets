from fastapi import APIRouter
from ..data.seed import shows


router = APIRouter()

@router.get("/shows/", tags=["shows"])
async def get_shows():
    return shows
