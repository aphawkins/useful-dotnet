Feature: MonoAlphabeticSettingsObservableCollectionSpecFlow
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Test the default settings
	Given I have the default settings
	Then The key will equal "ABCDEFGHIJKLMNOPQRSTUVWXYZ||True"
	And The IV will equal ""
	And The allowed letters will equal "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
	And The cipher name will equal "MonoAlphabetic"
	And The SettingKey will equal ""

Scenario: I can make a simple substitution
	Given I have the default settings
	When I make the substitution "A" to "B"
	Then The letter "A" will encrypt to "B"
	And The letter "B" will encrypt to "A"
	And The letter "B" will decrypt to "A"
	And The letter "A" will decrypt to "B"
	And The substitution count will be 1
	And The SettingKey will equal "AB"

Scenario: I cannot make a substitution using an unallowed letter
	Given I have the default settings
	When I make the substitution "a" to "A"
	Then The letter "A" will encrypt to "A"

Scenario: I cannot make a substitution to an unallowed letter
	Given I have the default settings
	When I make the substitution "A" to "a"
	Then The letter "A" will encrypt to "A"

Scenario: Unallowed letters will pass through
	Given I have the default settings
	Then The letter "a" will encrypt to "a"

Scenario: I get notified when properties change
	Given I have the default settings
	When I make the substitution "A" to "B"
	Then The properties changed are "Item;Key;"

Scenario: I can reset the class
	Given I have the default settings
	And I have the substitution "A" to "B"
	When I Reset the settings
	Then The properties changed are "Item;Key;"
	And The letter "A" will encrypt to "A"

Scenario: I can create a new instance specifying the settings
	When I create a new instance with the key "ABCD|AB CD|True" and IV "" values
	Then The key will be "ABCD|AB CD|True"
	And The IV will be ""

Scenario: I can create a new instance with random settings
	When I create a new random instance
	Then The key will be "ABCD|AB CD|True"
	And The IV will be ""

Scenario: Test invalid keys
	Given I am testing invalid keys:
	| Key                  | IV | Notes                         |
	| ABCD\|AB BC CA\|True |    | Invalid asymmetric key        |
	| ABCD\|AB             |    | Incorrect number of key parts |
	| ABCD \|\|False       |    | Whitespace                    |
	| ABCD\| AB CD \|False |    | Whitespace                    |
	| ABCD\|DE\|False      |    | Disallowed letters            |
	| ABCD\|aB CD\|False   |    | Case sensitive                |
	| ABCD\|AA\|True       |    | Substitution to self          |
	| ABCD\|AB BA\|True    |    | Duplicate substitution        |
	| ABCC\|\|True         |    | Duplicate allowed letter      |
	| ABCD\|\|             |    | Missing symmetry              |
	| ABCD\|\|null         |    | null symmetry                 |
	| ABCD\|\| True        |    | Whitespace                    |
	| "ABCD\|\|True "      |    | Whitespace                    |

Scenario: Test the Monoalphabetic Settings Observable Collection
	Given I am testing MonoAlphabeticSettingsObservableCollection:
	| Key                  | IV | Substitutions | NewKey               | Changed           | Notes                   |
	| ABC\|\|True          |    | AB            | ABC\|AB\|True        | AB,AA;BA,BB       |                         |
	| ABC\|AB\|True        |    | CA            | ABC\|AC\|True        | CA,CC;AC,AB;BB,BA |                         |
	| ABC\|AC\|True        |    | AB            | ABC\|AB\|True        | AB,AC;BA,BB;CC,CA |                         |
	| ABC\|AB\|True        |    | AA            | ABC\|\|True          | AA,AB;BB,BA       |                         |
	| ABC\|\|False         |    | AB            | ABC\|AB\|False       | AB,AA;BA,BB       |                         |
	| ABC\|AB\|False       |    | BC            | ABC\|AB BC CA\|False | BC,BA;CA,CC       |                         |
	| ABC\|AB BC\|False    |    | CA            | ABC\|AB BC CA\|False |                   |                         |
	| ABC\|AB BC CA\|False |    | BB            | ABC\|AC\|False       | BB,BC;AC,AB       |                         |
	| ABC\|AC\|False       |    | AA            | ABC\|\|False         | AA,AC;CC,CA       |                         |
	| ØABC\|ØB\|True       |    |               | ØABC\|ØB\|True       |                   | Unicode                 |
	| ABCD\|\|tRuE         |    |               | ABCD\|\|True         |                   | Case sensitive symmetry |