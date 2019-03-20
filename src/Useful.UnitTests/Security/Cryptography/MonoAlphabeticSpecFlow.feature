Feature: MonoAlphabeticSpecFlow
	In order to use the Monoalphabetic Substitution cipher
	As a cryptographer
	I want to test encryption and decryption

@mytag
Scenario: Test the Monoalphabetic Substitution cipher
	Given I am testing Monoalphabetic:
	| TransformMode | Key                                           | IV | Plaintext  | Ciphertext | NewIV | Notes                       |
	| Encrypt       | ABC\|AB\|True                                 |    | ABC        | BAC        |       | Basic Symmetric Encryption  |
	| Encrypt       | ABCD\|AB CD\|True                             |    | ABCD       | BADC       |       | Multiple Substitution pairs |
	| Encrypt       | ABC\|AB BC CA\|False                          |    | ABC        | BCA        |       | Basic Asymmetric Encryption |
	| Encrypt       | ABC\|\|True                                   |    | ABC        | ABC        |       | No Substitutions            |
	| Encrypt       | ABCD\|\|True                                  |    | ABÅCD      | ABÅCD      |       | Disallowed chars            |
	| Decrypt       | ABCD\|AB CD\|True                             |    | ABCD       | BADC       |       | Basic Symmetric Decryption  |
	| Decrypt       | ABC\|AB BC CA\|False                          |    | ABC        | BCA        |       | Basic Asymmetric Decryption |
	| Encrypt       | ABCD\|AB CD\|True                             |    | AB CD      | BA DC      |       | Whitespace                  |
	| Encrypt       | ABCDEFGHIJKLMNOPQRSTUVWXYZ\|AB CD EF GH\|True |    | HeLlOwOrLd | GeLlOwOrLd |       | Mixed case                  |
	| Decrypt       | ABCDEFGHIJKLMNOPQRSTUVWXYZ\|AB CD EF GH\|True |    | HeLlOwOrLd | GeLlOwOrLd |       | Mixed case                  |
