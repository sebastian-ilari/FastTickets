import uuid

def is_valid_uuid(uuid_to_test: str, version: int =4) -> bool:
    if isinstance(uuid_to_test, uuid.UUID):
        return True
    
    try:
        uuid.UUID(uuid_to_test, version=version)
    except ValueError:
        return False
    return True
