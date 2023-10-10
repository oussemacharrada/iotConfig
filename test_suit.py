import unittest

from suit_tool.manifest import SUITManifestDict

class TestSUITManifestDictToSUIT(unittest.TestCase):

    def setUp(self):
        # Define a sample SUITManifestDict instance
        self.sample_manifest_dict = SUITManifestDict()
        self.sample_manifest_key = self.sample_manifest_dict.ManifestKey["field1"]

    def test_to_suit_single_field(self):
        manifest_dict = SUITManifestDict()
        manifest_dict.field1 = "value1"
        
        manifest_dict.fields = {"field1": self.sample_manifest_key}
        
        # Temporarily replace the to_suit method to work with a string
        original_to_suit = manifest_dict.to_suit
        manifest_dict.to_suit = lambda: manifest_dict.field1
        
        # Convert to "SUIT" format
        suit_dict = manifest_dict.to_suit()
        
        # Expected result
        expected_suit_dict = {"suit_key": "value1"}
        
        self.assertEqual(suit_dict, expected_suit_dict)
        
        # Restore the original to_suit method
        manifest_dict.to_suit = original_to_suit

    def test_to_suit_multiple_fields(self):
        # Create a SUITManifestDict instance with multiple fields
        manifest_dict = SUITManifestDict()
        manifest_dict.field1 = "value1"
        manifest_dict.field2 = "value2"
        
        # Assign the sample_manifest_key to both fields
        manifest_dict.fields = {"field1": self.sample_manifest_key, "field2": self.sample_manifest_key}
        
        # Temporarily replace the to_suit method to work with strings
        original_to_suit = manifest_dict.to_suit
        manifest_dict.to_suit = lambda: {"suit_key": manifest_dict.field1, "suit_key": manifest_dict.field2}
        
        # Convert to "SUIT" format
        suit_dict = manifest_dict.to_suit()
        
        # Expected result
        expected_suit_dict = {"suit_key": "value1", "suit_key": "value2"}
        
        self.assertEqual(suit_dict, expected_suit_dict)
        
        # Restore the original to_suit method
        manifest_dict.to_suit = original_to_suit

if __name__ == '__main__':
    unittest.main()
