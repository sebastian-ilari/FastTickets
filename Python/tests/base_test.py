import unittest
from sqlmodel import Session

from ..setup.database import engine, create_db_and_tables


class DatabaseTestCase(unittest.TestCase):
    def setUp(self):
        create_db_and_tables()
        self.session = Session(engine)
    
    def tearDown(self):
        self.session.close()
