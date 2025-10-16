import unittest
from sqlmodel import SQLModel, Session

from ...setup.database import engine


class DatabaseTestCase(unittest.TestCase):
    def setUp(self):
        # TODO: Can I use create_db_and_tables and make it return the engine?
        SQLModel.metadata.create_all(engine)
        
        # Create a connection and transaction
        self.connection = engine.connect()
        self.transaction = self.connection.begin()
        
        # Create session bound to the transaction
        self.session = Session(bind=self.connection)
    
    def tearDown(self):
        # Rollback transaction and close connection
        self.session.close()
        self.transaction.rollback()
        self.connection.close()
