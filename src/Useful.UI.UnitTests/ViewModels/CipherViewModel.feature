Feature: CipherViewModel

@mytag
Scenario: CipherViewModel - Initialization
	Given I have a CipherViewModel
	Then the CurrentCipher is not null
	And the CurrentCipherName is "MoqCipher"
	And the CipherNames are "MoqCipher"
	And the EncryptCommand is not null

Scenario: CipherViewModel - Plaintext property
	Given I have a CipherViewModel
	And I set the Plaintext property
	Then the Plaintext property has changed

Scenario: CipherViewModel - Plaintext property no event subscription
	Given I have a CipherViewModel
	And I set the Plaintext property when the event is not subscribed
	Then the Plaintext property has not changed

Scenario: CipherViewModel - Ciphertext property
	Given I have a CipherViewModel
	And I set the Ciphertext property
	Then the Ciphertext property has changed

Scenario: CipherViewModel - CurrentCipher property
	Given I have a CipherViewModel
	And I set the CurrentCipher property
	Then the CurrentCipher property has changed
	And the CurrentCipherName property has changed

#Scenario: CipherViewModel - CurrentCipherName property
#	Given I have a CipherViewModel
#	And I set the CurrentCipherName property
#	Then the CurrentCipher property has changed
#	And the CurrentCipherName property has changed
	
Scenario: CipherViewModel - Encrypt
	Given I have a CipherViewModel
	And my viewmodel plaintext is "MoqPlaintext"
	When I use the viewmodel to encrypt
	Then the viewmodel ciphertext should be "MoqCiphertext"

Scenario: CipherViewModel - EncryptCommand Executable
	Given I have a CipherViewModel
	And my viewmodel plaintext is "MoqPlaintext"
	Then the EncryptCommand should be Executable
	And the viewmodel ciphertext should be "MoqCiphertext"

Scenario: CipherViewModel - EncryptCommand not Executable
	Given I have a CipherViewModel
	And my viewmodel plaintext is ""
	Then the EncryptCommand should not be Executable

Scenario: CipherViewModel - Decrypt
	Given I have a CipherViewModel
	And my viewmodel ciphertext is "MoqCiphertext"
	When I use the viewmodel to decrypt
	Then the viewmodel plaintext should be "MoqPlaintext"

Scenario: CipherViewModel - DecryptCommand Executable
	Given I have a CipherViewModel
	And my viewmodel ciphertext is "MoqCiphertext"
	Then the DecryptCommand should be Executable
	And the viewmodel plaintext should be "MoqPlaintext"

Scenario: CipherViewModel - DecryptCommand not Executable
	Given I have a CipherViewModel
	And my viewmodel ciphertext is ""
	Then the DecryptCommand should not be Executable