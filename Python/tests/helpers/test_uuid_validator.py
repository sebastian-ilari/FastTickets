import unittest
import uuid

from ...tests.helpers.uuid_validator import is_valid_uuid


class TestUUIDValidator(unittest.TestCase):

    def test_is_valid_uuid_string_returns_false(self):
        assert not is_valid_uuid("invalid-uuid-string")

    def test_is_valid_uuid_valid_uuid_object_returns_true(self):
        assert is_valid_uuid(uuid.uuid1())
        assert is_valid_uuid(uuid.uuid4())

    def test_is_valid_uuid_valid_string_uuid_returns_true(self):
        valid_uuid = str(uuid.uuid4())
        assert is_valid_uuid(valid_uuid)

    def test_is_valid_uuid_valid_uuid_and_specific_version_returns_true(self):
        valid_uuid = str(uuid.uuid1())
        assert is_valid_uuid(valid_uuid, 1)
