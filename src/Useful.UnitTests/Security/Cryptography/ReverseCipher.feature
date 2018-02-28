Feature: ReverseCipher
	In order to encrypt messages
	As a cryptographer
	I want to use the Reverse Cipher

Scenario: ReverseCipher - Ciphername
	Given I have a 'Reverse' cipher
	Then the cipher name should be 'Reverse'

Scenario Outline: ReverseCipher - Encrypt a string
	Given I have a 'Reverse' cipher
	And my plaintext is <plaintext>
	When I encrypt
	Then the ciphertext should be <ciphertext>
	Examples:
	| plaintext                  | ciphertext                 |
	| ABCDEFGHIJKLMNOPQRSTUVWXYZ | ZYXWVUTSRQPONMLKJIHGFEDCBA |
	| abcdefghijklmnopqrstuvwxyz | zyxwvutsrqponmlkjihgfedcba |
	| >?@ [\]                    | ]\[ @?>                    |

Scenario Outline: ReverseCipher - Decrypt a string
	Given I have a 'Reverse' cipher
	And my ciphertext is <ciphertext>
	When I decrypt
	Then the plaintext should be <plaintext>
	Examples:
	| plaintext                  | ciphertext                 |
	| ABCDEFGHIJKLMNOPQRSTUVWXYZ | ZYXWVUTSRQPONMLKJIHGFEDCBA |
	| abcdefghijklmnopqrstuvwxyz | zyxwvutsrqponmlkjihgfedcba |
	| >?@ [\]                    | ]\[ @?>                    |