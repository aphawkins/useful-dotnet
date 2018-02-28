Feature: CaesarCipher
	In order to encrypt messages
	As a cryptographer
	I want to use the Caesar Shift Cipher

Scenario: CaesarCipher - Ciphername
	Given I have a 'Caesar' cipher
	Then the cipher name should be 'Caesar'

Scenario Outline: CaesarCipher - Encrypt a string
	Given I have a 'Caesar' cipher
	And my plaintext is <plaintext>
	And my Caesar right shift is <rightshift>
	When I encrypt
	Then the ciphertext should be <ciphertext>
	Examples:
	| plaintext                  | rightshift | ciphertext                 |
	| ABCDEFGHIJKLMNOPQRSTUVWXYZ | 0          | ABCDEFGHIJKLMNOPQRSTUVWXYZ |
	| ABCDEFGHIJKLMNOPQRSTUVWXYZ | 3          | DEFGHIJKLMNOPQRSTUVWXYZABC |
	| abcdefghijklmnopqrstuvwxyz | 3          | defghijklmnopqrstuvwxyzabc |
	| >?@ [\]                    | 3          | >?@ [\]                    |

Scenario Outline: CaesarCipher - Decrypt a string
	Given I have a 'Caesar' cipher
	And my ciphertext is <ciphertext>
	And my Caesar right shift is <rightshift>
	When I decrypt
	Then the plaintext should be <plaintext>
	Examples:
	| plaintext                  | rightshift | ciphertext                 |
	| ABCDEFGHIJKLMNOPQRSTUVWXYZ | 0          | ABCDEFGHIJKLMNOPQRSTUVWXYZ |
	| ABCDEFGHIJKLMNOPQRSTUVWXYZ | 3          | DEFGHIJKLMNOPQRSTUVWXYZABC |
	| abcdefghijklmnopqrstuvwxyz | 3          | defghijklmnopqrstuvwxyzabc |
	| >?@ [\]                    | 3          | >?@ [\]                    |