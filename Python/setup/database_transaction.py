from sqlmodel import Session

from ..setup.database import create_db_and_tables, DATABASE_ENGINE


class DatabaseTransaction:
    def create_transaction(self):
        create_db_and_tables()
        self.connection = DATABASE_ENGINE.connect()
        self.transaction = self.connection.begin()
        self.session = Session(bind=self.connection)

    def rollback_transaction(self):
        self.session.close()
        self.transaction.rollback()
        self.connection.close()
