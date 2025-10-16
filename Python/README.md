# README

Python + FastApi implementation of the FastTickets API.

## Running the application

### Local setup

Commands to be run in a PowerShell terminal.

1. Create a virtual environment
```
python -m venv fast_tickets.venv
```
2. Activate the virtual environment
```
.\fast_tickets.venv\Scripts\Activate.ps1
```
3. Install necessary packages
```
pip install -r requirements.txt
```

### Running the application

To run the application locally execute this command in the Python folder:

```
fastapi dev main.py
```

The API can then be reached in [this url](http://127.0.0.1:8000/fast-tickets) and the Swagger documentation in [this url](http://127.0.0.1:8000/docs).

## Persistence

The application persistence is done in an In-memory SQLite database.

## Tests

All tests can be run with the following command:

```
pytest -v
```

All tests use an In-memory SQLite database and each test run is wrapped in a transaction that is rolled back right after the test is run, so each test will use the same seeded data.

### API

See `session_fixture` function in `conftest.py` for rollback mechanism.

### Integration

See `DatabaseTestCase` class in `base_test.py` for rollback mechanism.
