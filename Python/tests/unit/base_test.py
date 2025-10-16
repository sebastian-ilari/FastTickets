import unittest

from ...setup.database_transaction import DatabaseTransaction


class DatabaseTestCase(unittest.TestCase):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.database_transaction = DatabaseTransaction()

    def setUp(self):
        self.database_transaction.create_transaction()
        self.session = self.database_transaction.session
    
    def tearDown(self):
        self.database_transaction.rollback_transaction()
