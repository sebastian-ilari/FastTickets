from sqlmodel import Session

from ..setup.database import create_db_and_tables, get_engine


class DatabaseTransaction:
    def create_transaction(self):
        self.engine = get_engine()
        create_db_and_tables(self.engine)
        self.connection = self.engine.connect()
        self.transaction = self.connection.begin()
        self.session = Session(bind=self.connection)

    def rollback_transaction(self):
        self.session.close()
        self.transaction.rollback()
        self.connection.close()
