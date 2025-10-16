import unittest
from sqlmodel import Session

from ...setup.database import create_db_and_tables, engine


class DatabaseTestCase(unittest.TestCase):
    def setUp(self):
        create_db_and_tables()
        self.connection = engine.connect()
        self.transaction = self.connection.begin()
        self.session = Session(bind=self.connection)
    
    def tearDown(self):
        self.session.close()
        self.transaction.rollback()
        self.connection.close()
